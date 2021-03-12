using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyJuices.Persistence.Ef.Migrations
{
    public partial class money_value_object : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DefaultPricePerUnit",
                table: "Products",
                newName: "DefaultPricePerUnitAmount");

            migrationBuilder.AddColumn<int>(
                name: "DefaultPricePerUnitCurrency",
                table: "Products",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultPricePerUnitCurrency",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "DefaultPricePerUnitAmount",
                table: "Products",
                newName: "DefaultPricePerUnit");
        }
    }
}
