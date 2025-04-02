using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.ViewModels.Students;

namespace XCourse.Services.Interfaces.Student
{
    public interface IStudentHomeService
    { 
       public HomeViewModel IndexService(string  id);
    }
}
