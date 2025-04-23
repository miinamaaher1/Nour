using NetTopologySuite.Geometries;

namespace XCourse.Core.ViewModels.StudentsViewModels
{
    public class CenterVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsGirlsOnly { get; set; }
        public string Governorate { get; set; }
        public string City { get; set; }
        public string Neighborhood { get; set; }
        public string Street { get; set; }
        public byte[]? PreviewPicture { get; set; }
        public bool IncompatibleGender { get; set; }
        public ICollection<RoomVM> AvailbleRooms { get; set; }
        public Point? Location { get; set; }
        public string MapKey { get; set; }
        public decimal? WalletBalance { get; set; }

    }
}
