using Microsoft.EntityFrameworkCore;
using Moq;
using RecipeShare.Domain;
using RecipeShare.Persistence.DatabaseContext;
using RecipeShare.Persistence.Repositories;
using RecipeShare.UnitTests.Helpers;
using Xunit;

namespace RecipeShare.UnitTests.Features
{
    public class CreateRecipeRepositoryTests
    {
        private readonly Mock<RecipeDatabaseContext> _mockContext;
        private readonly List<Recipe> _recipeList;
        private readonly RecipeRepository _recipeRepository;

        public CreateRecipeRepositoryTests() 
        {
            _recipeList = new List<Recipe>
            {
                new Recipe { Id = 1, Title = "Spaghetti", Ingredients = "Pasta", Steps = "Boil", CookingTime = 10 },
                new Recipe { Id = 2, Title = "Omelette", Ingredients = "Eggs", Steps = "Fry", CookingTime = 5 }
            };

            var mockRecipeSet = DbSetMockHelper.CreateMockDbSet(_recipeList);

            _mockContext = new Mock<RecipeDatabaseContext>();
            _mockContext.Setup(c => c.Recipes).Returns(mockRecipeSet.Object);

            _recipeRepository = new RecipeRepository(_mockContext.Object);
        }

        [Fact]
        public async Task GetRecipe_ExistingId_ShouldReturnRecipe() 
        {
            // Arrange
            var options = new DbContextOptionsBuilder<RecipeDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "RecipeDb_GetRecipeTest")
                .Options;

            using var context = new RecipeDatabaseContext(options);

            var recipe = new Recipe
            {
                Id = 1,
                Title = "Spaghetti",
                Ingredients = "Pasta, Tomato Sauce" ,
                Steps = "Boil water, Cook pasta, Add sauce" 
            };

            context.Recipes.Add(recipe);
            context.SaveChanges();

            var repository = new RecipeRepository(context);

            // Act
            var result = await repository.GetRecipe(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Spaghetti", result.Title);
        }

        [Fact]
        public async Task GetRecipe_ShouldReturnListOfRecipes() 
        {
            // Arrange
            var options = new DbContextOptionsBuilder<RecipeDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "GetAllRecipesDb")
                .Options;

            using var context = new RecipeDatabaseContext(options);
            var repository = new RecipeRepository(context);

            // Seed data
            var seededRecipes = new List<Recipe>
            {
                new Recipe { Title = "Lasagna", Ingredients = "Cheese", Steps = "Layer", CookingTime = 45 },
                new Recipe { Title = "Stir Fry", Ingredients = "Veggies", Steps = "Fry", CookingTime = 10 }
            };
            context.Recipes.AddRange(seededRecipes);
            await context.SaveChangesAsync();

            // Act
            var recipes = await repository.GetRecipes();

            // Assert
            Assert.NotEmpty(recipes);
            Assert.Equal(2, recipes.Count);
            Assert.Contains(recipes, r => r.Title == "Lasagna");
            Assert.Contains(recipes, r => r.Title == "Stir Fry");
        }

        [Fact]
        public async Task AddRecipe_ListOfRecipes_ShouldAddToDatabase() 
        {
            // Arrange
            var options = new DbContextOptionsBuilder<RecipeDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "AddListOfRecipesDb")
                .Options;

            using var context = new RecipeDatabaseContext(options);
            var repository = new RecipeRepository(context);

            // Optional: seed initial data if you want to simulate "Original 2"
            context.Recipes.AddRange(new List<Recipe>
            {
                new Recipe { Title = "Pasta", Ingredients = "Noodles", Steps = "Boil", CookingTime = 10 },
                new Recipe { Title = "Soup", Ingredients = "Water", Steps = "Heat", CookingTime = 5 }
            });
            await context.SaveChangesAsync();

            var newRecipes = new List<Recipe>
    {
        new Recipe { Title = "Curry", Ingredients = "Chicken", Steps = "Cook", CookingTime = 15 },
        new Recipe { Title = "Salad", Ingredients = "Lettuce", Steps = "Mix", CookingTime = 2 }
    };

            // Act
            await repository.AddRecipe(newRecipes);

            // Assert
            var allRecipes = await context.Recipes.ToListAsync();
            Assert.Equal(4, allRecipes.Count); // 2 seeded + 2 new
            Assert.Contains(allRecipes, r => r.Title == "Curry");
            Assert.Contains(allRecipes, r => r.Title == "Salad");
        }

        [Fact]
        public async Task AddRecipe_SingleRecipe_ShouldAddToDatabase() 
        {
            // Arrange
            var options = new DbContextOptionsBuilder<RecipeDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "AddSingleRecipeDb")
                .Options;

            using var context = new RecipeDatabaseContext(options);

            var repository = new RecipeRepository(context);

            var newRecipe = new Recipe
            {
                Title = "Stew",
                Ingredients = "Pasta, Tomato Sauce",
                Steps = "Boil water, Cook pasta, Add sauce",
                CookingTime = 20
            };

            // Act
            await repository.AddRecipe(newRecipe);

            // Assert
            var recipes = await context.Recipes.ToListAsync();
            Assert.Single(recipes);
            Assert.Contains(recipes, r => r.Title == "Stew");
        }

        [Fact]
        public async Task DeleteRecipe_ExistingRecipe_ShouldRemoveFromDatabase() 
        {
            // Arrange
            var options = new DbContextOptionsBuilder<RecipeDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "DeleteRecipeDb")
                .Options;

            using var context = new RecipeDatabaseContext(options);

            var recipe = new Recipe
            {
                Id = 1,
                Title = "Lasagna",
                Ingredients = "Pasta, Tomato Sauce",
                Steps = "Boil water, Cook pasta, Add sauce"
            };

            context.Recipes.Add(recipe);
            await context.SaveChangesAsync();

            var repository = new RecipeRepository(context);

            // Act
            await repository.DeleteRecipe(1);

            // Assert
            var deleted = await context.Recipes.FindAsync(1);
            Assert.Null(deleted);
        }
    }
}
