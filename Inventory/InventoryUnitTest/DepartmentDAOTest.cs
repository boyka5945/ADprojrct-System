using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Inventory_mvc;
using Inventory_mvc.DAO;


namespace InventoryUnitTest
{
    [TestClass]
    public class DepartmentDAOTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            DepartmentDAO 

            // Act

            // Assert
        }

        /*
        [TestMethod]
        public void Create_Post_ReturnsViewIfModelStateIsNotValid()
        {
            // Arrange
            HomeController controller = GetHomeController(new InMemoryContactRepository());
            // Simply executing a method during a unit test does just that - executes a method, and no more. 
            // The MVC pipeline doesn't run, so binding and validation don't run.
            controller.ModelState.AddModelError("", "mock error message");
            Contact model = GetContactNamed(1, "", "");

            // Act
            var result = (ViewResult)controller.Create(model);

            // Assert
            Assert.AreEqual("Create", result.ViewName);
        }
        */

    }
}
