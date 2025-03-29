using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.DTOs.Teacher;
using XCourse.Core.Entities;

namespace XCourse.Services.Interfaces.Teacher
{
    public interface IGroupService 
    {
        Task<IEnumerable<Subject>> GetMatchingSubjects(RequestSubjectDto request);
        Task<IEnumerable<ResponseCenterDto>> GetAllCentersPerGovernorate(string governorate);
        Task<IEnumerable<Room>> GetAllAvailableRooms(RequestRoomDto request);
        Task<IEnumerable<string>> GetAllGovernorates();
    }
}
