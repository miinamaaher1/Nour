using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XCourse.Core.Entities;

namespace XCourse.Infrastructure.Data.Configurations
{
    class CenterConfiguration : IEntityTypeConfiguration<Center>
    {
        public void Configure(EntityTypeBuilder<Center> builder)
        {
            builder.HasOne(c => c.CenterAdmin)
                .WithMany(ca => ca.Centers)
                .HasForeignKey(c => c.CenterAdminID);
        }
    }
}
