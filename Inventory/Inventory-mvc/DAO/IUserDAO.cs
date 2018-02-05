using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public interface IUserDAO
    {
        List<string> GetStoreRoles();

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

        bool AlrDelegated(string id);

        string[] FindApprovingStaffsEmailByRequesterID(string requesterID, int[] approvingRoleID);

        int GetRoleByID(string userID);

        void AutoRomove(User user);

        int changePassword(User user);
        List<User> FindUsersByRole(int roleID);

        bool Promote(string uid);

        bool Demote(string uid);
    }
}