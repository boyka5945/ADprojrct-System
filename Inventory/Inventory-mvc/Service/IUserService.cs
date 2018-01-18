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
        List<UserViewModel> GetAllUserViewModel();

        List<UserViewModel> GetUserByDept(UserViewModel user);

        UserViewModel FindByUserID(string userid);

        void DelegateEmp(string userid, DateTime from, DateTime to);

        bool UpdateUserInfo(UserViewModel userVM);

        bool isExistingID(string id);

        bool AddNewUser(UserViewModel userVM);

        bool AssignRep(string userid);

        bool Remove_Delegate(string userid);

        List<int> FindAllRole();

        

        string[] FindApprovingStaffsEmailByRequesterID(string requesterID);

        string FindDeptCodeByID(string userid);

        string FindNameByID(string userid);
        bool FindRole(int role);
    }
}