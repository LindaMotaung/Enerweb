import { RestService } from '../../services/RestService';
import '../../styles.css';
import dietaryTagsMapper from '../../utils/dietaryTagsMapper';
import './RecipeDetails.css';
import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

export interface IRecipeDetails {
  // ID: number;
  id: number;
  title: string;
  ingredients: string;
  cookingTime: number;
  steps: string;
  dietaryTagsId: number;
  dietaryTags: string;
}

interface Props {
  recipes: IRecipeDetails[];
  onUpdateRecipe: (recipe: IRecipeDetails) => void;
}

const RecipeDetails: React.FC<Props> = ({ recipes: recipeList, onUpdateRecipe }) => {

  const { id } = useParams();
  const navigate = useNavigate();
  const [recipe, setRecipe] = useState<IRecipeDetails | null>(null);
  const [title, setTitle] = useState('');
  const [ingredients, setIngredients] = useState('');
  const [steps, setSteps] = useState('');
  const [cookingTime, setCookingTime] = useState<number>(0);
  const [dietaryTags, setDietaryTags] = useState(recipe?.dietaryTags || '');
  const [isUpdating, setIsUpdating] = useState(false);

  useEffect(() => {
    if (!id || isNaN(Number(id))) return;
    const fetchRecipe = async () => {
      try {
        const response = await RestService.getRecipe(Number(id));
        setRecipe(response.data);
        setTitle(response.data.title);
        setIngredients(response.data.ingredients);
        setSteps(response.data.steps);
        setCookingTime(response.data.cookingTime);
        const dietaryTagName = dietaryTagsMapper(response.data.dietaryTags);
        setDietaryTags(dietaryTagName.toString());
      } catch (error) {
        console.error(error);
      }
    };
    fetchRecipe();
  }, [id]);


  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
  
    const updatedRecipe: IRecipeDetails = {
      id: recipe ? recipe.id : 0,
      title,
      ingredients,
      steps,
      cookingTime,
      dietaryTagsId: recipe ? recipe.dietaryTagsId : 0,
      dietaryTags: dietaryTags,
    };
  
    try {
      // Calling API
      const response = await RestService.updateRecipe(updatedRecipe.id, updatedRecipe);
      console.log('Updated recipe via API:', response);
  
      // 💾 Notify parent so it updates app state too
      onUpdateRecipe(updatedRecipe);
  
      setIsUpdating(false);
      navigate(`/recipes/${updatedRecipe.id}`);
    } catch (error) {
      console.error('Update failed:', error);
    }
  };


  const handleUpdateClick = () => {
    setIsUpdating(true);
    setTitle(recipe ? recipe.title: 'null title');
    setIngredients(recipe ? recipe.ingredients: 'null ingredient');
    setSteps(recipe ? recipe.steps : 'null steps');
    setCookingTime(recipe ? recipe.cookingTime : 0);
    setDietaryTags(recipe ? recipe.dietaryTags : 'null dietary tags');
  };

  const handleEditRecipe = async (recipe: IRecipeDetails) => {
    // For now, we're just simulating an edit by modifying the title
    const updatedRecipe = { ...recipe, title: recipe.title };
  
    // Call the update method
    await onUpdateRecipe(updatedRecipe);
  };

  if (!recipe) {
    return <p>Recipe not found</p>;
  }

  return (
    <div className="recipe-details-container">
      <h2>{recipe.title}</h2>
      <table className="recipe-details-table">
        <tbody>
          <tr>
            <th>Ingredients:</th>
            <td>{recipe.ingredients}</td>
          </tr>
          <tr>
            <th>Steps:</th>
            <td>{recipe.steps}</td>
          </tr>
          <tr>
            <th>Cooking Time:</th>
            <td>{recipe.cookingTime}</td>
          </tr>
          <tr>
            <th>Dietary Tags:</th>
            <td>{recipe.dietaryTags}</td>
          </tr>
        </tbody>
      </table>
      {!isUpdating && (
        <div>
          <button
            className="update-button"
            type="button"
            onClick={handleUpdateClick}
          >
            Update
          </button>
        </div>
      )}
      {isUpdating && (
        <form className="update-form" onSubmit={handleSubmit}>
          <table className="recipe-details-table">
            <tbody>
              <tr>
                <th>Title:</th>
                <td>
                  <input
                    className="recipe-details-input"
                    type="text"
                    value={title}
                    onChange={(event) => setTitle(event.target.value)}
                  />
                </td>
              </tr>
              <tr>
                <th>Ingredients:</th>
                <td>
                  <input
                    className="recipe-details-input"
                    type="text"
                    value={ingredients}
                    onChange={(event) => setIngredients(event.target.value)}
                  />
                </td>
              </tr>
              <tr>
                <th>Steps:</th>
                <td>
                  <input
                    className="recipe-details-input"
                    type="text"
                    value={steps}
                    onChange={(event) => setSteps(event.target.value)}
                  />
                </td>
              </tr>
              <tr>
                <th>Cooking Time:</th>
                <td>
                  <input
                    className="recipe-details-input"
                    type="text"
                    value={cookingTime}
                    onChange={(event) => setCookingTime(parseInt(event.target.value, 10))}
                  />
                </td>
              </tr>
              <tr>
                <th>Dietary Tags:</th>
                <td>
                  <input
                    className="recipe-details-input"
                    type="text"
                    value={dietaryTags || ""}
                    onChange={(event) => setDietaryTags(event.target.value)}
                  />
                </td>
              </tr>
            </tbody>
          </table>
          <button className="update-button" type="submit">
            Save Changes
          </button>
        </form>
      )}
    </div>
  );
};

export default RecipeDetails;