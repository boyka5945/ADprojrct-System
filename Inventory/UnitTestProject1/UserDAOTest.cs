using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory_mvc.DAO;

namespace UnitTestProject1
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
            var result = dao.FindApprovingStaffsEmailByRequesterID(requesterID, new int[] { 2 });

            // Assert
            Assert.AreEqual(result[0], "barryallen@logic.sg");
        }
    }
}
