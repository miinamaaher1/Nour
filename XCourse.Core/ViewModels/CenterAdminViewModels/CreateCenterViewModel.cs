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

        [MaxLength(80, ErrorMessage = "Number of characters for center's name must be less than or equal 80")]
        public string Name { get; set; }

        public bool IsGirlsOnly { get; set; }

        public MapInfoDTO Location { set; get; }

        public Address? Address { get; set; }

        [Column(TypeName = "varbinary(MAX)")]
        [Display(Name = "Preview Picture")]
        public byte[]? PreviewPicture { get; set; }

        public readonly List<string> Governorates =
        [
            "Cairo", "Alexandria", "Giza", "Port Said", "Suez",
            "Dakahlia", "Sharqia", "Qalyubia", "Beheira", "Minya",
            "Helwan", "6th of October", "Ismailia", "Gharbia", "Monufia",
            "Kafr El Sheikh", "Faiyum", "Beni Suef", "Asyut", "Sohag",
            "Qena", "Aswan", "Luxor", "Red Sea", "New Valley",
            "Matrouh", "North Sinai", "South Sinai", "Damietta"
        ];
        public string? Description { get; set; }
    }
}
