using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RecipeBox
{
    public class Recipe
    {
        private int _id;
        private string _name;
        private string _ingredient;
        private string _instruction;
        private int _rating;

        public Recipe(string Name, string Ingredient, string Instruction, int Rating, int Id = 0)
        {
            _id = Id;
            _name = Name;
            _ingredient = Ingredient;
            _instruction = Instruction;
            _rating = Rating;
        }

        public override bool Equals(System.Object otherRecipe)
        {
            if (!(otherRecipe is Recipe))
            {
                return false;
            }
            else
            {
                Recipe newRecipe = (Recipe) otherRecipe;
                bool idEquality = (this.GetId() == newRecipe.GetId());
                bool nameEquality = (this.GetName() == newRecipe.GetName());
                bool ingredientEquality = (this.GetIngredient() == newRecipe.GetIngredient());
                bool instructionEquality = (this.GetInstruction() == newRecipe.GetInstruction());
                bool ratingEquality = (this.GetRating() == newRecipe.GetRating());
                return (idEquality && nameEquality && ingredientEquality && instructionEquality && ratingEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetName().GetHashCode();
        }

        public int GetId()
        {
            return _id;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetName(string newName)
        {
            _name = newName;
        }

        public string GetIngredient()
        {
            return _ingredient;
        }

        public void SetIngredient(string newIngredient)
        {
            _ingredient = newIngredient;
        }

        public string GetInstruction()
        {
            return _instruction;
        }

        public void SetInstruction(string newInstruction)
        {
            _instruction = newInstruction;
        }

        public int GetRating()
        {
            return _rating;
        }

        public void SetRating(int newRating)
        {
            _rating = newRating;
        }

        public static List<Recipe> GetAll()
        {
            List<Recipe> AllRecipes = new List<Recipe>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM recipes ORDER BY rating desc;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int recipeId = rdr.GetInt32(0);
                string recipeName = rdr.GetString(1);
                string recipeIngredient = rdr.GetString(2);
                string recipeInstruction = rdr.GetString(3);
                int recipeRating = rdr.GetInt32(4);
                Recipe newRecipe = new Recipe(recipeName, recipeIngredient, recipeInstruction, recipeRating, recipeId);
                AllRecipes.Add(newRecipe);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return AllRecipes;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO recipes (name, ingredient, instruction, rating) OUTPUT INSERTED.id VALUES (@RecipeName, @RecipeIngredient, @RecipeInstruction, @RecipeRating);", conn);

            cmd.Parameters.Add(new SqlParameter("@RecipeName", this.GetName()));
            cmd.Parameters.Add(new SqlParameter("@RecipeIngredient", this.GetIngredient()));
            cmd.Parameters.Add(new SqlParameter("@RecipeInstruction", this.GetInstruction()));
            cmd.Parameters.Add(new SqlParameter("@RecipeRating", this.GetRating()));

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

        }

        public static Recipe Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM recipes WHERE id = @RecipeId;", conn);

            cmd.Parameters.Add(new SqlParameter("@RecipeId", id.ToString()));

            SqlDataReader rdr = cmd.ExecuteReader();
            int foundRecipeId = 0;
            string foundRecipeName = null;
            string foundRecipeIngredient = null;
            string foundRecipeInstruction = null;
            int foundRecipeRating = 0;
            while(rdr.Read())
            {
                foundRecipeId = rdr.GetInt32(0);
                foundRecipeName = rdr.GetString(1);
                foundRecipeIngredient = rdr.GetString(2);
                foundRecipeInstruction = rdr.GetString(3);
                foundRecipeRating = rdr.GetInt32(4);
            }
            Recipe foundRecipe = new Recipe(foundRecipeName, foundRecipeIngredient, foundRecipeInstruction, foundRecipeRating, foundRecipeId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return foundRecipe;
        }


        public void AddCategory(Category newCategory)
        {
          SqlConnection conn = DB.Connection();
          conn.Open();

          SqlCommand cmd = new SqlCommand("INSERT INTO categories_recipes (recipe_id, category_id) VALUES (@RecipeId, @CategoryId);", conn);
          cmd.Parameters.Add(new SqlParameter("@RecipeId", this.GetId()));
          cmd.Parameters.Add(new SqlParameter("@CategoryId", newCategory.GetId()));

          cmd.ExecuteNonQuery();

          if (conn != null)
          {
            conn.Close();
          }
        }

        public List<Category> GetCategories()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT categories.* FROM recipes JOIN categories_recipes ON (recipes.id = categories_recipes.recipe_id) JOIN categories ON (categories_recipes.category_id = categories.id) WHERE recipes.id = @RecipeId;", conn);

            cmd.Parameters.Add(new SqlParameter("@RecipeId", this.GetId().ToString()));

            SqlDataReader rdr = cmd.ExecuteReader();

            List<Category> categories = new List<Category>{};

            while(rdr.Read())
            {
                int categoryId = rdr.GetInt32(0);
                string categoryName = rdr.GetString(1);
                Category newCategory = new Category(categoryName, categoryId);
                categories.Add(newCategory);

            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return categories;

        }

        public void UpdateRecipes(string newName, string newIngredient, string newInstruction, int newRating)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE recipes SET name = @NewName, ingredient = @NewIngredient, instruction = @NewInstruction, rating = @NewRating OUTPUT INSERTED.* WHERE id = @RecipeId;", conn);

            cmd.Parameters.Add(new SqlParameter("@NewName", newName));

            cmd.Parameters.Add(new SqlParameter("@NewIngredient", newIngredient));

            cmd.Parameters.Add(new SqlParameter("@NewInstruction", newInstruction));

            cmd.Parameters.Add(new SqlParameter("@NewRating", newRating));

            SqlParameter recipeIdParameter = new SqlParameter();
            recipeIdParameter.ParameterName = "@RecipeId";
            recipeIdParameter.Value = this.GetId();
            cmd.Parameters.Add(recipeIdParameter);


            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._name = rdr.GetString(1);
                this._ingredient = rdr.GetString(2);
                this._instruction = rdr.GetString(3);
                this._rating = rdr.GetInt32(4);
            }

            if (rdr != null)
            {
                rdr.Close();
            }

            if (conn != null)
            {
                conn.Close();
            }
        }

        public static Recipe FindByIngredient(string ingredient)
       {
           SqlConnection conn = DB.Connection();
           conn.Open();

           SqlCommand cmd = new SqlCommand("SELECT * FROM recipes WHERE ingredient = @RecipeIngredient;", conn);
           SqlParameter recipeIngredientParameter = new SqlParameter();
           recipeIngredientParameter.ParameterName = "@RecipeIngredient";
           recipeIngredientParameter.Value = ingredient;
           cmd.Parameters.Add(recipeIngredientParameter);
           SqlDataReader rdr = cmd.ExecuteReader();

           int foundRecipeId = 0;
           string foundRecipeName = null;
           string foundRecipeIngredient = null;
           string foundRecipeInstruction = null;
           int foundRecipeRating = 0;

           while(rdr.Read())
           {
               foundRecipeId = rdr.GetInt32(0);
               foundRecipeName = rdr.GetString(1);
               foundRecipeIngredient = rdr.GetString(2);
               foundRecipeInstruction = rdr.GetString(3);
               foundRecipeRating = rdr.GetInt32(4);
           }
           Recipe foundRecipe = new Recipe(foundRecipeName, foundRecipeIngredient, foundRecipeInstruction, foundRecipeRating, foundRecipeId);

           if (rdr != null)
           {
               rdr.Close();
           }
           if (conn != null)
           {
               conn.Close();
           }
           return foundRecipe;
       }

        public void Delete()
        {
          SqlConnection conn = DB.Connection();
          conn.Open();

          SqlCommand cmd = new SqlCommand("DELETE FROM recipes WHERE id = @RecipeId; DELETE FROM categories_recipes WHERE recipe_id = @RecipeId;", conn);

          cmd.Parameters.Add(new SqlParameter("@RecipeId", this.GetId()));

          cmd.ExecuteNonQuery();

          if (conn != null)
          {
            conn.Close();
          }
        }

            public static void DeleteAll()
        {
          SqlConnection conn = DB.Connection();
          conn.Open();
          SqlCommand cmd = new SqlCommand("DELETE FROM recipes;", conn);
          cmd.ExecuteNonQuery();
          conn.Close();
        }
    }
}
