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

        List<User> GetUserByDept(UserViewModel user);

        UserViewModel FindByUserID(string userid);

        void DelegateEmp(string userid, DateTime from, DateTime to);

        bool UpdateUserInfo(UserViewModel userVM);
    }
}