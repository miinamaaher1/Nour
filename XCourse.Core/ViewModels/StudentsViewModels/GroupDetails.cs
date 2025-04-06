using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace XCourse.Core.ViewModels.StudentsViewModels
{
    public class GroupDetails
    {
        [Key]
        public int Id { get; set; } 
        public Address? Address { get; set; }
        public Point? Location { get; set; }
        public string Key { set; get; }
        public virtual Room? DefaultRoom { get; set; }

        public virtual ICollection<Session>? Sessions { get; set; }
        public WeekDay DefaultSessionDays { get; set; }
        public bool IsActive { get; set; } = true; 
        public bool IsPrivate { get; set; }
        public bool IsOnline { get; set; }
        public Teacher Teacher { get; set; }
        public byte[]? CoverPicture { get; set; }

        public String Subject { get; set; }

    }
}
