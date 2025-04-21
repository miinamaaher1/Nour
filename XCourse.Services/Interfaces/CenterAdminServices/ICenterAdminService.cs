using XCourse.Core.DTOs.CenterAdmins;
using XCourse.Core.ViewModels.CenterAdminViewModels;
namespace XCourse.Services.Interfaces.CenterAdminServices
{
    public interface ICenterAdminService
    {
        public List<CenterViewModel> GetCenters(string id);
        public List<ReservationViewModel> GetReservations(int id);
        public int ApproveReservation(int id);
        public Task<int> RejectReservation(int id);
        public DetailsReservationViewModel DetailsReservation(int id);
        public int AddNewCenter(CreateCenterViewModel center);
        public int EditCenter(CreateCenterViewModel Center);
        public CreateCenterViewModel GetCenter(int id);
        public int EditRoom(RoomDto room);
        public RoomDto GetRoom(int id);
        public int AddNewRoom(RoomDto room);
        public EditRoomReservation EditRoomReservation(int ReservationID);
        public int UpdateReservtion(EditRoomReservation editRoom);
        public List<RoomsViewModel> getAllRooms(int CenterId);
        public int DeleteRoom(RoomDto room);
        public transfomReservations transfomReservations(int RoomId, int CenterId);
        public int ConfirmTransformRoom(transfomReservations transfom);


        public int DeleteCenter(CreateCenterViewModel center);


        public Task<int> DeleteReservation(DetailsReservationViewModel details);


    }
}
