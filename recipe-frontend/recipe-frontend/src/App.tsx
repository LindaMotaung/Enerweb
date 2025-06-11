import './styles.css';
import { BrowserRouter, Route, Routes, Link } from 'react-router-dom';
import RecipeList from './components/list/RecipeList';
import RecipeDetails from './components/details/RecipeDetails';
import CreateRecipe from './components/create/CreateRecipe';
import React, { useState, useEffect  } from 'react';
import 'react-toastify/dist/ReactToastify.css';
import { ICreateRecipe } from './components/create/CreateRecipe';
import { RestService } from './services/RestService';

interface IRecipeApp {
  //id: number;
  ID: number;
  title: string;
  ingredients: string;
  steps: string;
  cookingTime: number;
  dietaryTagsId: number;
  dietaryTags: string;
}

function App() {
  const [recipes, setRecipes] = useState<IRecipeApp[]>([]);

  useEffect(() => {
    getRecipes().then((data) => setRecipes(data));
  }, []);
  
  const getRecipes = async () => {
    try {
      const response = await RestService.getRecipes();
      return response.data;
    } catch (error) {
      console.error(error);
      return [];
    }
  };

  const handleAddRecipe = (recipe: ICreateRecipe) => {
    const newRecipe = {
      ID: Math.max(...recipes.map((b) => b.ID)) + 1 || 1,
      title: recipe.title,
      ingredients: recipe.ingredients,
      steps: recipe.steps,
      cookingTime: recipe.cookingTime,
      dietaryTagsId: 0,
      dietaryTags: ''
    };
    setRecipes([...recipes, newRecipe]);
  };

  const handleUpdateRecipe = (updatedRecipe) => {
    setRecipes((prevRecipes) =>
      prevRecipes.map((recipe) =>
        recipe.ID === updatedRecipe.ID ? updatedRecipe : recipe
      )
    );
  };
  
  const handleDeleteRecipe = (id: number) => {
    const recipeToDelete = recipes.find((recipe) => recipe.ID === id);
    if (recipeToDelete) {
      setRecipes(recipes.filter((recipe) => recipe.ID !== id));
      alert(`Recipe '${recipeToDelete.title}' deleted successfully!`);
    }
  };
  
  return (
    <BrowserRouter>
      <nav>
        <ul>
          <li>
            {/* <Link to="/">
              <img src="/favicon.ico" alt="Recipe Logo" width="20" />
            </Link> */}
          </li>
          <li>
            <Link to="/">Home</Link>
          </li>
          <li>
            <Link to="/create">Create Recipe</Link>
          </li>
        </ul>
      </nav>
      <main>
        <Routes>
          <Route
            path="/"
            element={<RecipeList recipes={recipes} onDeleteRecipe={handleDeleteRecipe} />}
          />
          <Route
            path="/recipes/:id"
            element={
              <RecipeDetails
                recipes={recipes}
                onUpdateRecipe={handleUpdateRecipe}
                onDeleteRecipe={handleDeleteRecipe}
              />
            }
          />
        <Route path="/create" element={<CreateRecipe onAddRecipe={handleAddRecipe} recipes={recipes.map(recipe => ({ id: recipe.ID, title: recipe.title, ingredients: recipe.ingredients, steps: recipe.steps, cookingTime: recipe.cookingTime }))} />} />
        </Routes>
      </main>
    </BrowserRouter>
  );
}

export default App;
