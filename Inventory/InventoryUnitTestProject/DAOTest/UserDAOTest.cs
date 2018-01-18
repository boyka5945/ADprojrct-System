using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Inventory_mvc.DAO;
using Inventory_mvc.Models;

namespace InventoryUnitTestProject
{
    [TestClass]
    public class UserDAOTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            IUserDAO dao = new UserDAO();
            string requesterID = "S1013";

            // Act
            var result = dao.FindApprovingStaffsEmailByRequesterID(requesterID);

            // Assert
            Assert.AreEqual(result[0], "barryallen@logic.sg");
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
