using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.ViewModels.CenterAdminViewModels;
using XCourse.Core.ViewModels.StudentsViewModels;

namespace XCourse.Services.Interfaces.CenterAdminServices
{
    public interface ICenterAdminHomeService
    {

        public  Task<List<RoomHomeViewModel>> GetAllRooms(string id);

        Task<List<TopCenterViewModel>> GetTopCenter(string id);
    }
}
