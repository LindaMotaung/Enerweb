using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeShare.Domain;

namespace RecipeShare.Persistence.Configurations
{
    public class RecipeGenreConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.HasData(
            new Recipe
            {
                Id = 1,
                Title = "The Delicious Recipes of Jamie Oliver",
                Ingredients = "Tomatoes, Basil, Olive Oil, Garlic, Salt, Pepper",
                Steps = "1. Preheat oven to 200C. 2. Chop tomatoes and garlic. 3. Mix with olive oil, salt, and pepper. 4. Roast for 20 minutes. 5. Serve with fresh basil.",
                CookingTime = 30,
                DateCreated = new DateTime(2023, 6, 10), // Example static date
                DateModified = new DateTime(2023, 6, 10), // Example static date
                CreatedBy = "Gordon Ramsey",
                ModifiedBy = "Jamie Oliver"
            }
        );

            builder.Property(q => q.Title)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
