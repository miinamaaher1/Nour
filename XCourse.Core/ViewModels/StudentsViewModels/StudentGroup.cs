using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;
using NetTopologySuite.Geometries;


namespace XCourse.Core.ViewModels.StudentsViewModels
{
    public class StudentGroup
    {

      public int GroupId {  get; set; }

        
        public Address? Address { get; set; }
        [EnumDataType(typeof(WeekDay))]
        public WeekDay DefaultSessionDays { get; set; }
        public bool IsActive { get; set; } = true; // by default active when initialized

        [Column(TypeName = "geography")]
        public Point? Location { get; set; }

        public bool IsPrivate { get; set; }
        public bool IsOnline { get; set; }


    
        public string TeacherName { get; set; }

        [Column(TypeName = "varbinary(MAX)")]
        [Display(Name = "Cover Picture")]
        public byte[]? CoverPicture { get; set; }


       

        public String Subject { get; set; }
 
    }
}
