using Microsoft.EntityFrameworkCore;
using XCourse.Core.Entities;

namespace XCourse.Infrastructure.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<AppUser> AppUsers { get; }
        IRepository<Announcement> Announcements { get; }
        IRepository<Assistant> Assistants { get; }
        IRepository<AssistantInvitation> AssistantInvitations { get; }
        IRepository<Attendance> Attendances { get; }
        IRepository<Center> Centers { get; }
        IRepository<CenterAdmin> CenterAdmins { get; }
        IRepository<Group> Groups { get; }
        IRepository<PrivateGroupRequest> PrivateGroupRequests { get; }
        IRepository<Room> Rooms { get; }
        IRepository<RoomReservation> RoomReservations { get; }
        IRepository<Session> Sessions { get; }
        IRepository<Student> Students { get; }
        IRepository<Subject> Subjects { get; }
        IRepository<Teacher> Teachers { get; }
        IRepository<Transaction> Transactions { get; }
        IRepository<Wallet> Wallets { get; }

        int Save();
        Task<int> SaveAsync();
    }
}