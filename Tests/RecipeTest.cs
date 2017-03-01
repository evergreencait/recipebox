using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RecipeBox
{
    public class RecipeBoxTest : IDisposable
    {
        public RecipeBoxTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=recipe_box_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_DatabaseEmptyAtFirst()
        {
            //Arrange, Act
            int result = Recipe.GetAll().Count;

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame_true()
        {
            //Arrange, Act
            Recipe firstRecipe = new Recipe("Mac and cheese", "cheese and noodles", "cook it", 5);
            Recipe secondRecipe = new Recipe("Mac and cheese", "cheese and noodles", "cook it", 5);

            //Assert
            Assert.Equal(firstRecipe, secondRecipe);
        }
        public void Dispose()
        {
        //   Recipe.DeleteAll();
        //   Category.DeleteAll();
        }
    }
}
