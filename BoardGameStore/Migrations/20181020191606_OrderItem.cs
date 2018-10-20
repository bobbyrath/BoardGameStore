using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BoardGameStore.Migrations
{
    public partial class OrderItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_CartItems_CartItemID",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CheckoutViewModel_CheckOutInfoId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "CheckoutViewModel");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CheckOutInfoId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_CartItemID",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CheckOutInfoId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CartItemID",
                table: "OrderItems");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "OrderItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "OrderItems",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProductPrice",
                table: "OrderItems",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "OrderItems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProductPrice",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "OrderItems");

            migrationBuilder.AddColumn<int>(
                name: "CheckOutInfoId",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CartItemID",
                table: "OrderItems",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CheckoutViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckoutViewModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CheckOutInfoId",
                table: "Orders",
                column: "CheckOutInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_CartItemID",
                table: "OrderItems",
                column: "CartItemID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_CartItems_CartItemID",
                table: "OrderItems",
                column: "CartItemID",
                principalTable: "CartItems",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CheckoutViewModel_CheckOutInfoId",
                table: "Orders",
                column: "CheckOutInfoId",
                principalTable: "CheckoutViewModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
