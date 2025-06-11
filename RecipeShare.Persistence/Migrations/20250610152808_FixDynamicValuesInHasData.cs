using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeShare.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixDynamicValuesInHasData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2023, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 6, 10, 16, 32, 16, 114, DateTimeKind.Local).AddTicks(6016), new DateTime(2025, 6, 10, 16, 32, 16, 116, DateTimeKind.Local).AddTicks(6764) });
        }
    }
}
