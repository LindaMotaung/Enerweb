using RecipeShare.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecipeShare.Domain
{
    public class Recipe : BaseEntity
    {
        [JsonPropertyName("ID")]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Ingredients { get; set; }

        [Required]
        public string Steps { get; set; }

        [Range(1, int.MaxValue)]
        public int CookingTime { get; set; }

        // Navigation property to the bridging table
        [JsonIgnore]
        public ICollection<RecipeDietaryTag> RecipeDietaryTags { get; set; } = new List<RecipeDietaryTag>();
    }

}
