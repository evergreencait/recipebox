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
        public void Dispose()
        {
        //   Recipe.DeleteAll();
        //   Category.DeleteAll();
        }
    }
}
