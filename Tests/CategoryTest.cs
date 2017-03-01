using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RecipeBox
{
    public class CategoryTest : IDisposable
    {
        public CategoryTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=recipe_box_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_CategoryEmptyAtFirst()
        {
            //Arrange, Act
            int result = Category.GetAll().Count;

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_Equal_ReturnsTrueForSameName()
        {
            //Arrange, Act
            Category firstCategory = new Category("Mexican");
            Category secondCategory = new Category("Mexican");

            //Assert
            Assert.Equal(firstCategory, secondCategory);
        }

        [Fact]
        public void Test_Save_SavesCategoryToDatabase()
        {
            //Arrange
            Category testCategory = new Category("Mexican");
            testCategory.Save();

            //Act
            List<Category> result = Category.GetAll();
            List<Category> testList = new List<Category>{testCategory};

            //Assert
            Assert.Equal(testList, result);
        }

        [Fact]
        public void Test_Save_AssignsIdToCategoryObject()
        {
          //Arrange
          Category testCategory = new Category("Mexican");
          testCategory.Save();

          //Act
          Category savedCategory = Category.GetAll()[0];

          int result = savedCategory.GetId();
          int testId = testCategory.GetId();

          //Assert
          Assert.Equal(testId, result);
        }

        [Fact]
        public void Test_AddRecipe_AddsRecipeToCategory()
        {
          //Arrange
          Recipe testRecipe = new Recipe("Mac and cheese", "cheese and noodles", "cook it", 5);
          testRecipe.Save();

          Category testCategory = new Category("Mexican");
          testCategory.Save();

          //Act
          testCategory.AddRecipe(testRecipe);

          List<Recipe> result = testCategory.GetRecipes();
          List<Recipe> testList = new List<Recipe>{testRecipe};

          //Assert
          Assert.Equal(testList, result);
        }

        [Fact]
        public void UpdateCategories_UpdateCategoriesInDatabase_true()
        {
            //Arrange
            string name = "Mexican";

            Category testCategory = new Category(name);
            testCategory.Save();
            string newName = "Thai";

            //Act
            testCategory.UpdateCategories(newName);
            Category result = Category.GetAll()[0];

            //Assert
            Assert.Equal(testCategory, result);
        }

        public void Dispose()
        {
            Recipe.DeleteAll();
            Category.DeleteAll();
        }
    }

}
