using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.DTOs.Teachers;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.TeachersViewModels;

namespace XCourse.Services.Interfaces.Teachers
{
    public interface IGroupService 
    {
        Task<ICollection<Subject>> GetMatchingSubjects(RequestSubjectDto request);
        Task<ICollection<Subject>> GetAllSubjects(RequestSubjectDto request);
        Task<ICollection<ResponseCenterDto>> GetAllCentersPerGovernorate(string governorate);
        Task<ICollection<Room>> GetAllAvailableRooms(RequestRoomDto request);
        Task<ReserveGroupResponseDTO> ReserveAnOfflineGroupInCenter(RequestOfflineGroupReservation request);
        Task<ICollection<GroupVM>> GetAllGroups(int userId);
        Task<ICollection<int>> GetAllMatchingYears(int teacherId);
        Task<Teacher> GetTeacherByUserId(string userId);
        Task<GroupDetailsVM> GetGroupDetailsById(int id);
        Task<bool> PostAnnouncement(int groupId, int teacherId, string body, bool isImportnat, string? title);
        Task<ReserveGroupResponseDTO> ReserveOnlineGroup(ReserveOnlineGroupRequestDTO request);
        Task<ReserveGroupResponseDTO> ReserveOfflineLocalGroup(ReserveOfflineLocalGroupRequestDTO request);
    }
}
