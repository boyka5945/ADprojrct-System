using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.DAO;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.Service
{
    public class UserService : IUserService
    {
        private IUserDAO userDAO = new UserDAO();

        List<UserViewModel> IUserService.GetAllUserViewModel()
        {
            List<User> userList = userDAO.GetAllUser();

            List<UserViewModel> viewModelList = new List<UserViewModel>();
            foreach (User u in userList)
            {
                viewModelList.Add(ConvertToViewModel(u));
            }

            return viewModelList;
        }

        List<User> IUserService.GetUserByDept(UserViewModel user)
        {

            return userDAO.GetUserByDept(ConvertFromViewModel(user));
        }

        UserViewModel IUserService.FindByUserID(string userid)
        {
            return ConvertToViewModel(userDAO.FindByUserID(userid));
        }

        void IUserService.DelegateEmp(string userid, DateTime from, DateTime to)
        {
            userDAO.DelegateEmp(userid, from, to);
        }

        bool IUserService.UpdateUserInfo(UserViewModel userVM)
        {
            User user = ConvertFromViewModel(userVM);

            IUserDAO userDAO = new UserDAO();

            if (userDAO.UpdateUserInfo(user) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private User ConvertFromViewModel(UserViewModel userVM)
        {
            User user = new User();

            user.userID = userVM.UserID;
            user.password = userVM.Password;
            user.userEmail = userVM.UserEmail;
            user.role = userVM.Role;
            user.name = userVM.Name;
            user.departmentCode = userVM.DepartmentCode;
            user.contactNo = userVM.ContactNo;
            user.address = userVM.Address;
            user.delegationStart = userVM.DelegationStart;
            user.delegationEnd = userVM.DelegationEnd;
            return user;

        }

        private UserViewModel ConvertToViewModel(User u)
        {
            UserViewModel userVM = new UserViewModel();

            userVM.UserID = u.userID;
            userVM.UserEmail = u.userEmail;
            userVM.Password = u.password;
            userVM.Name = u.name;
            userVM.Role = u.role;
            userVM.DepartmentCode = u.departmentCode;
            userVM.Address = u.address;
            userVM.ContactNo = u.contactNo;
        //    userVM.DelegationStart = (DateTime) u.delegationStart;
        //    userVM.DelegationEnd = (DateTime) u.delegationEnd;
            return userVM ;
        }
    }
}