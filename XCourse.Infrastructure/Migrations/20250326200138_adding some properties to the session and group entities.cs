using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XCourse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addingsomepropertiestothesessionandgroupentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sessions_RoomReservationID",
                table: "Sessions");

            migrationBuilder.RenameColumn(
                name: "ExpiryDateTime",
                table: "Sessions",
                newName: "StartDateTime");

            migrationBuilder.AlterColumn<int>(
                name: "RoomReservationID",
                table: "Sessions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateTime",
                table: "Sessions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "DefaultEndTime",
                table: "Groups",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefaultSessionDays",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "DefaultStartTime",
                table: "Groups",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Groups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_RoomReservationID",
                table: "Sessions",
                column: "RoomReservationID",
                unique: true,
                filter: "[RoomReservationID] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sessions_RoomReservationID",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "EndDateTime",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "DefaultEndTime",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "DefaultSessionDays",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "DefaultStartTime",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "StartDateTime",
                table: "Sessions",
                newName: "ExpiryDateTime");

            migrationBuilder.AlterColumn<int>(
                name: "RoomReservationID",
                table: "Sessions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_RoomReservationID",
                table: "Sessions",
                column: "RoomReservationID",
                unique: true);
        }
    }
}
