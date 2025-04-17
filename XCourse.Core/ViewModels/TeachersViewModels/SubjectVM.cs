using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Core.ViewModels.TeachersViewModels
{
    class SubjectVM
    {
        public int ID { get; set; }
        public string Topic { get; set; }
        public Major Magor { get; set; }
        public Year Year { get; set; }
        public Semester Semester { get; set; }
        
    }
}
