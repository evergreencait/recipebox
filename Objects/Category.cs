using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RecipeBox
{
    public class Category
    {
        private int _id;
        private string _name;

        public Category(string Name, int Id = 0)
        {
            _id = Id;
            _name = Name;
        }


        public override bool Equals(System.Object otherCategory)
        {
            if (!(otherCategory is Category))
            {
                return false;
            }
            else
            {
                Category newCategory = (Category) otherCategory;
                bool idEquality = this.GetId() == newCategory.GetId();
                bool nameEquality = this.GetName() == newCategory.GetName();
                return (idEquality && nameEquality);
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

        public static List<Category> GetAll()
        {
            List<Category> AllCategories = new List<Category>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM categories;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int categoryId = rdr.GetInt32(0);
                string categoryName = rdr.GetString(1);
                Category newCategory= new Category(categoryName, categoryId);
                AllCategories.Add(newCategory);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return AllCategories;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO categories (name) OUTPUT INSERTED.id VALUES (@CategoryName);", conn);

            cmd.Parameters.Add(new SqlParameter("@CategoryName", this.GetName()));

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


        public static Category Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM categories WHERE id = @CategoryId;", conn);

            cmd.Parameters.Add(new SqlParameter("@CategoryId", id.ToString()));

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundCategoryId = 0;
            string foundCategoryName = null;

            while(rdr.Read())
            {
                foundCategoryId = rdr.GetInt32(0);
                foundCategoryName = rdr.GetString(1);
            }
            Category foundCategory = new Category(foundCategoryName, foundCategoryId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return foundCategory;
        }

        public void AddRecipe(Recipe newRecipe)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO categories_recipes (recipe_id, category_id) VALUES (@RecipeId, @CategoryId);", conn);

            cmd.Parameters.Add(new SqlParameter("@CategoryId", this.GetId()));
            cmd.Parameters.Add(new SqlParameter("@RecipeId", newRecipe.GetId()));

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }

        public List<Recipe> GetRecipes()
        {
          SqlConnection conn = DB.Connection();
          conn.Open();

          SqlCommand cmd = new SqlCommand("SELECT recipes.* FROM categories JOIN categories_recipes ON (categories.id = categories_recipes.category_id) JOIN recipes ON (categories_recipes.recipe_id = recipes.id) WHERE categories.id = @CategoryId;", conn);

          cmd.Parameters.Add(new SqlParameter("@CategoryId", this.GetId().ToString()));

          SqlDataReader rdr = cmd.ExecuteReader();

          List<Recipe> recipes = new List<Recipe>{};

          while(rdr.Read())
          {
              int recipeId = rdr.GetInt32(0);
              string recipeName = rdr.GetString(1);
              string recipeIngredient = rdr.GetString(2);
              string recipeInstruction = rdr.GetString(3);
              int recipeRating = rdr.GetInt32(4);

              Recipe newRecipe = new Recipe(recipeName, recipeIngredient, recipeInstruction, recipeRating, recipeId);
              recipes.Add(newRecipe);
          }

          if (rdr != null)
          {
              rdr.Close();
          }
          if (conn != null)
          {
              conn.Close();
          }
          return recipes;
        }

        public void UpdateCategories(string newName)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE categories SET name = @NewName OUTPUT INSERTED.* WHERE id = @CategoryId;", conn);

            cmd.Parameters.Add(new SqlParameter("@NewName", newName));

            SqlParameter categoryIdParameter = new SqlParameter();
            categoryIdParameter.ParameterName = "@CategoryId";
            categoryIdParameter.Value = this.GetId();
            cmd.Parameters.Add(categoryIdParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._name = rdr.GetString(1);
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

        public void Delete()
        {
          SqlConnection conn = DB.Connection();
          conn.Open();

          SqlCommand cmd = new SqlCommand("DELETE FROM categories WHERE id = @CategoryId; DELETE FROM categories_recipes WHERE category_id = @CategoryId;", conn);

          cmd.Parameters.Add(new SqlParameter("@CategoryId", this.GetId()));

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
          SqlCommand cmd = new SqlCommand("DELETE FROM categories;", conn);
          cmd.ExecuteNonQuery();
          conn.Close();
        }
    }
}
