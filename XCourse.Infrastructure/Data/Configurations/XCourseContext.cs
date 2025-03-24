using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XCourse.Core.Entities;
using XCourse.Infrastructure.Data.Interceptors;

namespace XCourse.Infrastructure.Data
{
    public partial class XCourseContext
    {

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
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
        }
        partial void OnConfiguringPartial(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new AppUserInterceptor()).AddInterceptors(new AssistantInterceptor())
                .AddInterceptors(new AttendanceInterceptor()).AddInterceptors(new CenterAdminInterceptor()).
                AddInterceptors(new CenterInterceptor()).AddInterceptors(new GroupInterceptor()).
                AddInterceptors(new RoomReservationInterceptor()).AddInterceptors(new SessionInterceptor()).
                AddInterceptors(new StudentInterceptor()).AddInterceptors(new SubjectInterceptor()).
                AddInterceptors(new TeacherInterceptor()).AddInterceptors(new WalletInterceptor());


        }

    }
}
