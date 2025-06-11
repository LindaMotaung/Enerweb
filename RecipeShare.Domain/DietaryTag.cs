using System.ComponentModel.DataAnnotations;

namespace RecipeShare.Domain
{
    public class DietaryTag
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Navigation property for many-to-many relationship
        public ICollection<RecipeDietaryTag> RecipeDietaryTags { get; set; } = new List<RecipeDietaryTag>();
    }
}
