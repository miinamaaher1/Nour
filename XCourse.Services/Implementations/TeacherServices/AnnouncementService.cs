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
        async public Task<IEnumerable<Announcement>> GetAnnouncementById(int announcementId)
        {
            throw new NotImplementedException();
        }

        async public Task<PostAnnouncementResponseDTO> PostAnnouncement(int teacherId, int[] groupIds, string? announcementBody, string? announcementTitle)
        {
            PostAnnouncementResponseDTO responseDTO = new PostAnnouncementResponseDTO
            {
                Errors = new List<string>(),
                IsValid = true
            };

            Teacher teacher = await _unitOfWork.Teachers.FindAsync(
                t => t.ID == teacherId,
                ["Groups"]
            );

            foreach (var groupId in groupIds)
            {
                bool hasAccess = teacher.Groups!.Any(g => g.ID == groupId);
                if (!hasAccess)
                {
                    responseDTO.IsValid = false;
                    responseDTO.Errors.Add("You don't have access to one of the groups you are trying to announce to.");
                    return responseDTO;
                }
            }

            var TargetGroups = await _unitOfWork.Groups.FindAllAsync(targetGroup => groupIds.Any(id => id == targetGroup.ID));

            try
            {
                Announcement announcement = new Announcement()
                {
                    IsImportant = false,
                    Body = announcementBody,
                    Title = announcementTitle,
                    DateTime = DateTime.Now,
                    Groups = new List<Group>()
                };
                announcement.Groups = TargetGroups.ToList();

                await _unitOfWork.SaveAsync();


                foreach (var group in teacher.Groups!)
                {
                    if (groupIds.Contains(group.ID))
                    {
                        group.Announcements = new List<Announcement>();
                        group.Announcements!.Add(announcement);
                    }
                    await _unitOfWork.SaveAsync();
                }

            }
            catch
            {
                responseDTO.IsValid = false;
                responseDTO.Errors.Add("Something went wrong");
            }

            return responseDTO;
        }

        /*----------------------------------AR12 Area---------------------------------------------------*/
        public async Task<PostAnnouncementResponseDTO> GetAnnouncements (PostAnnouncementRequestDTO announcementRequest, int? take, int? skip)
        {
            var response = new PostAnnouncementResponseDTO
            {
                IsValid = true,
                Errors = new List<string>()
            };

            var teacher =  await _unitOfWork.Teachers.FindAsync(t => t.ID == announcementRequest.TeacherId);
            if (teacher == null)
            {
                response.IsValid = false;
                response.Errors= new List<string> { "Teacher Not Found !" };
                return response;
            }

            var groups = await _unitOfWork.Groups.FindAllAsync(g => g.TeacherID == announcementRequest.TeacherId, includes: ["Announcements"]);
            if (groups == null || !groups.Any())
            {
                response.IsValid = false;
                response.Errors = new List<string> { "Groups not found" };
                return response;
            }

            var announcements = await _unitOfWork.Announcements.FindAllAsync(
                   a => a.Groups.Any(g => announcementRequest.GroupsIds.Contains(g.ID)),
                   includes: ["Groups"]
             );

            if (skip.HasValue)
                announcements = announcements.Skip(skip.Value);
            if (take.HasValue)
                announcements = announcements.Take(take.Value);


            response.Announcements = announcements.Select(a => new AnnouncementDataVM
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

        async public Task<PostAnnouncementResponseDTO> GetAnnouncementsByGroupId(int groupId, int? take,  int? skip)
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
            response.Announcements = announcements.Select(a => new AnnouncementDataVM
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

            var teacher = await _unitOfWork.Teachers.FindAsync(t => t.ID == announcementRequest.TeacherId);
            if (teacher == null)
            {

                response.IsValid = false;
                response.Errors = new List<string> { "Teacher not found" };
                return response;
            }

            var groups = await _unitOfWork.Groups.FindAllAsync(g => announcementRequest.GroupsIds.Contains(g.ID));
            if (groups == null || !groups.Any())
            {

                response.IsValid = false;
                response.Errors = new List<string> { "Groups not found" };
                
            }

            var announcement = new Announcement
            {
                Title = announcementRequest.AnnouncementTitle,
                Body = announcementRequest.AnnouncementBody,
                DateTime = DateTime.Now,
                IsImportant = Convert.ToBoolean( announcementRequest.IsImportant),
                Groups = groups.ToList()
            };

            try
            {
                await _unitOfWork.Announcements.AddAsync(announcement);
                await _unitOfWork.SaveAsync();

                // Map to VM and add to response ======> Talabatak ya zeft Keshk
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

                response.Announcements.Add(announcementVM);
            }
            catch (Exception ex)
            {
                response.IsValid = false;
                response.Errors.Add("Error while adding announcement: " + ex.Message);
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


            var announcement = await _unitOfWork.Announcements.FindAsync(a => a.ID == announcementRequest.AnnouncementID);

            if ( announcement ==null)
            {

                response.IsValid = false;
                response.Errors = new List<string> { "Announcement Not Found !" };
                
                return response;
            }
          
            var groups= await _unitOfWork.Groups.FindAsync(g=> announcementRequest.GroupsIds.Contains(g.ID));
            if (groups == null)
            {

                response.IsValid = false;
                response.Errors = new List<string> { "Groups not found" };

                return response;
            }

            announcement.ID =  Convert.ToInt32(announcementRequest.AnnouncementID);
            announcement.Title = announcementRequest.AnnouncementTitle;
            announcement.Body = announcementRequest.AnnouncementBody;
            announcement.DateTime = DateTime.Now;
            announcement.IsImportant =Convert.ToBoolean(announcementRequest.IsImportant);
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

                response.Announcements.Add(announcementVM);
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
