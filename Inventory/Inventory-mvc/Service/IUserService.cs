using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
namespace Inventory_mvc.Service
{
    public interface IUserService
    {
        List<User> GetAllUserViewModel();

        List<User> GetUserByDept(User user);

        User FindByUserID(string userid);

         void DelegateEmp(string userid, DateTime? from, DateTime? to);

        bool UpdateUserInfo(User userVM);

        bool isExistingID(string id);

        bool AddNewUser(User userVM);

        bool AssignRep(string userid);

        bool Remove_Delegate(string userid);

        List<int> FindAllRole(string id);

        

        string[] FindApprovingStaffsEmailByRequesterID(string requesterID);
        int GetRoleByID(string userID);
        string FindDeptCodeByID(string userid);

        string FindNameByID(string userid);
        bool FindRole(int role);

        List<int> RoleForEditAndCreate(string userid);

        bool AlrDelegated(string id);
    }
}