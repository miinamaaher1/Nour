using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.DTOs.CenterAdmins;
using XCourse.Core.ViewModels.CenterAdminViewModels;
namespace XCourse.Services.Interfaces.CenterAdminServices
{
    public interface ICenterAdminService
    {
        public List<CenterViewModel> GetCenters(string id);

        
       public int AddNewCenter(CreateCenterViewModel center);

        public int EditRoom(RoomDto room);

        public RoomDto GetRoom(int id);
        public int AddNewRoom(RoomDto room);


        public List<RoomsViewModel> getAllRooms(int CenterId);



       
       


    }
}
