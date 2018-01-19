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

        List<User> IUserService.GetAllUserViewModel()
        {
            List<User> userList = userDAO.GetAllUser();

            List<User> viewModelList = new List<User>();
            foreach (User u in userList)
            {
                viewModelList.Add(u);
            }

            return viewModelList;
        }

        List<User> IUserService.GetUserByDept(User user)
        {
            List<User> userList=userDAO.GetUserByDept(user);
            List<User> viewModelList = new List<User>();

            foreach (User s in userList)
            {
                if (s.userID != user.userID)
                    viewModelList.Add(s);
            }
            return viewModelList;
        }

        User IUserService.FindByUserID(string userid)
        {
            return userDAO.FindByUserID(userid);
        }

        void IUserService.DelegateEmp(string userid, DateTime? from, DateTime? to)
        {
            userDAO.DelegateEmp(userid, from, to);
        }

        bool IUserService.UpdateUserInfo(User userVM)
        {
           // User user = ConvertFromViewModel(userVM);

            IUserDAO userDAO = new UserDAO();

            if (userDAO.UpdateUserInfo(userVM) == 1)
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
            userVM.DelegationStart = (DateTime?)u.delegationStart;
            userVM.DelegationEnd = (DateTime?)u.delegationEnd;
            return userVM ;
        }

        
        bool IUserService.isExistingID(string userid)
        {
            string id = userid.ToUpper().Trim();

            return userDAO.GetAllUserID().Contains(id);
        }

        bool IUserService.AddNewUser(User userVM)
        {
            return userDAO.AddNewUser(userVM);
        }

        bool IUserService.AssignRep(string userid)
        {
            return userDAO.AssignRep(userid);
        }

        bool IUserService.Remove_Delegate(string userid)
        {
            return userDAO.Remove_Delegate(userid);
        }

        List<int> IUserService.FindAllRole(string id)
        {
            return userDAO.FindAllRole(id);
        }

        bool IUserService.FindRole(int role)
        {
            return userDAO.FindRole(role);
        }

        string[] IUserService.FindApprovingStaffsEmailByRequesterID(string requesterID)
        {
            return userDAO.FindApprovingStaffsEmailByRequesterID(requesterID);
        }

        string IUserService.FindDeptCodeByID(string userid)
        {
            return userDAO.FindByUserID(userid).departmentCode;
        }

        string IUserService.FindNameByID(string userid)
        {
            return userDAO.FindByUserID(userid).name;
        }


        //List<string> IUserService.RoleForEditAndCreate(string userid)
        //{
        //    return userDAO.RoleForEditAndCreate(userid);
        //}


        bool IUserService.AlrDelegated(string userid)
        {
            return userDAO.AlrDelegated(userid);
        }
        int IUserService.GetRoleByID(string userID)
        {
            return userDAO.GetRoleByID(userID);
        }

        void IUserService.AutoRemove()
        {
            userDAO.AutoRomove();
        }
    }
}