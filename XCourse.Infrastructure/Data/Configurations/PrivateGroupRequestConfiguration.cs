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
    class PrivateGroupRequestConfiguration : IEntityTypeConfiguration<PrivateGroupRequest>
    {
        public void Configure(EntityTypeBuilder<PrivateGroupRequest> builder)
        {
            builder.HasKey(p => new {p.StudentID , p.TeacherID , p.SubjectID});

            builder.HasOne(p => p.Student)
                .WithMany(s => s.PrivateGroupRequests)
                .HasForeignKey(p => p.StudentID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Teacher)
                .WithMany(s => s.PrivateGroupRequests)
                .HasForeignKey(p => p.TeacherID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Subject)
                .WithMany(s => s.PrivateGroupRequests)
                .HasForeignKey(p => p.SubjectID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
