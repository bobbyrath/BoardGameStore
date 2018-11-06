using Microsoft.EntityFrameworkCore.Migrations;

namespace BoardGameStore.Migrations
{
    public partial class ProposalUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Proposal_ProposalID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Proposal_ProposalID",
                table: "InventoryItems");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProposalID",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Proposal",
                table: "Proposal");

            migrationBuilder.DropColumn(
                name: "ProposalID",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Proposal",
                newName: "Proposals");

            migrationBuilder.AddColumn<string>(
                name: "BoardGameHubUserId",
                table: "Proposals",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Proposals",
                table: "Proposals",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_BoardGameHubUserId",
                table: "Proposals",
                column: "BoardGameHubUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Proposals_ProposalID",
                table: "InventoryItems",
                column: "ProposalID",
                principalTable: "Proposals",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Proposals_AspNetUsers_BoardGameHubUserId",
                table: "Proposals",
                column: "BoardGameHubUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Proposals_ProposalID",
                table: "InventoryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Proposals_AspNetUsers_BoardGameHubUserId",
                table: "Proposals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Proposals",
                table: "Proposals");

            migrationBuilder.DropIndex(
                name: "IX_Proposals_BoardGameHubUserId",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "BoardGameHubUserId",
                table: "Proposals");

            migrationBuilder.RenameTable(
                name: "Proposals",
                newName: "Proposal");

            migrationBuilder.AddColumn<int>(
                name: "ProposalID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Proposal",
                table: "Proposal",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProposalID",
                table: "AspNetUsers",
                column: "ProposalID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Proposal_ProposalID",
                table: "AspNetUsers",
                column: "ProposalID",
                principalTable: "Proposal",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Proposal_ProposalID",
                table: "InventoryItems",
                column: "ProposalID",
                principalTable: "Proposal",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
