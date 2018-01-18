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

        List<User> GetUserByDept(User user);

        User FindByUserID(string userID);

        bool AddNewUser(User user);

        int UpdateUserInfo(User user);

        void DelegateEmp(string userid, DateTime? from, DateTime? to);

        List<string> GetAllUserID();

        bool AssignRep(string userID);

        bool Remove_Delegate(string userID);

        List<int> FindAllRole(string id);
        
        bool FindRole(int role);

        List<int> RoleForEditAndCreate(string userID);

        bool AlrDelegated(string id);

        //  bool DeleteUser(string userID);
    }
}