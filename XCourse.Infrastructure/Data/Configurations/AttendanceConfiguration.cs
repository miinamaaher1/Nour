using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Infrastructure.Data.Configurations
{
    class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.HasKey(e => new {e.StudentID , e.SessionID});

            builder.HasOne(e => e.Session)
                .WithMany(s => s.Attendances)
                .HasForeignKey(e => e.SessionID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Student)
                .WithMany(s => s.Attendances)
                .HasForeignKey(e => e.StudentID)
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
