using RecipeShare.Application.Models;

namespace RecipeShare.Application.Contracts.Persistence.Service
{
    /// <summary>
    /// Recipe service interface for managing recipes and to encapsulate business logic related to recipes.
    /// </summary>
    public interface IRecipeService
    {
        Task<RecipeDto> GetRecipeByIdAsync(int id);
        Task<List<RecipeDto>> GetAllRecipesAsync();
        Task<RecipeDto> AddRecipeAsync(AddRecipesRequest request);
        Task<AddRecipesResponse> AddRecipesAsync(List<AddRecipesRequest> requests);
        Task UpdateRecipeAsync(int id, AddRecipesRequest request);
        Task DeleteRecipeAsync(int id);
    }
}
