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
    class RoomReservationConfiguration : IEntityTypeConfiguration<RoomReservation>
    {
        public void Configure(EntityTypeBuilder<RoomReservation> builder)
        {

            builder.HasOne(r => r.Student)
                .WithMany(s => s.RoomReservations)
                .HasForeignKey(r => r.StudentID);

            builder.HasOne(r => r.Room)
                .WithMany(s => s.RoomReservations)
                .HasForeignKey(r => r.RoomID);

            builder.HasOne(r => r.Session)
                .WithOne(s => s.RoomReservation)
                .HasForeignKey<Session>(s => s.RoomReservationID);

        }
    }
}
