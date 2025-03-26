using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace XCourse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringNullableAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Sessions_SessionId",
                table: "Attendances");

            migrationBuilder.RenameColumn(
                name: "IsAvilableForPrivateGroup",
                table: "Teachers",
                newName: "IsAvailableForPrivateGroups");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "Attendances",
                newName: "SessionID");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_SessionId",
                table: "Attendances",
                newName: "IX_Attendances_SessionID");

            migrationBuilder.AlterColumn<decimal>(
                name: "PrivatePrice",
                table: "Teachers",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "Students",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Major",
                table: "Students",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "URL",
                table: "Sessions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Point>(
                name: "Location",
                table: "PrivateGroupRequests",
                type: "geography",
                nullable: true,
                oldClrType: typeof(Point),
                oldType: "geography");

            migrationBuilder.AlterColumn<Point>(
                name: "Location",
                table: "Centers",
                type: "geography",
                nullable: true,
                oldClrType: typeof(Point),
                oldType: "geography");

            migrationBuilder.AlterColumn<int>(
                name: "TeacherID",
                table: "AssistantInvitations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Point>(
                name: "HomeLocation",
                table: "AspNetUsers",
                type: "geography",
                nullable: true,
                oldClrType: typeof(Point),
                oldType: "geography");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<int>(
                name: "AccountType",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "Announcements",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "Announcements",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Sessions_SessionID",
                table: "Attendances",
                column: "SessionID",
                principalTable: "Sessions",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Sessions_SessionID",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "IsAvailableForPrivateGroups",
                table: "Teachers",
                newName: "IsAvilableForPrivateGroup");

            migrationBuilder.RenameColumn(
                name: "SessionID",
                table: "Attendances",
                newName: "SessionId");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_SessionID",
                table: "Attendances",
                newName: "IX_Attendances_SessionId");

            migrationBuilder.AlterColumn<decimal>(
                name: "PrivatePrice",
                table: "Teachers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Major",
                table: "Students",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "URL",
                table: "Sessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Point>(
                name: "Location",
                table: "PrivateGroupRequests",
                type: "geography",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geography",
                oldNullable: true);

            migrationBuilder.AlterColumn<Point>(
                name: "Location",
                table: "Centers",
                type: "geography",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geography",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TeacherID",
                table: "AssistantInvitations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Point>(
                name: "HomeLocation",
                table: "AspNetUsers",
                type: "geography",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geography",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "Announcements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "Announcements",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Sessions_SessionId",
                table: "Attendances",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "ID");
        }
    }
}
