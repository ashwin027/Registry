using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductCatalog.Repository.Migrations
{
    public partial class initialdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Baby Orange - Training Teether Tooth Brush for Infant, Baby, and Toddler", "Teether" },
                    { 2, "Pacifier Value Pack, Boy, 6-18 Months (Pack of 3)", "Foo Pacifier value pack" },
                    { 3, "Fisher-Price Fidget Cube", "Baby fidget cube" },
                    { 4, "Costco steps walker (12 - 18 months)", "Steps Walker" },
                    { 5, "Activity floor seat and booster all in one", "Summer Booster chair" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
