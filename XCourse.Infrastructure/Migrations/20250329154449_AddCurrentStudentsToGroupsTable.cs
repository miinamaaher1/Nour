using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XCourse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrentStudentsToGroupsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Subjects_SubjectID",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "SatartTime",
                table: "GroupDefaults",
                newName: "StartTime");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectID",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentStudents",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Subjects_SubjectID",
                table: "Groups",
                column: "SubjectID",
                principalTable: "Subjects",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Subjects_SubjectID",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "CurrentStudents",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "GroupDefaults",
                newName: "SatartTime");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectID",
                table: "Groups",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Subjects_SubjectID",
                table: "Groups",
                column: "SubjectID",
                principalTable: "Subjects",
                principalColumn: "ID");
        }
    }
}
