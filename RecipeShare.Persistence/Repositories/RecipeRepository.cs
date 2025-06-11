using Microsoft.EntityFrameworkCore;
using RecipeShare.Application.Contracts.Persistence;
using RecipeShare.Application.Models;
using RecipeShare.Domain;
using RecipeShare.Persistence.DatabaseContext;

namespace RecipeShare.Persistence.Repositories
{
    public class RecipeRepository : GenericRepository<Recipe>, IRecipeRepository
    {
        public RecipeRepository(RecipeDatabaseContext context) : base(context)
        {
        }

        public async Task<AddRecipesResponse> AddRecipe(List<Recipe> recipes)
        {
            var result = new AddRecipesResponse();

            if (recipes == null || !recipes.Any())
                throw new ArgumentException("The recipe list cannot be empty", nameof(recipes));

            try
            {
                var uniqueRecipes = new List<Recipe>();

                foreach (var recipe in recipes)
                {
                    // Check if a book with the same title, ingredients, and cooking steps already exists
                    var existingRecipe = await _context.Recipes
                        .FirstOrDefaultAsync(b => b.Title == recipe.Title
                                               && b.Ingredients == recipe.Ingredients
                                               && b.Steps == recipe.Steps);

                    if (existingRecipe == null)
                    {
                        uniqueRecipes.Add(recipe); // Only add the recipe if it doesn't exist
                        result.AddedRecipes.Add(recipe);
                    }
                    else
                    {
                        result.SkippedRecipes.Add(recipe);
                    }
                }

                if (uniqueRecipes.Any() || result.AddedRecipes.Any())
                {
                    await _context.Recipes.AddRangeAsync(uniqueRecipes);
                    await _context.SaveChangesAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()); //Inject the logger via DI and call it here
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }

        public async Task<AddRecipesResponse> AddRecipe(Recipe recipe)
        {
            var result = new AddRecipesResponse();

            // Check if a recipe with the same title, ingredients, and cooking steps already exists
            var existingRecipe = await _context.Recipes
                .FirstOrDefaultAsync(b => b.Title == recipe.Title
                                           && b.Ingredients == recipe.Ingredients
                                           && b.Steps == recipe.Steps);

            if (existingRecipe == null)
            {
                // If no duplicate is found, proceed to add the recipe
                await _context.AddAsync(recipe);
                await _context.SaveChangesAsync();

                result.AddedRecipes.Add(recipe);
            }
            else
            {
                result.SkippedRecipes.Add(recipe);
            }

            return result;
        }

        public async Task DeleteRecipe(int id)
        {
            var entity = await GetRecipe(id);
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }


        public async Task<Recipe?> GetRecipe(int id)
        {
            return await _context.Recipes
                .Include(r => r.RecipeDietaryTags)
                    .ThenInclude(rdt => rdt.DietaryTag)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Recipe>> GetRecipes()
        {
            return await _context.Recipes
                .Include(r => r.RecipeDietaryTags)
                    .ThenInclude(rdt => rdt.DietaryTag)
                .ToListAsync();
        }

        public async Task UpdateRecipe(Recipe recipe)
        {
            _context.Update(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRecipe(int id, Recipe recipe)
        {
            _context.Entry(recipe).State = EntityState.Modified;
            var existingBook = await GetRecipe(id);

            if (existingBook != null)
            {
                existingBook.Title = recipe.Title;
                existingBook.Ingredients = recipe.Ingredients;
                existingBook.Steps = recipe.Steps;
                existingBook.CookingTime = recipe.CookingTime;
                //existingBook.DietaryTags = recipe.DietaryTags;
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateRecipe(int id, Recipe recipe, List<string> dietaryTags)
        {
            var existingRecipe = await GetRecipe(id);
            if (existingRecipe == null) throw new Exception("Recipe not found");

            existingRecipe.Title = recipe.Title;
            existingRecipe.Ingredients = recipe.Ingredients;
            existingRecipe.Steps = recipe.Steps;
            existingRecipe.CookingTime = recipe.CookingTime;

            // Remove existing dietary tags
            _context.RecipeDietaryTags.RemoveRange(existingRecipe.RecipeDietaryTags);
            existingRecipe.RecipeDietaryTags.Clear();

            foreach (var tagName in dietaryTags.Distinct()) // Prevent duplicate tag names
            {
                var tag = await _context.DietaryTags.FirstOrDefaultAsync(dt => dt.Name == tagName);
                if (tag == null)
                {
                    tag = new DietaryTag { Name = tagName };
                    _context.DietaryTags.Add(tag);
                    await _context.SaveChangesAsync(); // Get the ID of the new tag
                }

                existingRecipe.RecipeDietaryTags.Add(new RecipeDietaryTag
                {
                    RecipeId = id,
                    DietaryTagId = tag.Id
                });
            }

            _context.Update(existingRecipe);
            await _context.SaveChangesAsync();
        }

    }
}
