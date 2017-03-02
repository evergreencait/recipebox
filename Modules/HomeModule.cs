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
            Get["/categories/{id}"] = parameters => {
                Dictionary<string, object> model = new Dictionary<string, object>();
                var SelectedCategory = Category.Find(parameters.id);
                var CategoryRecipes = SelectedCategory.GetRecipes();
                List<Recipe> AllRecipes = Recipe.GetAll();
                model.Add("category", SelectedCategory);
                model.Add("categoryRecipes", CategoryRecipes);
                model.Add("allRecipes", AllRecipes);
                return View["category.cshtml", model];
            };

            Post["/recipe/add_category"] = _ => {
                Category category = Category.Find(Request.Form["category-id"]);
                Recipe recipe = Recipe.Find(Request.Form["recipe-id"]);
                recipe.AddCategory(category);
                return View["category-detail.cshtml", recipe.GetCategories()];
            };
            Post["/category/add_recipe"] = _ => {
                Category category = Category.Find(Request.Form["category-id"]);
                Recipe recipe = Recipe.Find(Request.Form["recipe-id"]);
                category.AddRecipe(recipe);
                return View["recipe-detail.cshtml", category.GetRecipes()];
            };

            //     Get["category/edit/{id}"] = parameters => {
            //        Category SelectedCategory = Category.Find(parameters.id);
            //        return View["edit_category.cshtml", SelectedCategory];
            //    };
            //
            //    Patch["cuisine/edit/{id}"] = parameters => {
            //        Category SelectedCategory = Category.Find(parameters.id);
            //        SelectedCategory.UpdateName(Request.Form["category-name"]);
            //        return View["categories.cshtml", Category.GetAll()];
            //    };

            Delete["categories/{id}"] = parameters =>
            {
                Category targetCategory = Category.Find(parameters.id);
                targetCategory.Delete();
                return View["categories.cshtml", Category.GetAll()];
            };

            Delete["recipes/{id}"] = parameters =>
            {
                Recipe targetRecipe = Recipe.Find(parameters.id);
                targetRecipe.Delete();
                return View["recipes.cshtml", Recipe.GetAll()];
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
