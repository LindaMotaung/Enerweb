using RecipeShare.Application.Models;
using RecipeShare.Domain;
namespace RecipeShare.Application.Contracts.Persistence
{
    public interface IRecipeRepository 
    {
        Task<AddRecipesResponse> AddRecipe(List<Recipe> recipes); //Adding a list of recipe in one go
        Task<AddRecipesResponse> AddRecipe(Recipe recipe); //Adding a single recipe at a time
        Task UpdateRecipe(Recipe recipe);
        Task UpdateRecipe(int id, Recipe recipe); //Updating a recipe. Can have Put or Patch for a partial update
        Task DeleteRecipe(int id); //deletes a specific recipe
        Task<Recipe?> GetRecipe(int id); //Gets a specific recipe by its ID
        Task<List<Recipe>> GetRecipes(); //Gets a list of recipe
        Task UpdateRecipe(int id, Recipe recipe, List<string> dietaryTags);
    }
}
