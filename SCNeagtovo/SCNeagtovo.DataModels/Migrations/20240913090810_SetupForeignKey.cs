using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCNeagtovo.DataModels.Migrations
{
    /// <inheritdoc />
    public partial class SetupForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryDetail_Orders_OrderId",
                table: "DeliveryDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients_ClientId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ClientId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryDetail_OrderId",
                table: "DeliveryDetail");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryId",
                table: "Orders",
                column: "DeliveryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryDetail_DeliveryId",
                table: "Orders",
                column: "DeliveryId",
                principalTable: "DeliveryDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryDetail_DeliveryId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DeliveryId",
                table: "Orders");

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientId",
                table: "Orders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDetail_OrderId",
                table: "DeliveryDetail",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryDetail_Orders_OrderId",
                table: "DeliveryDetail",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clients_ClientId",
                table: "Orders",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");
        }
    }
}
