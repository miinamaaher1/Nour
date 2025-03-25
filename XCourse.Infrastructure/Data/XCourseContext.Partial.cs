using Microsoft.EntityFrameworkCore;
using XCourse.Core.Entities;
using XCourse.Infrastructure.Data.Configurations;
using XCourse.Infrastructure.Data.Interceptors;

namespace XCourse.Infrastructure.Data
{
    public partial class XCourseContext
    {

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            // Query filtering for IsDeleting
            modelBuilder.Entity<AppUser>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Assistant>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Attendance>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<CenterAdmin>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Center>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Group>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<RoomReservation>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Session>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Student>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Subject>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Teacher>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Wallet>().HasQueryFilter(e => !e.IsDeleted);

            // Mapping Configurations
            modelBuilder.ApplyConfiguration(new AnnouncementConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new AssistantInvitationConfiguration());
            modelBuilder.ApplyConfiguration(new AttendanceConfiguration());
            modelBuilder.ApplyConfiguration(new CenterEntityConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new PrivateGroupRequestConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
            modelBuilder.ApplyConfiguration(new RoomReservationConfiguration());
            modelBuilder.ApplyConfiguration(new SessionConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectConfiguration());
            modelBuilder.ApplyConfiguration(new WalletConfiguration());
        }
        partial void OnConfiguringPartial(DbContextOptionsBuilder optionsBuilder)
        {
            // soft delete interceptors
            optionsBuilder.AddInterceptors(new AppUserInterceptor()).AddInterceptors(new AssistantInterceptor())
                .AddInterceptors(new AttendanceInterceptor()).AddInterceptors(new CenterAdminInterceptor())
                .AddInterceptors(new CenterInterceptor()).AddInterceptors(new GroupInterceptor())
                .AddInterceptors(new RoomReservationInterceptor()).AddInterceptors(new SessionInterceptor())
                .AddInterceptors(new StudentInterceptor()).AddInterceptors(new SubjectInterceptor())
                .AddInterceptors(new TeacherInterceptor()).AddInterceptors(new WalletInterceptor());
        }

    }
}
