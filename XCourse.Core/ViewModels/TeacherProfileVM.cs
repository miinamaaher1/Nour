using XCourse.Core.DTOs;

namespace XCourse.Core.ViewModels
{
    public class TeacherProfileVM
    {
        public int TeacherID { get; set; }
        public string TeacherName { get; set; }
        public byte[] TeacherProfilePicture { get; set; }
        public List<CompactGroupDTO> AvailbleGroups { get; set; }
    }
}
