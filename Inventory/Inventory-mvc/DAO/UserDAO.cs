using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public class UserDAO : IUserDAO
    {
        List<User> IUserDAO.GetAllUser()
        {
            using (StationeryModel entity = new StationeryModel())
            {
                return (from u in entity.User select u).ToList<User>();
            }
        }

        List<User> IUserDAO.GetUserByDept(User user)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                return (from u in entity.User where u.departmentCode == user.departmentCode select u).ToList();
            }
        }

        User IUserDAO.FindByUserID(string userID)
        {
            string userid = userID.ToUpper().Trim();
            using (StationeryModel entity = new StationeryModel())
            {
                return (from user in entity.User where user.userID == userid select user).FirstOrDefault();
            }
        }

        bool IUserDAO.AddNewUser(User user)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                entity.User.Add(user);
                int rowAffected = entity.SaveChanges();

                if (rowAffected == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        int IUserDAO.UpdateUserInfo(User user)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                try
                {
                    User u = (from a in entity.User where a.userID == user.userID select a).LastOrDefault();
                    u.userID = user.userID;
                    u.password = user.password;
                    u.address = user.address;
                    u.role = user.role;
                    u.userEmail = user.userEmail;
                    u.name = user.name;
                    u.contactNo = user.contactNo;
                    u.delegationStart = user.delegationStart;
                    u.delegationEnd = user.delegationEnd;
                    u.departmentCode = user.departmentCode;
                    int rowAffected = entity.SaveChanges();
                    return rowAffected;
                }
                catch (Exception)
                {
                    throw new Exception("incorrect userid");
                }
            }
        }

        //bool IUserDAO.DeleteUser(string userID)
        //{
        //    using (StationeryModel entity = new StationeryModel())
        //    {

        //    }
        //}

        void IUserDAO.DelegateEmp(string userid, DateTime from, DateTime to)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                User u = (from user in entity.User where user.userID == userid select user).FirstOrDefault();
                u.delegationStart = from;
                u.delegationEnd = to;
                u.role = "ActingDeptHead";

                entity.SaveChanges();

            }
        }

    }
}