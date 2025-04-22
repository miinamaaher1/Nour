namespace XCourse.Core.ViewModels.CenterAdminViewModels
{
    public class IndexHomeViewModel
    {
        public string UserName { get; set; }
        public List<TopCenterViewModel> topCenterViews = new List<TopCenterViewModel>();
        public List<RoomHomeViewModel> roomHomeViewModels = new List<RoomHomeViewModel>();
    }
}
