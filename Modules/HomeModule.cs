using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace RecipeBox
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => {
                List<Category> AllCategories = Category.GetAll();
                return View["index.cshtml", AllCategories];
            };

            Get["/recipes"] = _ => {
                List<Recipe> AllRecipes = Recipe.GetAll();
                return View["recipes.cshtml", AllRecipes];
            };
            Get["/categories"] = _ => {
                List<Category> AllCategories = Category.GetAll();
                return View["categories.cshtml", AllCategories];
            };

            Get["/categories/new"] = _ => {
                return View["categories_form.cshtml"];
            };
            Post["/categories/new"] = _ => {
                Category newCategory = new Category(Request.Form["category-name"]);
                newCategory.Save();
                return View["categories.cshtml", Category.GetAll()];
            };

            Get["/recipes/new"] = _ => {
                List<Category> AllCategories = Category.GetAll();
                return View["recipes_form.cshtml", AllCategories];
            };
            Post["/recipes/new"] = _ => {
                Recipe newRecipe = new Recipe(Request.Form["recipe-name"], Request.Form["recipe-ingredient"], Request.Form["recipe-instruction"], Request.Form["recipe-rating"], Request.Form["category-id"]);
                newRecipe.Save();
                return View["recipes.cshtml", Recipe.GetAll()];
            };

            Get["recipes/{id}"] = parameters => {
              Dictionary<string, object> model = new Dictionary<string, object>();
              Recipe SelectedRecipe = Recipe.Find(parameters.id);
              List<Category> RecipeCategories = SelectedRecipe.GetCategories();
              List<Category> AllCategories = Category.GetAll();
              model.Add("recipe", SelectedRecipe);
              model.Add("recipeCategories", RecipeCategories);
              model.Add("allCategories", AllCategories);
              return View["recipe.cshtml", model];
          };

            Post["/categories/delete"] = _ => {
                Category.DeleteAll();
                Recipe.DeleteAll();
                return View["index.cshtml"];
            };
            Post["/recipes/delete"] = _ => {
                Recipe.DeleteAll();
                return View["index.cshtml"];
            };
        }
    }
}
