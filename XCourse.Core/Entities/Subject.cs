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
        public int ID { get; set; }
        public string Topic { get; set; }
        public Major Major { get; set; }
        public Year Year { get; set; }
        public Semester Semester { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; } = new HashSet<Teacher>();
        public virtual ICollection<Group> Groups { get; set; } = new HashSet<Group>();
        public virtual ICollection<PrivateGroupRequest> PrivateGroupRequests { get; set; } = new HashSet<PrivateGroupRequest>();


        // IsDeleted 
        public bool IsDeleted { get; set; }
    }
}
