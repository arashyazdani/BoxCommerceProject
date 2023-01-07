using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeVehclesPartsIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VehiclesParts",
                table: "VehiclesParts");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "VehiclesParts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehiclesParts",
                table: "VehiclesParts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_VehiclesParts_ProductId_VehicleId",
                table: "VehiclesParts",
                columns: new[] { "ProductId", "VehicleId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VehiclesParts",
                table: "VehiclesParts");

            migrationBuilder.DropIndex(
                name: "IX_VehiclesParts_ProductId_VehicleId",
                table: "VehiclesParts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "VehiclesParts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehiclesParts",
                table: "VehiclesParts",
                columns: new[] { "ProductId", "VehicleId" });
        }
    }
}
