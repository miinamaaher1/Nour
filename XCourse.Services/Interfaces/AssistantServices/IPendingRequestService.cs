﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.ViewModels.AssistantViewModels;

namespace XCourse.Services.Interfaces.AssistantServices
{
    public interface IPendingRequestService
    {
        Task<ICollection<PendingRequestVM>> GetPendingRequestsAsync(ClaimsPrincipal user);
        Task<PendingRequestVM> FindInvitationRequestByID(int id);

        Task<bool> AcceptInvitationRequest(int id);
        Task<bool> RejectInvitationRequest(int id);

    }
}
