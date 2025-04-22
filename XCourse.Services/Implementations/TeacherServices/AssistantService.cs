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
    public class AssistantService : IAssistantService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssistantService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Teacher> GetTeacherByUserId(string userId)
        {
            var teacher = await _unitOfWork.Teachers.FindAsync(t => t.AppUser!.Id == userId);
            return teacher;
        }
        public async Task<Assistant>? GetAssistantById(int id)
        {
            var assistant = await _unitOfWork.Assistants.FindAsync(ass => ass.ID == id, ["AppUser"]);
            return assistant;
        }
        async public Task<List<Group>> GetAccessedGroups(int assistantId, int teacherId)
        {
            if (teacherId == 0 || assistantId == 0)
            {
                return new List<Group>();
            }
            var groups = await _unitOfWork.Groups.FindAllAsync(g => g.TeacherID == teacherId, ["Subject", "GroupDefaults"]);
            var groupsOfAssistant = await _unitOfWork.AssistantInvitations.FindAllAsync(assInv => assInv.AssistantID == assistantId);
            var groupsOfAssistantIds = groupsOfAssistant.Select(ga => ga.GroupID);

            var AccessedGroups = groups.Where(g => groupsOfAssistantIds.Contains(g.ID)).ToList();

            return AccessedGroups;
        }

        // If group = 0  => return all assistants of the teacher
        public async Task<List<Assistant>> GetAssistantsInGroup(int teacherId, int groupId)
        {
            if (teacherId <= 0)
                return new List<Assistant>();

            List<AssistantInvitation> assistantInvitations;

            if (groupId > 0)
            {
                // Validate that the group belongs to the teacher
                var group = await _unitOfWork.Groups.FindAsync(g => g.ID == groupId && g.TeacherID == teacherId);
                if (group == null)
                    return new List<Assistant>();

                assistantInvitations = (await _unitOfWork.AssistantInvitations
                    .FindAllAsync(inv => inv.GroupID == groupId && inv.Status == AssistantInvitationStatus.Accepted))
                    .ToList();
            }
            else
            {
                var groupIds = (await _unitOfWork.Groups
                    .FindAllAsync(g => g.TeacherID == teacherId))
                    .Select(g => g.ID)
                    .ToList();

                assistantInvitations = (await _unitOfWork.AssistantInvitations
                    .FindAllAsync(inv => groupIds.Contains(inv.GroupID) && inv.Status == AssistantInvitationStatus.Accepted))
                    .ToList();
            }

            var assistantIds = assistantInvitations
                .Select(inv => inv.AssistantID)
                .Distinct()
                .ToList();

            var assistants = (await _unitOfWork.Assistants
                .FindAllAsync(a => assistantIds.Contains(a.ID), includes: new[] { "AppUser" }))
                .ToList();

            return assistants;
        }

        public async Task<bool> AssignAssistantToGroup(int assistantId, int groupId, int teacherId)
        {
            if (teacherId <= 0 || groupId <= 0)
                return false;

            // Ensure the group exists and belongs to the teacher
            var group = await _unitOfWork.Groups.FindAsync(g => g.ID == groupId && g.TeacherID == teacherId);
            if (group == null)
                return false;

            // Ensure the assistant exists
            var assistant = await _unitOfWork.Assistants.FindAsync(a => a.ID == assistantId);
            if (assistant == null)
                return false;

            // Check if an invitation already exists
            var existingInvitation = await _unitOfWork.AssistantInvitations
                .FindAsync(inv => inv.AssistantID == assistantId && inv.GroupID == groupId);

            if (existingInvitation == null)
            {
                // Create a new AssistantInvitation
                var newInvitation = new AssistantInvitation
                {
                    AssistantID = assistantId,
                    GroupID = groupId,
                    Status = AssistantInvitationStatus.Pending
                };

                await _unitOfWork.AssistantInvitations.AddAsync(newInvitation);
            }
            else
            {
                existingInvitation.Status = AssistantInvitationStatus.Pending;
                _unitOfWork.AssistantInvitations.Update(existingInvitation);
            }

            try
            {
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        async public Task<bool> RemoveAssistantAccess(int assistantId, int groupId, int teacherId)
        {
            if (teacherId <= 0 || groupId <= 0)
            {
                return false;
            }

            var group = await _unitOfWork.Groups.FindAsync(g => g.ID == groupId);
            if (group == null)
            {
                return false;
            }

            var assistant = await _unitOfWork.Assistants.FindAsync(ass => ass.ID == assistantId);
            if (assistant == null)
            {
                return false;
            }

            var assistantInvitation = await _unitOfWork.AssistantInvitations
                .FindAsync(assInv => assInv.AssistantID == assistant.ID && assInv.GroupID == groupId);

            if (assistantInvitation == null)
            {
                return false;
            }

            // Hard delete the invitation
            _unitOfWork.AssistantInvitations.Delete(assistantInvitation);

            try
            {
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Assistant>> GetRecommendedAssistants(int groupId)
        {
            var assistantInvitationsOfGroup = await _unitOfWork.AssistantInvitations
                .FindAllAsync(assInv => assInv.GroupID == groupId);

            var assignedAssistantsIds = assistantInvitationsOfGroup
                .Select(assInv => assInv.AssistantID)
                .ToList();

            var assistants = await _unitOfWork.Assistants
                .FindAllAsync(ass => !assignedAssistantsIds.Contains(ass.ID), includes: new[] { "AppUser" });

            return assistants.ToList();
        }


        async public Task<Group>? GetGroupById(int groupId, int teacherId)
        {
            if (teacherId <= 0 || groupId <= 0)
            {
                return null;
            }

            var group = await _unitOfWork.Groups.FindAsync(g => g.ID == groupId, ["Subject", "GroupDefaults"]);
            if (group != null && group.TeacherID == teacherId)
            {
                return group;
            }

            return null;
        }
    }
}
