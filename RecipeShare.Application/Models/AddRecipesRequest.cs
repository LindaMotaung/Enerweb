namespace RecipeShare.Application.Models
{
    public class AddRecipesRequest
    {
        public string Title { get; set; }
        public string Ingredients { get; set; }
        public string Steps { get; set; }
        public int CookingTime { get; set; }
        public List<string> DietaryTags { get; set; }
    }
}
