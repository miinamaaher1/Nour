using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.AssistantViewModels;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.AssistantServices;

namespace XCourse.Services.Implementations.AssistantServices
{
    public class PendingRequestService : IPendingRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;


        public PendingRequestService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public Task<bool> AcceptInvitationRequest(int id)
        {
            var invitationRequest = _unitOfWork.AssistantInvitations.Find(x => x.ID == id);
            if (invitationRequest != null)
            {
                invitationRequest.Status = AssistantInvitationStatus.Accepted;
                _unitOfWork.AssistantInvitations.Update(invitationRequest);
                _unitOfWork.Save();
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }   

        }
        public Task<bool> RejectInvitationRequest(int id)
        {
            var invitationRequest = _unitOfWork.AssistantInvitations.Find(x => x.ID == id);
            if (invitationRequest != null)
            {
                invitationRequest.Status = AssistantInvitationStatus.Rejected;
                _unitOfWork.AssistantInvitations.Update(invitationRequest);
                _unitOfWork.Save();
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }
        public async Task<PendingRequestVM> FindInvitationRequestByID(int id)
        {
            var invitationRequest = await _unitOfWork.AssistantInvitations
                .FindAsync(x => x.ID == id, includes: new[] { "Group.Teacher.AppUser", "Group.Subject" });
            PendingRequestVM pendingRequest = new PendingRequestVM
            {
                Status = invitationRequest.Status,
                InvitationID = invitationRequest.ID,
                GroupID = invitationRequest.GroupID,
                TeacherID = invitationRequest.Group.TeacherID,
                GroupDescription = invitationRequest.Group.Description,
                TeacherName = invitationRequest.Group.Teacher.AppUser.FirstName + " " + invitationRequest.Group.Teacher.AppUser.LastName,
                SubjectName = invitationRequest.Group.Subject.Topic,
                Major = invitationRequest.Group.Subject.Major ,
                Year = invitationRequest.Group.Subject.Year,
                Semester = invitationRequest.Group.Subject.Semester,
            };
            return pendingRequest;

        }

        public async Task<ICollection<PendingRequestVM>> GetPendingRequestsAsync(ClaimsPrincipal user)
        {
            var appUser = await _userManager.GetUserAsync(user);
            var assistant = await _unitOfWork.Assistants.FindAsync(a => a.AppUserID == appUser.Id);

            if (assistant == null)
                return new List<PendingRequestVM>();

            var pendingRequests = await _unitOfWork.AssistantInvitations
                .FindAllAsync(
                    x => x.AssistantID == assistant.ID && x.Status == AssistantInvitationStatus.Pending,
                    includes: new[] { "Group.Teacher.AppUser", "Group.Teacher.Subjects" }
                );

            return pendingRequests.Select(x => new PendingRequestVM
            {
                Status = x.Status,
                InvitationID = x.ID,
                GroupID = x.GroupID,
                TeacherID = x.Group.TeacherID,
                GroupDescription = x.Group.Description,
                TeacherName = x.Group.Teacher.AppUser.FirstName + " " + x.Group.Teacher.AppUser.LastName,
                SubjectName = x.Group.Subject.Topic,
                Major = x.Group.Subject.Major,
                Year = x.Group.Subject.Year,
                Semester = x.Group.Subject.Semester,

            }).ToList();
        }
    }
}
