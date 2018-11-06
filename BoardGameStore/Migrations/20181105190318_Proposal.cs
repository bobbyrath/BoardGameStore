using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BoardGameStore.Migrations
{
    public partial class Proposal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProposalID",
                table: "InventoryItems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProposalID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Proposal",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProposerID = table.Column<string>(nullable: true),
                    ProposeeID = table.Column<string>(nullable: true),
                    ProposerItemID = table.Column<int>(nullable: false),
                    ProposeeItemID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proposal", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_ProposalID",
                table: "InventoryItems",
                column: "ProposalID");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Proposal_ProposalID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Proposal_ProposalID",
                table: "InventoryItems");

            migrationBuilder.DropTable(
                name: "Proposal");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItems_ProposalID",
                table: "InventoryItems");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProposalID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProposalID",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "ProposalID",
                table: "AspNetUsers");
        }
    }
}
