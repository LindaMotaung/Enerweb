using RecipeShare.Domain;

namespace RecipeShare.Application.Models
{
    public class AddRecipesResponse
    {
        public List<Recipe> AddedRecipes { get; set; } = new();
        public List<Recipe> SkippedRecipes { get; set; } = new();
    }
}
