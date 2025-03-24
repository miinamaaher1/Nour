using XCourse.Core.Entities;
using XCourse.Infrastructure.Data;
using XCourse.Infrastructure.Repositories;
using XCourse.Infrastructure.Repositories.Interfaces;

namespace Xcourse.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly XCourseContext context;

        public IRepository<AppUser> AppUsers { get; }
        public IRepository<Announcement> Announcements { get; }
        public IRepository<Assistant> Assistants { get; }
        public IRepository<AssistantInvitation> AssistantInvitations { get; }
        public IRepository<Attendance> Attendances { get; }
        public IRepository<Center> Centers { get; }
        public IRepository<CenterAdmin> CenterAdmins { get; }
        public IRepository<Group> Groups { get; }
        public IRepository<PrivateGroupRequest> PrivateGroupRequests { get; }
        public IRepository<Room> Rooms { get; }
        public IRepository<RoomReservation> RoomReservations { get; }
        public IRepository<Session> Sessions { get; }
        public IRepository<Student> Students { get; }
        public IRepository<Subject> Subjects { get; }
        public IRepository<Teacher> Teachers { get; }
        public IRepository<Transaction> Transactions { get; }
        public IRepository<Wallet> Wallets { get; }

        public UnitOfWork(XCourseContext _context)
        {
            context = _context;

            AppUsers = new BaseRepository<AppUser>(context);
            Announcements = new BaseRepository<Announcement>(context);
            Assistants = new BaseRepository<Assistant>(context);
            AssistantInvitations = new BaseRepository<AssistantInvitation>(context);
            Attendances = new BaseRepository<Attendance>(context);
            Centers = new BaseRepository<Center>(context);
            CenterAdmins = new BaseRepository<CenterAdmin>(context);
            Groups = new BaseRepository<Group>(context);
            PrivateGroupRequests = new BaseRepository<PrivateGroupRequest>(context);
            Rooms = new BaseRepository<Room>(context);
            RoomReservations = new BaseRepository<RoomReservation>(context);
            Sessions = new BaseRepository<Session>(context);
            Students = new BaseRepository<Student>(context);
            Subjects = new BaseRepository<Subject>(context);
            Teachers = new BaseRepository<Teacher>(context);
            Transactions = new BaseRepository<Transaction>(context);
            Wallets = new BaseRepository<Wallet>(context);
        }

        public int Save()
        {
            return context.SaveChanges();
        }
        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
