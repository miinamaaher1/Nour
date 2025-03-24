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
    public class AssistantInvitationConfiguration : IEntityTypeConfiguration<AssistantInvitation>
    {
        public void Configure(EntityTypeBuilder<AssistantInvitation> builder)
        {
            builder.HasKey(entity => new {entity.AssistantID , entity.GroupID});

            builder.HasOne(e => e.Assistant)
                .WithMany(a => a.AssistantInvitations)
                .HasForeignKey(e => e.AssistantID);

            builder.HasOne(e => e.Teacher)
                .WithMany(a => a.AssistantInvitations)
                .HasForeignKey(e => e.TeacherID);

            builder.HasOne(e => e.Group)
                .WithMany(a => a.AssistantInvitations)
                .HasForeignKey(e => e.GroupID);


        }
    }
}
