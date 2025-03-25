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
            builder.HasOne(a => a.Wallet)
                   .WithOne(w => w.AppUser)
                   .HasForeignKey<AppUser>(w => w.WalletID);

            builder.HasOne(a => a.Student)
                   .WithOne(w => w.AppUser)
                   .HasForeignKey<Student>(w => w.AppUserID);

            builder.HasOne(a => a.Teacher)
                   .WithOne(w => w.AppUser)
                   .HasForeignKey<Teacher>(w => w.AppUserID);

            builder.HasOne(a => a.Assistant)
                   .WithOne(w => w.AppUser)
                   .HasForeignKey<Assistant>(w => w.AppUserID);

            builder.HasOne(a => a.CenterAdmin)
                   .WithOne(w => w.AppUser)
                   .HasForeignKey<CenterAdmin>(w => w.AppUserID);
        }
    }
}
