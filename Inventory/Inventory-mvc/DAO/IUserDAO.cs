using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public interface IUserDAO
    {
        List<User> GetAllUser();

        User FindByUserID(string userID);

        bool AddNewUser(User user);

        int UpdateUserInfo(User user);

      //  bool DeleteUser(string userID);
    }
}