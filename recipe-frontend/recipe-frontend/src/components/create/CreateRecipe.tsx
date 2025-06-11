import './CreateRecipe.css';
import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { RestService } from '../../services/RestService';
import dietaryTagsMapper from '../../utils/dietaryTagsMapper';

export interface ICreateRecipe {
  id: number;
  title: string;
  ingredients: string;
  steps: string;
  cookingTime: number;
}

interface Props {
  onAddRecipe: (recipe: ICreateRecipe) => void;
  recipes: ICreateRecipe[];
}

const CreateRecipe: React.FC<Props> = ({ onAddRecipe, recipes }) => {
  const navigate = useNavigate();
  const [title, setTitle] = useState('');
  const [ingredients, setIngredients] = useState('');
  const [steps, setSteps] = useState('');
    const [cookingTime, setCookingTime] = useState<number>(0);
  const [dietaryTag, setDietaryTags] = useState('');
  const [id, setId] = useState(0);
  const [successMessage, setSuccessMessage] = useState('');

  useEffect(() => {
    const nextId = recipes.length > 0 ? Math.max(...recipes.map((recipe) => recipe.id)) + 1 : 1;
    setId(nextId);
  }, [recipes]);

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const dietaryTagsMapperId = dietaryTagsMapper(dietaryTag);
    const newRecipe = {
      title,
      ingredients: ingredients,
      steps: steps,
      cookingTime: cookingTime,
      dietaryTags: [dietaryTag],
    };
    console.log('newRecipe:', newRecipe);
    try {
      const response = await RestService.addRecipe(newRecipe);
      console.log('Created recipe response:', response.data);
      setSuccessMessage(`Recipe "${title}" created successfully!`);
      setTimeout(() => {
        setSuccessMessage('');
        navigate(`/recipes/${response.data.id}`);
      }, 800);
    } catch (error) {
      console.error(error);
    }
  };
  

  return (
    <div className="create-recipe-container">
      <form className="create-recipe-form" onSubmit={handleSubmit}>
        <table>
          <tbody>
            <tr>
              <td className="create-recipe-label">Title:</td>
              <td>
                <input
                  className="create-recipe-input"
                  type="text"
                  id="title"
                  value={title}
                  onChange={(event) => setTitle(event.target.value)}
                />
              </td>
            </tr>
            <tr>
              <td className="create-recipe-label">Ingredients:</td>
              <td>
                <input
                  className="create-recipe-input"
                  type="text"
                  id="ingredients"
                  value={ingredients}
                  onChange={(event) => setIngredients(event.target.value)}
                />
              </td>
            </tr>
            <tr>
              <td className="create-recipe-label">Steps:</td>
              <td>
              <input
                className="create-recipe-input"
                type="text"
                id="steps"
                value={steps}
                onChange={(event) => setSteps(event.target.value)}
              />
              </td>
            </tr>
            <tr>
              <td className="create-recipe-label">Cooking Time:</td>
              <td>
                <input
                  className="create-recipe-input"
                  type="text"
                  id="cookingTime"
                  value={cookingTime}
                  onChange={(event) => setCookingTime(parseInt(event.target.value, 10))}
                />
              </td>
            </tr>
            <tr>
              <td className="create-recipe-label">Dietary Tags:</td>
              <td>
                <input
                  className="create-recipe-input"
                  type="text"
                  id="dietaryTag"
                  value={dietaryTag}
                  onChange={(event) => setDietaryTags(event.target.value)}
                />
              </td>
            </tr>
          </tbody>
        </table>
        <button className="create-recipe-button" type="submit">
          Add Recipe
        </button>
        {successMessage && <p className="success-message">{successMessage}</p>}
      </form>
    </div>
  );
};

export default CreateRecipe;