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

            SqlCommand cmd = new SqlCommand("SELECT * FROM recipes;", conn);
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
