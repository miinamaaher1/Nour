using XCourse.Core.Entities;

namespace XCourse.Core.ViewModels.StudentsViewModels
{
    public class RoomVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public Equipment Equipment { get; set; }
        public decimal PricePerHour { get; set; }
        public byte[]? PreviewPicture { get; set; }
    }
}
