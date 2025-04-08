using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XCourse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConvertingtherelationbetweentheannouncementandthegrouptoManytoMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Groups_GroupID",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_GroupID",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "GroupID",
                table: "Announcements");

            migrationBuilder.CreateTable(
                name: "AnnouncementGroup",
                columns: table => new
                {
                    AnnouncementsID = table.Column<int>(type: "int", nullable: false),
                    GroupsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementGroup", x => new { x.AnnouncementsID, x.GroupsID });
                    table.ForeignKey(
                        name: "FK_AnnouncementGroup_Announcements_AnnouncementsID",
                        column: x => x.AnnouncementsID,
                        principalTable: "Announcements",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnnouncementGroup_Groups_GroupsID",
                        column: x => x.GroupsID,
                        principalTable: "Groups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementGroup_GroupsID",
                table: "AnnouncementGroup",
                column: "GroupsID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnouncementGroup");

            migrationBuilder.AddColumn<int>(
                name: "GroupID",
                table: "Announcements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_GroupID",
                table: "Announcements",
                column: "GroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Groups_GroupID",
                table: "Announcements",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
