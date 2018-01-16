using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.DAO;

namespace Inventory_mvc.Service
{
    public class UserService : IUserService
    {
        private IUserDAO userDAO = new UserDAO();

        List<User> IUserService.GetAllUser()
        {
            return userDAO.GetAllUser();
        }
    }
}