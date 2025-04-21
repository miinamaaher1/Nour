using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.ViewModels.AssistantViewModels;
using XCourse.Core.ViewModels.StudentsViewModels;

namespace XCourse.Services.Interfaces.AssistantServices
{
    public interface IGroupDetailsService
    {
        public AssistantGroupDetails Details(int id);
    }
}
