using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Sales.Migrations
{
    /// <inheritdoc />
    public partial class AddProductdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "3b7b1ab4-e5ef-4b25-9088-4d5a746729cf");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "5b5a2b37-a884-47d7-8216-126b513a2b04");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "e4170b4e-a005-4141-9d8f-cdd784461e79");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Length", "Name", "Price", "Thickness", "Width" },
                values: new object[,]
                {
                    { 1, 1, 5.5, "Стальная труба", 2000.00m, 0.20000000298023224, 2.7999999523162842 },
                    { 2, 1, 4.4000000953674316, "Медная труба", 1200.00m, 0.30000001192092896, 2.5 },
                    { 3, 1, 5.4000000953674316, "Алюминиевая труба", 1000.00m, 0.5, 2.2999999523162842 },
                    { 4, 2, 2.0999999046325684, "Медная сетка", 500.00m, 0.029999999329447746, 3.0 },
                    { 5, 2, 1.8999999761581421, "Алюминиевая сетка", 300.00m, 0.05000000074505806, 2.5 },
                    { 6, 2, 1.7000000476837158, "Стальная сетка", 80.00m, 0.019999999552965164, 5.0 },
                    { 7, 3, 1.1000000238418579, "Алюминиевая проволка", 50.00m, 0.10000000149011612, 2.0 },
                    { 8, 3, 1.3999999761581421, "Медная проволка", 70.00m, 0.20000000298023224, 3.0 },
                    { 9, 3, 1.5, "Стальная проволка", 190.00m, 0.40000000596046448, 4.0 },
                    { 10, 4, 80.0, "Алюминиевый лист", 5500.00m, 2.0, 150.0 },
                    { 11, 4, 90.0, "Медный лист", 5000.00m, 3.0, 150.0 },
                    { 12, 4, 70.0, "Стальной лист", 6000.00m, 4.0, 250.0 }
                });
        }

        /// <inheritdoc />
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

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "7785db86-0b20-4b3f-8d00-85ab11d21ac1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "0f618f79-cc25-457f-abba-2b4df51f2554");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "8954ebfa-2260-43a6-b55d-ebe2ca5bc589");
        }
    }
}
