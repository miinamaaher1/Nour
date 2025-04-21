using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.DTOs;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.StudentsViewModels;

namespace XCourse.Services.Interfaces.Student
{
    public interface IStudentHomeService
    {
        public Task<HomeViewModel> IndexService(string id);
        
        public Task<ICollection<SessionViewModel>> SessionIndexService(string guid ,int groupId);

        public Task<SessionDetailsViewModel> SessionDetailsService(int sessionId, string userId);

        public Task<bool> PayForSessionServiceAsync(int sessionId, string userId);

        public Task<bool> SessionSaveFeedbackService(FeedBackDTO feedBackDTO, string userId);

        public Task<bool> SessionRemoveFeedbackService(FeedBackDTO feedBackDTO);


    }
}
