using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Services.Interfaces.AssistantServices;
using XCourse.Core.ViewModels.AssistantViewModels;
using System.Security.Claims;
using XCourse.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using XCourse.Core.Entities;

namespace XCourse.Services.Implementations.AssistantServices
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public GroupService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<ICollection<GroupVM>> GetAllAcceptedGroupsAsync(ClaimsPrincipal user)
        {
            var appUser = await _userManager.GetUserAsync(user);

            var assistant = await _unitOfWork.Assistants.FindAsync(a => a.AppUserID == appUser.Id);

            if (assistant == null)
                return new List<GroupVM>();

            var acceptedRequests = await _unitOfWork.AssistantInvitations
                .FindAllAsync(
                    x => x.AssistantID == assistant.ID && x.Status == AssistantInvitationStatus.Accepted
                );

            var acceptedGroups = await _unitOfWork.Groups
                .FindAllAsync(
                    x => acceptedRequests.Select(y => y.GroupID).Contains(x.ID),
                    includes: new[] { "Teacher.AppUser","Subject" }
                );
            return acceptedGroups.Select(x => new GroupVM
            {
                GroupID = x.ID,
                IsOnline = x.IsOnline,
                IsPrivate = x.IsPrivate,
                IsGirlsOnly = x.IsGirlsOnly,
                CurrentStudents = x.CurrentStudents,
                MaxStudents = x.MaxStudents,
                PricePerSession = x.PricePerSession,
                Address = x.Address,
                Subject = x.Subject,
                GroupPicture = x.CoverPicture,
                TeacherName = x.Teacher?.AppUser?.FirstName + x.Teacher?.AppUser?.LastName,
                TeacherPicture = x.Teacher?.AppUser?.ProfilePicture,
                DefaultSessionDays = x.DefaultSessionDays
            }).ToList();

        }
    }
    
}
