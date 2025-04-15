using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.TeacherServices;

namespace XCourse.Services.Implementations.TeacherServices
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SessionService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        async public Task<Teacher> GetTeacherByUserId(string userId)
        {
            var teacher = await _unitOfWork.Teachers.FindAsync(t => t.AppUser!.Id == userId);
            return teacher;
        }
        async public Task<Session> GetSessionDetailsById(int sessionId, int teacherId)
        {
            var tempSession = new Session();
            if (sessionId <= 0 || teacherId <= 0)
            {
                return tempSession;
            }

            var session = await _unitOfWork.Sessions.FindAsync(session => session.ID == sessionId, ["Group.GroupDefaults"]);
            if (session.Group!.TeacherID != teacherId)
            {
                return tempSession;
            }

            var subject = await _unitOfWork.Subjects.FindAsync(s => s.ID == session.Group.SubjectID);
            session.Group.Subject = subject;

            return session;
        }
    }
}
