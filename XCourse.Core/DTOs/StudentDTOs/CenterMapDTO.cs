namespace XCourse.Core.DTOs.StudentDTOs
{
    public class CenterMapDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsGirlsOnly { get; set; }
        public string Governorate { get; set; }
        public string City { get; set; }
        public string Neighborhood { get; set; }
        public string Street { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
