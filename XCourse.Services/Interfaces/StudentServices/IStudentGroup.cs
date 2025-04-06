using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.Students;
using XCourse.Core.ViewModels.StudentsViewModels;

namespace XCourse.Services.Interfaces.StudentServices
{
    public interface IStudentGroup
    {

        public List<StudentGroup> GetStudentGroup(string id);
        public GroupDetails Details(int id);




    }
}
