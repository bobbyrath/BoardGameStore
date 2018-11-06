using Microsoft.EntityFrameworkCore.Migrations;

namespace BoardGameStore.Migrations
{
    public partial class ProposalName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProposeeItemID",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "ProposerItemID",
                table: "Proposals");

            migrationBuilder.RenameColumn(
                name: "ProposerID",
                table: "Proposals",
                newName: "ProposerItem");

            migrationBuilder.RenameColumn(
                name: "ProposeeID",
                table: "Proposals",
                newName: "Proposer");

            migrationBuilder.AddColumn<string>(
                name: "Proposee",
                table: "Proposals",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProposeeItem",
                table: "Proposals",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Proposee",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "ProposeeItem",
                table: "Proposals");

            migrationBuilder.RenameColumn(
                name: "ProposerItem",
                table: "Proposals",
                newName: "ProposerID");

            migrationBuilder.RenameColumn(
                name: "Proposer",
                table: "Proposals",
                newName: "ProposeeID");

            migrationBuilder.AddColumn<int>(
                name: "ProposeeItemID",
                table: "Proposals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProposerItemID",
                table: "Proposals",
                nullable: false,
                defaultValue: 0);
        }
    }
}
