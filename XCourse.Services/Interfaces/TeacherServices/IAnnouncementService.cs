using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.DTOs.Teachers;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.TeachersViewModels;

namespace XCourse.Services.Interfaces.TeacherServices
{
    public interface IAnnouncementService
    {
        Task<Teacher> GetTeacherByUserId(string userId);
        Task<IEnumerable<AnnouncementGroupDTO>> GetAllGroups(int teacherId);
        Task<PostAnnouncementResponseDTO> GetAnnouncements(PostAnnouncementRequestDTO announcementRequest);
        Task<PostAnnouncementResponseDTO> GetAnnouncementsByGroupId(int groupId, int? take, int? skip);
        Task<PostAnnouncementResponseDTO> AddAnnouncementService( PostAnnouncementRequestDTO AnnouncementRequest);
        Task<PostAnnouncementResponseDTO> EditAnnouncementService(PostAnnouncementRequestDTO announcementRequest);
        Task<PostAnnouncementResponseDTO> DeleteAnnouncementService(int announcementId);

    }
}
