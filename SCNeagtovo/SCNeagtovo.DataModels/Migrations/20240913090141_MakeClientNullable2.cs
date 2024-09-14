using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCNeagtovo.DataModels.Migrations
{
    /// <inheritdoc />
    public partial class MakeClientNullable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "DeliveryDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "DeliveryDetail");
        }
    }
}
