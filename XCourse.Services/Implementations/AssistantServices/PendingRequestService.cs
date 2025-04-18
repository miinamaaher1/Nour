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

namespace XCourse.Services.Implementations.AssistantServices
{
    public class PendingRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;


        public PendingRequestService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<ICollection<PendingRequestVM>> GetPendingRequests(ClaimsPrincipal user)
        {
            var appUser = await _userManager.GetUserAsync(user);
            //var userID = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var assistant = await _unitOfWork.Assistants.FindAsync(a => a.AppUserID == appUser.Id);

            if (assistant == null)
                return new List<PendingRequestVM>();

            var pendingRequests = await _unitOfWork.AssistantInvitations.FindAllAsync(
                x => x.AssistantID == assistant.ID && x.Status == AssistantInvitationStatus.Pending);

            return pendingRequests.Select(x => new PendingRequestVM
            {
                Status = x.Status
            }).ToList();
        }
    }
}
