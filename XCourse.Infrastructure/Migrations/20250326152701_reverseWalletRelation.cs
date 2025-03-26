using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XCourse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class reverseWalletRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Wallets_WalletID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_WalletID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WalletID",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "AppUserID",
                table: "Wallets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_AppUserID",
                table: "Wallets",
                column: "AppUserID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_AspNetUsers_AppUserID",
                table: "Wallets",
                column: "AppUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_AspNetUsers_AppUserID",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_AppUserID",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "AppUserID",
                table: "Wallets");

            migrationBuilder.AddColumn<int>(
                name: "WalletID",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_WalletID",
                table: "AspNetUsers",
                column: "WalletID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Wallets_WalletID",
                table: "AspNetUsers",
                column: "WalletID",
                principalTable: "Wallets",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
