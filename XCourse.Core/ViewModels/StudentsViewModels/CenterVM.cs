namespace XCourse.Core.ViewModels.StudentsViewModels
{
    public class CenterVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsGirlsOnly { get; set; }
        public string Governorate { get; set; }
        public string City { get; set; }
        public string Neighborhood { get; set; }
        public string Street { get; set; }
        public byte[]? PreviewPicture { get; set; }
        public bool IncompatibleGender { get; set; }
        public ICollection<RoomVM> AvailbleRooms { get; set; }

    }
}
