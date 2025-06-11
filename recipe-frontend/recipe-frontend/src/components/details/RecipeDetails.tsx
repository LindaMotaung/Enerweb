import { RestService } from '../../services/RestService';
import '../../styles.css';
import dietaryTagsMapper from '../../utils/dietaryTagsMapper';
import './RecipeDetails.css';
import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

export interface IRecipeDetails {
  ID: number;
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
  onDeleteRecipe: (id: number) => void; 
}

const RecipeDetails: React.FC<Props> = ({ recipes: recipeList, onUpdateRecipe, onDeleteRecipe }) => {

  const { id } = useParams();
  const navigate = useNavigate();
  const [recipe, setRecipe] = useState<IRecipeDetails | null>(null);
  const [title, setTitle] = useState('');
  const [ingredients, setIngredients] = useState('');
  const [steps, setSteps] = useState('');
  const [cookingTime, setCookingTime] = useState<number>(0);
  const [genre, setGenre] = useState(recipe?.dietaryTags || '');
  const [isUpdating, setIsUpdating] = useState(false);

  useEffect(() => {
    const recipeFound = recipeList.find((recipe) => recipe.ID === Number(id));
    if(recipeFound){
        setRecipe(recipeFound);
        if (recipeFound && recipeFound.dietaryTags) {
          setTitle(recipeFound.title);
          setIngredients(recipeFound.ingredients);
          setSteps(recipeFound.steps);
          setCookingTime(recipeFound.cookingTime);
          const dietaryTagName = dietaryTagsMapper(recipeFound.dietaryTags);
          setGenre(dietaryTagName.toString());
        }
    }
    else{
        setRecipe(null);
    }
   
  }, [recipeList, id, dietaryTagsMapper]);


  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
  
    const updatedRecipe: IRecipeDetails = {
      ID: recipe ? recipe.ID : 0,
      title,
      ingredients,
      steps,
      cookingTime,
      dietaryTagsId: recipe ? recipe.dietaryTagsId : 0,
      dietaryTags: genre,
    };
  
    try {
      // Calling API
      const response = await RestService.updateRecipe(updatedRecipe.ID, updatedRecipe);
      console.log('Updated recipe via API:', response);
  
      // 💾 Notify parent so it updates app state too
      onUpdateRecipe(updatedRecipe);
  
      setIsUpdating(false);
      navigate(`/recipes/RecipeDetails/${updatedRecipe.ID}`);
    } catch (error) {
      console.error('Update failed:', error);
    }
  };
  

  const handleDeleteClick = () => {
    if (id !== undefined) {
    onDeleteRecipe(parseInt(id));
    navigate('/recipes');
    }
    else{
        console.error('Recipe ID is undefined');
    }
  };

  const handleUpdateClick = () => {
    setIsUpdating(true);
    setTitle(recipe ? recipe.title: 'null title');
    setIngredients(recipe ? recipe.ingredients: 'null ingredient');
    setSteps(recipe ? recipe.steps : 'null steps');
    setCookingTime(recipe ? recipe.cookingTime : 0);
    setGenre(recipe ? recipe.dietaryTags : 'null dietary tags');
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
          &nbsp; &nbsp;
          <button
            className="update-button"
            type="button"
            onClick={handleDeleteClick}
          >
            Delete
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
                <th>Genre:</th>
                <td>
                  <input
                    className="recipe-details-input"
                    type="text"
                    value={genre || ""}
                    onChange={(event) => setGenre(event.target.value)}
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