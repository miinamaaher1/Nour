using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace XCourse.Core.ViewModels.TeachersViewModels
{
    public class SubjectCreateVM
    {
        public string Topic { get; set; }
        public IEnumerable<SelectListItem> Topics { get; set; }
        public XCourse.Core.Entities.Major Major { get; set; }
        public XCourse.Core.Entities.Year Year { get; set; }
        public XCourse.Core.Entities.Semester Semester { get; set; }
    }
}
