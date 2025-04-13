using Azure.Core;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.DTOs.Teachers;
using XCourse.Core.Entities;
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
        public async Task<ICollection<AnnouncementResponseDTO>> GetAnnouncements(int teacherId, int? take, int? skip)
        {
            // Retrieve teacher's groups with the necessary includes.
            var teacherGroups = await _unitOfWork.Groups.FindAllAsync(
                g => g.TeacherID == teacherId,
                new[] { "Subject", "GroupDefaults" }
            );

            // Map groups to DTOs.
            var teacherGroupsDTOs = new List<AnnouncementGroupDTO>();
            foreach (var group in teacherGroups)
            {
                var announcementGroupDTO = new AnnouncementGroupDTO
                {
                    GroupID = group.ID
                };

                var groupDefaultsName = new StringBuilder();
                if (group.GroupDefaults != null)
                {
                    foreach (var def in group.GroupDefaults)
                    {
                        groupDefaultsName.Append($"[{def.WeekDay} - {def.StartTime}]");
                    }
                }

                announcementGroupDTO.GroupName = $"{group.Subject?.Topic} - {groupDefaultsName}";
                teacherGroupsDTOs.Add(announcementGroupDTO);
            }
            // Extract group IDs
            var groupIds = teacherGroupsDTOs.Select(tg => tg.GroupID);

            // Query announcements.
            List<Announcement> announcements = new List<Announcement>(
                await _unitOfWork.Announcements.FindAllAsync(
                    a => a.Groups!.Any(g => groupIds.Contains(g.ID)),
                    new[] { "Groups" },
                    skip,
                    take
                    
                )
            );

            var announcementResponseDTOs = new List<AnnouncementResponseDTO>();
            foreach (var announcement in announcements)
            {
                var announcementDTO = new AnnouncementResponseDTO
                {
                    Id = announcement.ID,
                    AnnouncementBody = announcement.Body,
                    AnnouncementTitle = announcement.Title,
                    DateTime = announcement.DateTime
                };

                var announcementGroupDTOs = new List<AnnouncementGroupDTO>();
                if (announcement.Groups != null)
                {
                    foreach (var group in announcement.Groups)
                    {
                        // Find matching group based on group ID.
                        var matchingGroupDTO = teacherGroupsDTOs.FirstOrDefault(ag => ag.GroupID == group.ID);
                        if (matchingGroupDTO != null)
                        {
                            announcementGroupDTOs.Add(matchingGroupDTO);
                        }
                    }
                }
                announcementDTO.Groups = announcementGroupDTOs;
                announcementResponseDTOs.Add(announcementDTO);
            }

            return announcementResponseDTOs;
        }

        async public Task<IEnumerable<Announcement>> GetAnnouncementsByGroubId(int teacherId, int? take, int groupId, int? skip)
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

    }
}
