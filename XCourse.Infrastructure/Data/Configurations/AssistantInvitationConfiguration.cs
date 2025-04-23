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
    public class AssistantInvitationConfiguration  : IEntityTypeConfiguration<AssistantInvitation>
    {
        public void Configure(EntityTypeBuilder<AssistantInvitation> builder)
        {
            builder.HasOne(e => e.Assistant)
                .WithMany(a => a.AssistantInvitations)
                .HasForeignKey(e => e.AssistantID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Group)
                .WithMany(a => a.AssistantInvitations)
                .HasForeignKey(e => e.GroupID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
