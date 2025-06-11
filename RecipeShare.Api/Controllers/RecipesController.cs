using Microsoft.AspNetCore.Mvc;
using RecipeShare.Application.Contracts.Persistence.Service;
using RecipeShare.Application.Models;

namespace RecipeShare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipesController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
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
            var recipe = await _recipeService.GetRecipeByIdAsync(id);
            if (recipe == null)
                return NotFound();

            return Ok(recipe);
        }

        /// <summary>
        /// Returns a list of recipes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-list-recipes")]
        public async Task<ActionResult<List<RecipeDto>>> GetRecipesDto()
        {
            var recipes = await _recipeService.GetAllRecipesAsync();
            if (recipes == null || !recipes.Any())
                return Ok("There are no recipes to display");

            return Ok(recipes);
        }

        /// <summary>
        /// Adds a single recipe
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("adds-one-recipe")]
        public async Task<IActionResult> AddRecipe([FromBody] AddRecipesRequest request)
        {
            if (request == null)
                return BadRequest();

            var created = await _recipeService.AddRecipeAsync(request);
            return CreatedAtAction(nameof(GetRecipe), new { id = created.Id }, created);
        }

        /// <summary>
        /// Adds a list of recipes 
        /// </summary>
        /// <param name="recipes"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-list-recipes")]
        public async Task<IActionResult> AddRecipes([FromBody] List<AddRecipesRequest> request)
        {
            if (request == null || !request.Any())
                return BadRequest("The recipe list cannot be empty.");

            try
            {
                var result = await _recipeService.AddRecipesAsync(request);
                if (!result.AddedRecipes.Any())
                {
                    return Ok("No new recipes were added. All recipes already exist.");
                }

                return Ok($"{result.AddedRecipes.Count} recipe(s) added successfully. " +
                          $"{result.SkippedRecipes.Count} recipe(s) were skipped because they already exist.");
            }
            catch (Exception ex)
            {
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
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] AddRecipesRequest request)
        {
            if (request == null)
                return BadRequest("Request is null");

            await _recipeService.UpdateRecipeAsync(id, request);
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
            await _recipeService.DeleteRecipeAsync(id);
            return NoContent();
        }
    }
}
