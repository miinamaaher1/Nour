using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XCourse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakePictiresNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PreviewPicture",
                table: "Rooms",
                type: "varbinary(MAX)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(MAX)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "CoverPicture",
                table: "Groups",
                type: "varbinary(MAX)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(MAX)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PreviewPicture",
                table: "Centers",
                type: "varbinary(MAX)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(MAX)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "varbinary(MAX)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(MAX)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PreviewPicture",
                table: "Rooms",
                type: "varbinary(MAX)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(MAX)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "CoverPicture",
                table: "Groups",
                type: "varbinary(MAX)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(MAX)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "PreviewPicture",
                table: "Centers",
                type: "varbinary(MAX)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(MAX)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "varbinary(MAX)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(MAX)",
                oldNullable: true);
        }
    }
}
