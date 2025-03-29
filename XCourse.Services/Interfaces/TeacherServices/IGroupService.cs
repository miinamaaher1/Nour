using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Services.Interfaces.TeacherServices
{
    interface IGroupService 
    {
        Task<IEnumerable<Subject>> GetMatchingSubjects(Year year);
        Task<IEnumerable<Center>> GetAllCentersPerGovernorate(string governorate);
        Task<IEnumerable<Room>> GetAllAvilableRooms(int centerID, TimeOnly start, TimeOnly end);
    }
}
