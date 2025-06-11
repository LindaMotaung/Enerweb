using Microsoft.EntityFrameworkCore;
using RecipeShare.Application.Contracts.Persistence;
using RecipeShare.Domain;
using RecipeShare.Persistence.DatabaseContext;

namespace RecipeShare.Persistence.Repositories
{
    public class DietaryTagRepository : IDietaryTagRepository
    {
        private readonly RecipeDatabaseContext _context;

        public DietaryTagRepository(RecipeDatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<DietaryTag>> GetAllDietaryTagsAsync()
        {
            return await _context.DietaryTags.ToListAsync();
        }

        public async Task<List<DietaryTag>> GetDietaryTagsByNamesAsync(List<string> tagNames)
        {
            return await _context.DietaryTags
                .Where(dt => tagNames.Contains(dt.Name))
                .ToListAsync();
        }

        public async Task<List<DietaryTag>> EnsureDietaryTagsExistAsync(List<string> tagNames)
        {
            var existingTags = await GetDietaryTagsByNamesAsync(tagNames);
            var existingNames = existingTags.Select(t => t.Name).ToHashSet();

            var missingNames = tagNames
                .Where(name => !existingNames.Contains(name))
                .Distinct()
                .ToList();

            var newTags = missingNames
                .Select(name => new DietaryTag { Name = name })
                .ToList();

            if (newTags.Any())
            {
                _context.DietaryTags.AddRange(newTags);
                await _context.SaveChangesAsync();
                existingTags.AddRange(newTags);
            }

            return existingTags;
        }

        public async Task<DietaryTag> GetOrCreateAsync(string tagName)
        {
            var tag = await _context.DietaryTags
                .FirstOrDefaultAsync(t => t.Name.ToLower() == tagName.ToLower());

            if (tag == null)
            {
                tag = new DietaryTag { Name = tagName };
                _context.DietaryTags.Add(tag);
                await _context.SaveChangesAsync();
            }

            return tag;
        }
    }
}
