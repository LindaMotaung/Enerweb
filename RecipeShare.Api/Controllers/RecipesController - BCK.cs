using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecipeShare.Application.Contracts.Persistence;
using RecipeShare.Application.Models;
using RecipeShare.Domain;

namespace RecipeShare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IDietaryTagRepository _dietaryTagRepository;
        private readonly IMapper _mapper;

        public RecipesController(IRecipeRepository recipeRepository,
             IDietaryTagRepository dietaryTagRepository,
            IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns a specific recipe
        /// </summary>
        /// <param name="id">The ID of the recipe</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:long}")]
        public async Task<ActionResult<RecipeDto>> GetRecipe(int id)
        {
            var recipe = await _recipeRepository.GetRecipe(id);
            if (recipe == null)
                return NotFound();

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

            return Ok(dto);
        }


        /// <summary>
        /// Returns a list of recipes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-recipes")]
        public async Task<ActionResult<List<Recipe>>> GetRecipes()
        {
            var recipes = await _recipeRepository.GetRecipes();
            if (recipes == null || !recipes.Any())
            {
                return Ok("There are no recipes to display");
            }
            return Ok(recipes);
        }

        [HttpGet("get-recipes-dto")]
        public async Task<ActionResult<List<RecipeDto>>> GetRecipesDto()
        {
            var recipes = await _recipeRepository.GetRecipes();
            var recipeDtos = _mapper.Map<List<RecipeDto>>(recipes);

            if (recipeDtos == null || !recipeDtos.Any())
            {
                return Ok("There are no recipes to display");
            }

            return Ok(recipeDtos);
        }

        /// <summary>
        /// Adds a single recipe
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("recipe")]
        public async Task<IActionResult> AddRecipe([FromBody] AddRecipesRequest request)
        {
            if (request == null) return BadRequest();

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
            return CreatedAtAction(nameof(GetRecipe), new { id = recipe.Id }, recipe);
        }


        /// <summary>
        /// Adds a list of recipes 
        /// </summary>
        /// <param name="recipes"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-recipes")]
        public async Task<IActionResult> AddRecipes([FromBody] List<Recipe> recipes)
        {
            if (recipes == null || !recipes.Any())
            {
                return BadRequest("The recipe list cannot be empty.");
            }

            try
            {
                var result = await _recipeRepository.AddRecipe(recipes); // Call the AddRecipe method in the repository
                if (!result.AddedRecipes.Any())
                {
                    return Ok("No new recipes were added. All recipes already exist.");
                }

                return Ok($"{result.AddedRecipes.Count} recipe(s) added successfully. {result.SkippedRecipes.Count} recipe(s) were skipped because they already exist.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates a specific recipe
        /// </summary>
        /// <param name="id">ID of the recipe to be updated</param>
        /// <param name="recipe">The recipe object to be updated</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:long}")]
        public async Task<IActionResult> UpdateRecipe(int id, Recipe recipe)
        {
            await _recipeRepository.UpdateRecipe(id, recipe);
            return NoContent();
        }

        [HttpPut]
        [Route("{id:long}/new")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] AddRecipesRequest request)
        {
            if (request == null) return BadRequest("Request is null");

            var recipe = _mapper.Map<Recipe>(request);
            await _recipeRepository.UpdateRecipe(id, recipe, request.DietaryTags);

            return NoContent();
        }

        /// <summary>
        /// Deletes a specific recipe
        /// </summary>
        /// <param name="id">The ID of the recipe</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            await _recipeRepository.DeleteRecipe(id);
            return NoContent();
        }
    }
}
