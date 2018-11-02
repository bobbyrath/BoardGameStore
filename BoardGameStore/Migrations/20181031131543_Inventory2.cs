using Microsoft.EntityFrameworkCore.Migrations;

namespace BoardGameStore.Migrations
{
    public partial class Inventory2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Inventories_InventryID",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "InventryID",
                table: "AspNetUsers",
                newName: "InventoryID");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_InventryID",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_InventoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Inventories_InventoryID",
                table: "AspNetUsers",
                column: "InventoryID",
                principalTable: "Inventories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Inventories_InventoryID",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "InventoryID",
                table: "AspNetUsers",
                newName: "InventryID");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_InventoryID",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_InventryID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Inventories_InventryID",
                table: "AspNetUsers",
                column: "InventryID",
                principalTable: "Inventories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
