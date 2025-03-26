using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XCourse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Editingsessionandgroupentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DefaultStartTime",
                table: "Groups",
                newName: "Duration");

            migrationBuilder.RenameColumn(
                name: "DefaultEndTime",
                table: "Groups",
                newName: "DefaultDayTwoTime");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Duration",
                table: "Sessions",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "DefaultDayOneTime",
                table: "Groups",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultDayOneTime",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Groups",
                newName: "DefaultStartTime");

            migrationBuilder.RenameColumn(
                name: "DefaultDayTwoTime",
                table: "Groups",
                newName: "DefaultEndTime");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Duration",
                table: "Sessions",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);
        }
    }
}
