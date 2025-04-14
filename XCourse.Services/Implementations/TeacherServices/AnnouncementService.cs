using Azure.Core;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.DTOs.Teachers;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.TeachersViewModels;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.TeacherServices;

namespace XCourse.Services.Implementations.TeacherServices
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AnnouncementService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }


        async public Task<Teacher> GetTeacherByUserId(string userId)
        {
            var teacher = await _unitOfWork.Teachers.FindAsync(t => t.AppUser!.Id == userId);
            return teacher;
        }
        async public Task<IEnumerable<AnnouncementGroupDTO>> GetAllGroups(int teacherId)
        {
            List<AnnouncementGroupDTO> groupDTOs = new List<AnnouncementGroupDTO>();

            var groups = await _unitOfWork.Groups.FindAllAsync(
                g => g.Teacher!.ID == teacherId && g.IsDeleted == false,
                includes: ["Subject"]
            );

            foreach (var group in groups)
            {
                var groupDTO = new AnnouncementGroupDTO
                {
                    GroupID = group.ID,
                    GroupName = $"{group.Subject?.Topic ?? "No Subject"} - {group.DefaultSessionDays}"
                };

                groupDTOs.Add(groupDTO);
            }

            return groupDTOs;
        }
        public async Task<PostAnnouncementResponseDTO> GetAnnouncements(PostAnnouncementRequestDTO announcementRequest)
        {
            var response = new PostAnnouncementResponseDTO
            {
                IsValid = true,
                Errors = new List<string>()
            };

            // Validate teacher
            var teacher = await _unitOfWork.Teachers.FindAsync(t => t.ID == announcementRequest.TeacherId);
            if (teacher == null)
            {
                response.IsValid = false;
                response.Errors = new List<string> { "Teacher not found." };
                return response;
            }

            // Fetch groups for the teacher
            var groups = await _unitOfWork.Groups.FindAllAsync(
                g => g.TeacherID == announcementRequest.TeacherId,
                includes: new[] { "Announcements" }
            );
            if (groups == null || !groups.Any())
            {
                response.IsValid = false;
                response.Errors = new List<string> { "Groups not found." };
                return response;
            }

            // Fetch announcements based on whether group IDs were provided:
            IEnumerable<Announcement> announcements;
            if (announcementRequest.GroupIds == null || !announcementRequest.GroupIds.Any())
            {
                // No GroupIDs provided: return all announcements for teacher's groups
                announcements = await _unitOfWork.Announcements.FindAllAsync(
                    a => a.Groups.Any(g => g.TeacherID == announcementRequest.TeacherId),
                    includes: new[] { "Groups" }
                );
            }
            else
            {
                // Filter announcements by the provided group IDs.
                announcements = await _unitOfWork.Announcements.FindAllAsync(
                    a => a.Groups.Any(g => announcementRequest.GroupIds.Contains(g.ID)),
                    includes: new[] { "Groups" }
                );
            }

            // Apply pagination
            if (announcementRequest.Skip > 0)
                announcements = announcements.Skip(announcementRequest.Skip);
            if (announcementRequest.Take > 0)
                announcements = announcements.Take(announcementRequest.Take);

            response.Data = announcements.Select(a => new AnnouncementDataVM
            {
                Id = a.ID,
                AnnouncementTitle = a.Title,
                AnnouncementBody = a.Body,
                DateTime = a.DateTime,
                IsImportant = a.IsImportant,
                Groups = a.Groups.Select(g => new AnnouncementGroupDTO
                {
                    GroupID = g.ID,
                    GroupName = $"{g.Subject?.Topic}"
                }).ToList()
            }).ToList();

            return response;
        }

        async public Task<PostAnnouncementResponseDTO> GetAnnouncementsByGroupId(int groupId, int? take, int? skip)
        {
            var response = new PostAnnouncementResponseDTO
            {
                IsValid = true,
                Errors = new List<string>()
            };

            var group = await _unitOfWork.Groups.FindAsync(g => g.ID == groupId);
            if (group == null)
            {
                response.IsValid = false;
                response.Errors = new List<string> { "Group not found" };
                return response;
            }
            var announcements = await _unitOfWork.Announcements.FindAllAsync(
                a => a.Groups!.Any(g => g.ID == groupId)
            );
            if (announcements == null)
            {
                response.IsValid = false;
                response.Errors = new List<string> { "No announcements found for this Group" };
                return response;
            }

            if (skip.HasValue)
                announcements = announcements.Skip(skip.Value);
            if (take.HasValue)
                announcements = announcements.Take(take.Value);
            response.Data = announcements.Select(a => new AnnouncementDataVM
            {
                Id = a.ID,
                AnnouncementTitle = a.Title,
                AnnouncementBody = a.Body,
                DateTime = a.DateTime,
                IsImportant = a.IsImportant,
                Groups = a.Groups.Select(g => new AnnouncementGroupDTO
                {
                    GroupID = g.ID,
                    GroupName = $"{g.Subject?.Topic}"
                }).ToList()
            }).ToList();

            return response;
        }

        public async Task<PostAnnouncementResponseDTO> AddAnnouncementService(PostAnnouncementRequestDTO announcementRequest)
        {
            var response = new PostAnnouncementResponseDTO
            {
                IsValid = true,
                Errors = new List<string>(),
                Data = new List<AnnouncementDataVM>()
            };

            // Validate the request object
            if (announcementRequest == null)
            {
                response.IsValid = false;
                response.Errors.Add("Invalid request.");
                return response;
            }

            if (string.IsNullOrWhiteSpace(announcementRequest.AnnouncementBody))
            {
                response.IsValid = false;
                response.Errors.Add("No content provided!");
                return response;
            }

            if (string.IsNullOrWhiteSpace(announcementRequest.AnnouncementTitle))
            {
                response.IsValid = false;
                response.Errors.Add("No title provided!");
                return response;
            }

            var teacher = await _unitOfWork.Teachers.FindAsync(t => t.ID == announcementRequest.TeacherId);
            if (teacher == null)
            {
                response.IsValid = false;
                response.Errors.Add("Teacher not found.");
                return response;
            }

            var groups = await _unitOfWork.Groups.FindAllAsync(g =>
            g.TeacherID == announcementRequest.TeacherId &&
            announcementRequest.GroupIds!.Contains(g.ID));
            if (groups == null || !groups.Any())
            {
                response.IsValid = false;
                response.Errors.Add("No valid groups were found.");
                return response;
            }

            var announcement = new Announcement
            {
                Title = announcementRequest.AnnouncementTitle,
                Body = announcementRequest.AnnouncementBody,
                DateTime = DateTime.Now,
                IsImportant = Convert.ToBoolean(announcementRequest.IsImportant),
                Groups = groups.ToList()
            };

            try
            {
                await _unitOfWork.Announcements.AddAsync(announcement);
                await _unitOfWork.SaveAsync();

                var announcementVM = new AnnouncementDataVM
                {
                    Id = announcement.ID,
                    AnnouncementTitle = announcement.Title,
                    AnnouncementBody = announcement.Body,
                    DateTime = announcement.DateTime,
                    IsImportant = announcement.IsImportant,
                    Groups = announcement.Groups.Select(g => new AnnouncementGroupDTO
                    {
                        GroupID = g.ID,
                        GroupName = g.Subject?.Topic ?? "Unknown"
                    }).ToList()
                };

                response.Data.Add(announcementVM);
            }
            catch (Exception ex)
            {
                response.IsValid = false;
                response.Errors.Add($"Error while adding announcement: {ex.Message}");
            }

            return response;
        }

        public async Task<PostAnnouncementResponseDTO> EditAnnouncementService(PostAnnouncementRequestDTO announcementRequest)
        {
            var response = new PostAnnouncementResponseDTO()
            {
                IsValid = true,
                Errors = new List<string>()
            };

            if (announcementRequest == null)
            {

                response.IsValid = false;
                response.Errors = new List<string> { "Invalid request" };


                return response;
            }

            if (string.IsNullOrEmpty(announcementRequest.AnnouncementBody))
            {
                response.IsValid = false;
                response.Errors = new List<string> { "No content provided!" };
                return response;

            }

            if (string.IsNullOrEmpty(announcementRequest.AnnouncementTitle))
            {
                response.IsValid = false;
                response.Errors = new List<string> { "No title provided!" };
                return response;
            }


            var announcement = await _unitOfWork.Announcements.FindAsync(a => a.ID == announcementRequest.AnnouncementId);

            if (announcement == null)
            {

                response.IsValid = false;
                response.Errors = new List<string> { "Announcement Not Found !" };

                return response;
            }

            var groups = await _unitOfWork.Groups.FindAllAsync(g => announcementRequest.GroupIds!.Contains(g.ID));

            if (groups == null)
            {

                response.IsValid = false;
                response.Errors = new List<string> { "Groups not found" };

                return response;
            }

            announcement.ID = Convert.ToInt32(announcementRequest.AnnouncementId);
            announcement.Title = announcementRequest.AnnouncementTitle;
            announcement.Body = announcementRequest.AnnouncementBody;
            announcement.DateTime = DateTime.Now;
            announcement.IsImportant = Convert.ToBoolean(announcementRequest.IsImportant);
            announcement.Groups = (ICollection<Group>?)groups;
            try
            {
                _unitOfWork.Announcements.Update(announcement);
                _unitOfWork.SaveAsync();

                var announcementVM = new AnnouncementDataVM
                {
                    Id = announcement.ID,
                    AnnouncementTitle = announcement.Title,
                    AnnouncementBody = announcement.Body,
                    DateTime = announcement.DateTime,
                    IsImportant = announcement.IsImportant,
                    Groups = announcement.Groups.Select(g => new AnnouncementGroupDTO
                    {
                        GroupID = g.ID,
                        GroupName = $"{g.Subject?.Topic}"
                    }).ToList()
                };

                response.Data.Add(announcementVM);
            }
            catch (Exception ex)
            {
                response.IsValid = false;
                response.Errors = new List<string> { "Error while updating announcement: " + ex.Message };
            }

            return response;

        }

        public Task<PostAnnouncementResponseDTO> DeleteAnnouncementService(int announcementId)
        {


            var response = new PostAnnouncementResponseDTO
            {
                IsValid = true,
                Errors = new List<string>()
            };
            var announcement = _unitOfWork.Announcements.Find(a => a.ID == announcementId);
            if (announcement == null)
            {
                response.IsValid = false;
                response.Errors.Add("Announcement not found");
                return Task.FromResult(response);
            }
            _unitOfWork.Announcements.Delete(announcement);
            _unitOfWork.Save();
            return Task.FromResult(response);


        }
    }
}
