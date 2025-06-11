import { Link } from 'react-router-dom';
import '../../styles.css';
import React from 'react';
import './RecipeList.css';
import { RestService } from '../../services/RestService';
import '../details/RecipeDetails';
import type { IRecipeDetails } from '../details/RecipeDetails';
import { useState } from 'react';
import RecipeDetails from '../details/RecipeDetails';

interface IRecipe {
  // ID: number;
  id: number;
  title: string;
  ingredients: string;
  steps: string;
  cookingTime: number;
  dietaryTags: string;
  dietaryTagsId: number;
}

interface Props {
  recipes: IRecipe[];
  onDeleteRecipe: (id: number) => void;
}

const RecipeListComponent: React.FC<Props> = ({ recipes: recipeList, onDeleteRecipe }) => {
  const [localRecipes, setLocalRecipes] = useState<IRecipe[]>(() => {
    return recipeList.map((recipe) => ({ ...recipe }));
  });

  const onUpdateRecipe = async (recipe: IRecipe) => {
    try {
      console.log('Console log request for update: ', recipe.id, ' + ', recipe);
      const response = await RestService.updateRecipe(recipe.id, recipe);
      console.log('Console log response for update: ', response);
      setLocalRecipes(localRecipes.map((b) => (b.id === recipe.id ? recipe : b)));
    } catch (error) {
      console.error(error);
    }
  };

  const handleDeleteRecipe = async (id: number) => {
    try {
      const response = await RestService.deleteRecipe(id);
      if (response.status === 200 || response.status === 204) {
        console.log('Recipe deleted successfully');
        // Update the recipe list to reflect the deletion
        setLocalRecipes(localRecipes.filter((recipe) => recipe.id !== id));
      } else {
        console.error('Failed to delete recipe');
      }
    } catch (error) {
      console.error('Error deleting recipe:', error);
    }
  };

  return (
    <div className="recipe-list-container">
      <h2>Recipe List</h2>
      <table className="recipe-list-table">
        <thead>
          <tr>
            <th>Title</th>
            <th>Ingredients</th>
            <th>Steps</th>
            <th>Cooking Time</th>
            <th>Dietary Tags</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
        {localRecipes.map((recipe, index) => (
          <tr key={recipe.id || index}>
            <td>{recipe.title}</td>
            <td>{recipe.ingredients}</td>
            <td>{recipe.steps}</td>
            <td>{recipe.cookingTime}</td>
            <td>{recipe.dietaryTags}</td>
            <td>
              <Link to={`/recipes/${recipe.id}`} className="button details-button" >
                Details
              </Link>
            </td>
            <td>
              <button onClick={() => handleDeleteRecipe(recipe.id)} className="button delete-button" >
                Delete
              </button>
            </td>
          </tr>
        ))}
      </tbody>
      </table>
      <div className="add-recipe-button-container">
        <Link to="/create" className="button add-recipe-button">
          Add Recipe
        </Link>
      </div>
    </div>
  );
};

export default RecipeListComponent;
