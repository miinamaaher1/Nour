using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Services.Interfaces.AssistantServices
{
    public interface ISessionService
    {
        public Task<Assistant> GetAssistantByUserId(string userId);
        public Task<Session> GetSessionDetailsById(int sessionId, int assistantId);
    }
}
