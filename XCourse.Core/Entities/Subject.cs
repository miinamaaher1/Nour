using System.ComponentModel.DataAnnotations;

namespace XCourse.Core.Entities
{
    public enum Semester
    {
        First,
        Second
    } 
    public enum Major
    {
        Science,
        Math,
        Literature
    }
    public enum Year
    {
        PrimaryOne,
        PrimaryTwo,
        PrimaryThree,
        PrimaryFour,
        PrimaryFive,
        PrimarySix,
        PreparatoryOne,
        PreparatoryTwo,
        PreparatoryThree,
        SecondaryOne,
        SecondaryTwo,
        SecondaryThree
    }
    public class Subject
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(25, ErrorMessage = "Number of characters for Topic must be less than or equal 25")]
        public string Topic { get; set; }

        [EnumDataType(typeof(Major))]
        public Major Major { get; set; }

        [EnumDataType(typeof(Year))]
        public Year Year { get; set; }

        [EnumDataType(typeof(Semester))]
        public Semester Semester { get; set; }

        public virtual ICollection<Teacher> Teachers { get; set; } = new HashSet<Teacher>();
        public virtual ICollection<Group> Groups { get; set; } = new HashSet<Group>();
        public virtual ICollection<PrivateGroupRequest> PrivateGroupRequests { get; set; } = new HashSet<PrivateGroupRequest>();


        // IsDeleted 
        public bool IsDeleted { get; set; }
    }
}
