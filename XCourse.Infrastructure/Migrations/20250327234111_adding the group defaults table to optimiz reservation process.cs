using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XCourse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addingthegroupdefaultstabletooptimizreservationprocess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "RoomReservations");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "RoomReservations");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "RoomReservations");

            migrationBuilder.DropColumn(
                name: "DefaultDayOneTime",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "DefaultDayTwoTime",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Groups");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "RoomReservations",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "RoomReservations",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReservationStatus",
                table: "RoomReservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                table: "RoomReservations",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeekDay",
                table: "RoomReservations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GroupDefaults",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekDay = table.Column<int>(type: "int", nullable: true),
                    SatartTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    RoomID = table.Column<int>(type: "int", nullable: true),
                    GroupID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupDefaults", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GroupDefaults_Groups_GroupID",
                        column: x => x.GroupID,
                        principalTable: "Groups",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_GroupDefaults_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupDefaults_GroupID",
                table: "GroupDefaults",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupDefaults_RoomID",
                table: "GroupDefaults",
                column: "RoomID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupDefaults");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "RoomReservations");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "RoomReservations");

            migrationBuilder.DropColumn(
                name: "ReservationStatus",
                table: "RoomReservations");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "RoomReservations");

            migrationBuilder.DropColumn(
                name: "WeekDay",
                table: "RoomReservations");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "RoomReservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "RoomReservations",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "RoomReservations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "DefaultDayOneTime",
                table: "Groups",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "DefaultDayTwoTime",
                table: "Groups",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Groups",
                type: "time",
                nullable: true);
        }
    }
}
