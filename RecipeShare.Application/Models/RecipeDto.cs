namespace RecipeShare.Application.Models
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Ingredients { get; set; }
        public string Steps { get; set; }
        public int CookingTime { get; set; }
        public List<string> DietaryTags { get; set; }
    }
}
