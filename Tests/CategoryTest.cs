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

        public void Dispose()
        {
             Recipe.DeleteAll();
             Category.DeleteAll();
        }

        [Fact]
        public void Test_Find_FindCategoryInDatabase()
        {
            //Arrange
            Category testCategory = new Category("Mexican");
            testCategory.Save();

            //Act
            Category foundCategory = Category.Find(testCategory.GetId());

            //Assert
            Assert.Equal(testCategory,foundCategory);
        }
    }
}
