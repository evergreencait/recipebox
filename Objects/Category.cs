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
    }
}
