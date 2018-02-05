using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.DAO;
using Inventory_mvc.ViewModel;
using Inventory_mvc.Utilities;
using Inventory_mvc.Function;

namespace Inventory_mvc.Service
{
    public class UserService : IUserService
    {
        private IUserDAO userDAO = new UserDAO();

        public List<string> GetStoreRoles()
        {
            List<string> roleList = userDAO.GetStoreRoles();        
            return roleList;
        }

        public List<User> GetUserByDept(User user)
        {
            List<User> userList=userDAO.GetUserByDept(user);
            List<User> users = new List<User>();

            foreach (User s in userList)
            {
                if (s.userID != user.userID)
                    users.Add(s);
            }
            return users;
        }

        public User FindByUserID(string userid)
        {
            return userDAO.FindByUserID(userid);
        }

        public ChangePasswordViewModel changePasswordUser(User user)
        {
          //  User user = userDAO.FindByUserID(uid);
            ChangePasswordViewModel viewModel = ConvertToViewModel(user);
            return viewModel;
        }

        public void DelegateEmp(string userid, DateTime? from, DateTime? to)
        {
            userDAO.DelegateEmp(userid, from, to);
        }

        public bool UpdateUserInfo(User userVM)
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
        private User ConvertFromViewModel(ChangePasswordViewModel userVM)
        {
            User user = new User();

            user.userID = userVM.UserID;
            user.password = Encrypt.EncryptMethod(userVM.ConfirmPassword);
            return user;

        }



        private ChangePasswordViewModel ConvertToViewModel(User u)
        {
            ChangePasswordViewModel userVM = new ChangePasswordViewModel();

            userVM.UserID = u.userID;
            return userVM;
        }


        public bool isExistingID(string userid)
        {
            string id = userid.ToUpper().Trim();

            return userDAO.GetAllUserID().Contains(id);
        }

        public bool AddNewUser(User userVM)
        {
            return userDAO.AddNewUser(userVM);
        }

        public bool AssignRep(string userid)
        {
            return userDAO.AssignRep(userid);
        }

        public bool Remove_Delegate(string userid)
        {
            return userDAO.Remove_Delegate(userid);
        }

        public List<int> FindAllRole(string id)
        {
            return userDAO.FindAllRole(id);
        }

        public bool FindRole(int role)
        {
            return userDAO.FindRole(role);
        }

        public string[] FindApprovingStaffsEmailByRequesterID(string requesterID, int[] approvingRoleID)
        {
            return userDAO.FindApprovingStaffsEmailByRequesterID(requesterID, approvingRoleID);
        }

        public string FindDeptCodeByID(string userid)
        {
            return userDAO.FindByUserID(userid).departmentCode;
        }

        public string FindNameByID(string userid)
        {
            return userDAO.FindByUserID(userid).name;
        }

        public bool AlrDelegated(string userid)
        {
            return userDAO.AlrDelegated(userid);
        }
        public int GetRoleByID(string userID)
        {
            return userDAO.GetRoleByID(userID);
        }

        public bool IsStoreManager(string userID)
        {
            // StoreManager = 5
            return (GetRoleByID(userID) == (int) UserRoles.RoleID.StoreManager) ? true : false;
        }

        public bool IsStoreSupervisor(string userID)
        {
            // StoreSupervisor = 6
            return (GetRoleByID(userID) == (int)UserRoles.RoleID.StoreSupervisor) ? true : false;
        }


        void IUserService.AutoRemove(User user)
        {
            userDAO.AutoRomove(user);
        }


        //ps
        public bool isSame(string password, string oldpassword)
        {

            if (password != oldpassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool isSameWithPassword(string password, string newpassword)
        {

            if (password == newpassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool changePassword(ChangePasswordViewModel vm)
        {
            User user = ConvertFromViewModel(vm);

            IUserDAO userDAO = new UserDAO();

            if (userDAO.changePassword(user) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<User> FindUsersByRole(int roleID)
        {
            return userDAO.FindUsersByRole(roleID);
        }

        public bool Promote(string uid)
        {
            return userDAO.Promote(uid);
        }

        public bool Demote(string uid)
        {
            return userDAO.Demote(uid);
        }
    }
}