using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.DTOs.StudentDTOs;
using XCourse.Core.Entities;

namespace XCourse.Core.ViewModels.CenterAdminViewModels
{
    public class CreateCenterViewModel
    {
        [Key]
       public int CenterAdminid{set;get;}
        public int CenterID { set; get; }
        public string Name { get; set; }
        public string? Key { set; get; }

        public bool IsGirlsOnly { get; set; }

        public MapInfoDTO Location { set; get; }

        public Address? Address { get; set; }

        [Column(TypeName = "varbinary(MAX)")]
        [Display(Name = "Preview Picture")]
        public byte[]? PreviewPicture { get; set; }


    }
}
