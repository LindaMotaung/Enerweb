using AutoMapper;
using RecipeShare.Application.Contracts.Persistence;
using RecipeShare.Application.Contracts.Persistence.Service;
using RecipeShare.Application.Models;
using RecipeShare.Domain;

namespace RecipeShare.Persistence.Repositories.Service
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IDietaryTagRepository _dietaryTagRepository;
        private readonly IMapper _mapper;

        public RecipeService(IRecipeRepository recipeRepository,
                             IDietaryTagRepository dietaryTagRepository,
                             IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _dietaryTagRepository = dietaryTagRepository;
            _mapper = mapper;
        }

        public async Task<RecipeDto> GetRecipeByIdAsync(int id)
        {
            var recipe = await _recipeRepository.GetRecipe(id);
            if (recipe == null)
                return null;

            var dto = new RecipeDto
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Ingredients = recipe.Ingredients,
                Steps = recipe.Steps,
                CookingTime = recipe.CookingTime,
                DietaryTags = recipe.RecipeDietaryTags
                    .Select(rt => rt.DietaryTag.Name)
                    .ToList()
            };

            return dto;
        }

        public async Task<List<RecipeDto>> GetAllRecipesAsync()
        {
            var recipes = await _recipeRepository.GetRecipes();
            var dtos = _mapper.Map<List<RecipeDto>>(recipes);
            return dtos;
        }

        public async Task<RecipeDto> AddRecipeAsync(AddRecipesRequest request)
        {
            var recipe = new Recipe
            {
                Title = request.Title,
                Ingredients = request.Ingredients,
                Steps = request.Steps,
                CookingTime = request.CookingTime
            };

            recipe.RecipeDietaryTags = new List<RecipeDietaryTag>();
            foreach (var tagName in request.DietaryTags)
            {
                var tag = await _dietaryTagRepository.GetOrCreateAsync(tagName);
                recipe.RecipeDietaryTags.Add(new RecipeDietaryTag
                {
                    Recipe = recipe,
                    DietaryTag = tag
                });
            }

            await _recipeRepository.AddRecipe(recipe);
            return _mapper.Map<RecipeDto>(recipe);
        }

        public async Task<AddRecipesResponse> AddRecipesAsync(List<AddRecipesRequest> requests)
        {
            var recipesToAdd = new List<Recipe>();
            var added = new List<Recipe>();
            var skipped = new List<Recipe>();

            foreach (var request in requests)
            {
                var recipe = new Recipe
                {
                    Title = request.Title,
                    Ingredients = request.Ingredients,
                    Steps = request.Steps,
                    CookingTime = request.CookingTime
                };

                recipe.RecipeDietaryTags = new List<RecipeDietaryTag>();
                foreach (var tagName in request.DietaryTags)
                {
                    var tag = await _dietaryTagRepository.GetOrCreateAsync(tagName);
                    recipe.RecipeDietaryTags.Add(new RecipeDietaryTag
                    {
                        Recipe = recipe,
                        DietaryTag = tag
                    });
                }

                recipesToAdd.Add(recipe);
            }

            var result = await _recipeRepository.AddRecipe(recipesToAdd);
            return result;
        }

        public async Task UpdateRecipeAsync(int id, AddRecipesRequest request)
        {
            var recipe = _mapper.Map<Recipe>(request);
            await _recipeRepository.UpdateRecipe(id, recipe, request.DietaryTags);
        }

        public async Task DeleteRecipeAsync(int id)
        {
            await _recipeRepository.DeleteRecipe(id);
        }
    }
}
