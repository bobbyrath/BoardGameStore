using Microsoft.EntityFrameworkCore.Migrations;

namespace BoardGameStore.Migrations
{
    public partial class Proposals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proposals_AspNetUsers_BoardGameHubUserId",
                table: "Proposals");

            migrationBuilder.DropIndex(
                name: "IX_Proposals_BoardGameHubUserId",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "BoardGameHubUserId",
                table: "Proposals");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BoardGameHubUserId",
                table: "Proposals",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_BoardGameHubUserId",
                table: "Proposals",
                column: "BoardGameHubUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Proposals_AspNetUsers_BoardGameHubUserId",
                table: "Proposals",
                column: "BoardGameHubUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
