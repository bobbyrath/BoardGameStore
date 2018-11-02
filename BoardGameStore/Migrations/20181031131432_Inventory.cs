using Microsoft.EntityFrameworkCore.Migrations;

namespace BoardGameStore.Migrations
{
    public partial class Inventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InventryID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_InventryID",
                table: "AspNetUsers",
                column: "InventryID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Inventories_InventryID",
                table: "AspNetUsers",
                column: "InventryID",
                principalTable: "Inventories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Inventories_InventryID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_InventryID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InventryID",
                table: "AspNetUsers");
        }
    }
}
