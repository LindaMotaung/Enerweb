using RecipeShare.Domain;

namespace RecipeShare.Application.Contracts.Persistence
{
    public interface IDietaryTagRepository
    {
        Task<List<DietaryTag>> GetAllDietaryTagsAsync();
        Task<List<DietaryTag>> GetDietaryTagsByNamesAsync(List<string> tagNames);
        Task<List<DietaryTag>> EnsureDietaryTagsExistAsync(List<string> tagNames);
        Task<DietaryTag> GetOrCreateAsync(string tagName);
    }
}
