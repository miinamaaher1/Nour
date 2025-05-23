﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Infrastructure.Data.Configurations
{
    class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasOne(g => g.Teacher)
                .WithMany(t => t.Groups)
                .HasForeignKey(g => g.TeacherID);

            builder.HasOne(g => g.DefaultRoom)
                .WithMany(t => t.Groups)
                .HasForeignKey(g => g.DefaultRoomID);

            builder.HasOne(g => g.Subject)
                .WithMany(t => t.Groups)
                .HasForeignKey(g => g.SubjectID);

            builder.HasMany(g => g.Students)
                    .WithMany(t => t.Groups)
                    .UsingEntity<Dictionary<string, object>>(
                        "StudentGroups",
                        j => j.HasOne<Student>().WithMany().HasForeignKey("StudentID").OnDelete(DeleteBehavior.NoAction),
                        j => j.HasOne<Group>().WithMany().HasForeignKey("GroupID").OnDelete(DeleteBehavior.NoAction)
                    );
        }
    }
}
