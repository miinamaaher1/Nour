using System.ComponentModel.DataAnnotations;

namespace XCourse.Core.Entities
{
    [Flags]
    public enum Semester
    {
        First = 1,
        Second = 2
    } 
    public enum Major
    {
        General,
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

        public virtual ICollection<Teacher>? Teachers { get; set; }
        public virtual ICollection<Group>? Groups { get; set; }
        public virtual ICollection<PrivateGroupRequest>? PrivateGroupRequests { get; set; }


        // IsDeleted 
        public bool IsDeleted { get; set; }
    }
}
