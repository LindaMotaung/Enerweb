using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeShare.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByDateFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 6, 10, 16, 32, 16, 114, DateTimeKind.Local).AddTicks(6016), new DateTime(2025, 6, 10, 16, 32, 16, 116, DateTimeKind.Local).AddTicks(6764) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 6, 10, 15, 29, 20, 362, DateTimeKind.Local).AddTicks(1020), new DateTime(2025, 6, 10, 15, 29, 20, 364, DateTimeKind.Local).AddTicks(5122) });
        }
    }
}
