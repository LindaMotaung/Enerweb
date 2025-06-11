namespace RecipeShare.Domain
{
    /// <summary>
    /// RecipeId and DietaryTagId form the composite primary key to map out the many-many relationship in the RecipeDietaryTag bridging table .
    /// </summary>
    public class RecipeDietaryTag
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int DietaryTagId { get; set; }
        public DietaryTag DietaryTag { get; set; }
    }
}
