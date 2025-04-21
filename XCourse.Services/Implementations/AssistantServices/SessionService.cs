using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.AssistantServices;

namespace XCourse.Services.Implementations.AssistantServices
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Assistant> GetAssistantByUserId(string userId)
        {
            var assistant = await _unitOfWork.Assistants.FindAsync(t => t.AppUser!.Id == userId);
            return assistant;
        }
        public async Task<Session> GetSessionDetailsById(int sessionId, int assistantId)
        {
            var tempSession = new Session();
            if (sessionId <= 0 || assistantId <= 0)
            {
                return tempSession;
            }

            var session = await _unitOfWork.Sessions.FindAsync(session => session.ID == sessionId, includes: new[] { "Group.GroupDefaults" });

            var invitation = await _unitOfWork.AssistantInvitations.FindAsync(inv => inv.GroupID == session!.Group!.ID && inv.AssistantID == assistantId);
            if (invitation == null)
            {
                return tempSession;
            }

            var subject = await _unitOfWork.Subjects.FindAsync(s => s.ID == session!.Group!.SubjectID);
            session!.Group!.Subject = subject;

            return session;
        }

    }
}
