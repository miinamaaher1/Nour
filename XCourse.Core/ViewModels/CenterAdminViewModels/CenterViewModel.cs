using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;
using NetTopologySuite.Geometries;
using XCourse.Core.DTOs.StudentDTOs;

namespace XCourse.Core.ViewModels.CenterAdminViewModels
{
    public class CenterViewModel
    {
        [Key]
     public int  CenterID{set;get;}
        public string Name { get; set; }
 
       public int CenterAdminid{set;get;}
        public bool IsGirlsOnly { get; set; }

     
        public Address? Address { get; set; }

      
        public byte[]? PreviewPicture { get; set; }





    }
}
