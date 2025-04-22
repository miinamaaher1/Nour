using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Services.Interfaces.TeacherServices
{
    public interface IAssistantService
    {
        Task<Teacher> GetTeacherByUserId(string userId);
        Task<List<Group>> GetAccessedGroups(int assistantId, int teacherId);
        Task<Assistant>? GetAssistantById(int id);
        Task<List<Assistant>> GetAssistantsInGroup(int teacherId, int groupId);
        Task<bool> AssignAssistantToGroup(int assistantId, int groupId, int teacherId);
        Task<bool> RemoveAssistantAccess(int assistantId, int groupId, int teacherId);
        Task<List<Assistant>> GetRecommendedAssistants(int groupId);
        Task<Group>? GetGroupById(int groupId, int techerId);
    }
}
