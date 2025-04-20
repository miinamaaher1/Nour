using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.StudentsViewModels;

namespace XCourse.Core.ViewModels.AssistantViewModels
{
    public class AssistantGroupDetails
    {
        [Key]
        public int Id { get; set; }
        public Address? Address { get; set; }
        public Point? Location { get; set; }
        public string Key { set; get; }
        public virtual Room? DefaultRoom { get; set; }
        public virtual ICollection<Session>? Sessions { get; set; }
        public List<DefaultTimeVM> Defaults { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsOnline { get; set; }
        public Teacher Teacher { get; set; }
        public byte[]? CoverPicture { get; set; }
        public Subject Subject { get; set; }
        public decimal PricePerSession { get; set; }
    }
}
