﻿using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
    [Flags]
    public enum WeekDay
    {
        None = 0,
        Saturday = 1,    
        Sunday = 2,    
        Monday = 4,    
        Tuesday = 8,    
        Wednesday = 16,   
        Thursday = 32,   
        Friday = 64    
    }

    public class Group
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Maximum no. of Students")]
        public int MaxStudents { get; set; }
        [Display(Name = "Current no. of Students")]
        public int CurrentStudents { get; set; }

        [Display(Name = "Price per Session")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerSession { get; set; }

        public Address? Address { get; set; }
        [EnumDataType(typeof(WeekDay))]
        public WeekDay DefaultSessionDays { get; set; }
        public bool IsActive { get; set; } = true; // by default active when initialized

        [Column(TypeName = "geography")]
        public Point? Location { get; set; }
        
        public bool IsPrivate { get; set; }
        public bool IsOnline { get; set; }
        public bool IsGirlsOnly { get; set; }

        public string? Description { get; set; }

        [ForeignKey(nameof(Teacher))]
        [Display(Name = nameof(Teacher))]
        public int TeacherID { get; set; }
        public virtual Teacher? Teacher { get; set; }

        [Column(TypeName = "varbinary(MAX)")]
        [Display(Name = "Cover Picture")]
        public byte[]? CoverPicture { get; set; }


        [ForeignKey(nameof(Room))]
        [Display(Name = nameof(Room))]
        public int? DefaultRoomID { get; set; }

        public virtual Room? DefaultRoom { get; set; }

        [ForeignKey(nameof(Subject))]
        [Display(Name = nameof(Subject))]
        public int SubjectID { get; set; }
        public virtual Subject? Subject { get; set; }
        public virtual ICollection<AssistantInvitation>? AssistantInvitations { get; set; }
        public virtual ICollection<Student>? Students { get; set; }
        public virtual ICollection<Announcement>? Announcements { get; set; }
        public virtual ICollection<Session>? Sessions { get; set; }
        public virtual ICollection<GroupDefaults>? GroupDefaults { get; set; }


        // IsDeleted
        public bool IsDeleted { get; set; }

    }
}
