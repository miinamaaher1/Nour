using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.DTOs.Teachers;
using XCourse.Core.Entities;

namespace XCourse.Services.Interfaces.TeacherServices
{
    public interface IAnnouncementService
    {
        Task<IEnumerable<Announcement>> GetAnnouncements(int teacherId, int? take, int? skip);
        Task<IEnumerable<Announcement>> GetAnnouncementsByGroubId(int teacherId, int? take, int groupId,int? skip);
        Task<IEnumerable<Announcement>> GetAnnouncementById(int announcementId);
        Task<PostAnnouncementResponseDTO> PostAnnouncement(int teacherId, int[] groupIds, string? announcementBody, string? announcementTitle);
    }
}
