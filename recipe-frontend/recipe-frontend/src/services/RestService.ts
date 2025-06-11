import axios, { AxiosResponse } from 'axios';

export class RestService {
    private static baseUrl(): string {
        return 'https://localhost:7113/';
      }

      public static recipe(recipeId?: number){ 
        //If no recipeId is provided then return a list of all recipes
        return `${this.baseUrl()}/api/Recipes/${recipeId ? recipeId : ''}`;
      }

      public static async getRecipe(recipeId: number): Promise<AxiosResponse<any>> {
        //Gets a single recipe
        return axios.get(`${this.baseUrl()}api/Recipes/${recipeId}`);
      }

      public static async getRecipes(): Promise<AxiosResponse<any>> {
        return axios.get(`${this.baseUrl()}api/Recipes/get-list-recipes`);
      }

      public static async deleteRecipe(recipeId: number): Promise<AxiosResponse<any>> {
        return axios.delete(`${this.baseUrl()}api/Recipes/${recipeId}`);
      }

      public static async addRecipe(recipe: any): Promise<AxiosResponse<any>> {
        return axios.post(`${this.baseUrl()}api/Recipes/adds-one-recipe`, recipe);
      }

      public static async updateRecipe(recipeId: number, recipe: any): Promise<AxiosResponse<any>> {
        return axios.put(`${this.baseUrl()}api/Recipes/${recipeId}`, recipe);
      }
 }