using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RecipeShare.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DietaryTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietaryTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Ingredients = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Steps = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CookingTime = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipeDietaryTags",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    DietaryTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeDietaryTags", x => new { x.RecipeId, x.DietaryTagId });
                    table.ForeignKey(
                        name: "FK_RecipeDietaryTags_DietaryTags_DietaryTagId",
                        column: x => x.DietaryTagId,
                        principalTable: "DietaryTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeDietaryTags_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DietaryTags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 2, "Keto" },
                    { 3, "Vegan" }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "CookingTime", "CreatedBy", "DateCreated", "DateModified", "Ingredients", "ModifiedBy", "Steps", "Title" },
                values: new object[] { 1, 30, "Gordon Ramsey", new DateTime(2025, 6, 10, 15, 29, 20, 362, DateTimeKind.Local).AddTicks(1020), new DateTime(2025, 6, 10, 15, 29, 20, 364, DateTimeKind.Local).AddTicks(5122), "Tomatoes, Basil, Olive Oil, Garlic, Salt, Pepper", "Jamie Oliver", "1. Preheat oven to 200C. 2. Chop tomatoes and garlic. 3. Mix with olive oil, salt, and pepper. 4. Roast for 20 minutes. 5. Serve with fresh basil.", "The Delicious Recipes of Jamie Oliver" });

            migrationBuilder.InsertData(
                table: "RecipeDietaryTags",
                columns: new[] { "DietaryTagId", "RecipeId" },
                values: new object[,]
                {
                    { 2, 1 },
                    { 3, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeDietaryTags_DietaryTagId",
                table: "RecipeDietaryTags",
                column: "DietaryTagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeDietaryTags");

            migrationBuilder.DropTable(
                name: "DietaryTags");

            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
