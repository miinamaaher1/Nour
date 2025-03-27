using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XCourse.Core.Entities;

namespace XCourse.Infrastructure.Data
{
    public partial class XCourseContext : IdentityDbContext<AppUser>
    {
        public XCourseContext(DbContextOptions<XCourseContext> options) : base(options)
        {

        }
        public virtual DbSet<Announcement> Announcements { get; set; }
        public virtual DbSet<Assistant> Assistants { get; set; }
        public virtual DbSet<AssistantInvitation> AssistantInvitations { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Center> Centers { get; set; }
        public virtual DbSet<CenterAdmin> CenterAdmins { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<PrivateGroupRequest> PrivateGroupRequests { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomReservation> RoomReservations { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }
        public virtual DbSet<GroupDefaults> GroupDefaults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);
            OnConfiguringPartial(optionsBuilder);
        }
        partial void OnConfiguringPartial(DbContextOptionsBuilder optionsBuilder);
    }
}
