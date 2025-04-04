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
        Task<IEnumerable<Subject>> GetMatchingSubjects(RequestSubjectDto request);
        Task<IEnumerable<ResponseCenterDto>> GetAllCentersPerGovernorate(string governorate);
        Task<IEnumerable<Room>> GetAllAvailableRooms(RequestRoomDto request);
        Task<IEnumerable<string>> GetAllGovernorates();
        Task<bool> ReserveAnOfflineGroupInCenter(RequestOfflineGroupReservation request);
        Task<IEnumerable<GroupVM>> GetAllGroups(int userId);
        Task<Teacher> GetTeacherByUserId(string userId);
        Task<GroupDetailsVM> GetGroupDetailsById(int id);
        Task<bool> PostAnnouncement(int groupId, int teacherId, string body, bool isImportnat, string? title);
    }
}
