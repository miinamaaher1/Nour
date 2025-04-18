using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.ViewModels.TeachersViewModels;

namespace XCourse.Services.Interfaces.TeacherServices
{
    public interface IHomeService
    {
        public Task<TeacherHomeVM> HomeIndexService(string userId);    
    }
}
