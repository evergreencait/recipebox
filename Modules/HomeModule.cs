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
        }
    }
}
