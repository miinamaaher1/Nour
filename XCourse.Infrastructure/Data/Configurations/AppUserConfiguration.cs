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
    class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasOne(u => u.Wallet)
                   .WithOne(w => w.AppUser)
                   .HasForeignKey<Wallet>(w => w.AppUserID);

            builder.HasOne(u => u.Student)
                   .WithOne(s => s.AppUser)
                   .HasForeignKey<Student>(s => s.AppUserID);

            builder.HasOne(u => u.Teacher)
                   .WithOne(t => t.AppUser)
                   .HasForeignKey<Teacher>(t => t.AppUserID);

            builder.HasOne(u => u.Assistant)
                   .WithOne(a => a.AppUser)
                   .HasForeignKey<Assistant>(a => a.AppUserID);

            builder.HasOne(u => u.CenterAdmin)
                   .WithOne(c => c.AppUser)
                   .HasForeignKey<CenterAdmin>(c => c.AppUserID);
        }
    }
}
