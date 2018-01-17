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
                    User u = (from a in entity.User where a.userID == user.userID select a).First();
                    u.userID = user.userID;
                    u.password = user.password;
                    u.address = user.address;
                    u.role = user.role;
                    u.userEmail = user.userEmail;
                    u.name = user.name;
                    u.contactNo = user.contactNo;
                    //u.delegationStart = user.delegationStart;
                    //u.delegationEnd = user.delegationEnd;
                    //u.departmentCode = user.departmentCode;
                    int rowAffected = entity.SaveChanges();
                    return rowAffected;
                }
                catch (Exception)
                {
                    throw new Exception("incorrect userid");
                }
            }
        }

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

        List<string> IUserDAO.GetAllUserID()
        {

            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.User
                        select s.userID).ToList();
            }
        }

        bool IUserDAO.AssignRep(string userID)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                User rep = (from r in entity.User where r.role == "UserRepresentative" select r).First();
                rep.role = "Employee";
                User user = (from u in entity.User where u.userID == userID select u).First();
                user.role = "UserRepresentative";

                int rowAffected = entity.SaveChanges();

                if (rowAffected == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        bool IUserDAO.Remove_Delegate(string userID)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                int i = 0;
                User user = (from u in entity.User where u.userID == userID select u).First();
                List<User> emplist = (from emps in entity.User where (emps.userID != userID && emps.departmentCode==user.departmentCode) select emps).ToList<User>();
                foreach (User u in emplist)
                {
                    if (u.role == "UserRepresentative")
                    {
                        i++;
                    }
                }
                if (i < 1)
                {
                    user.role = "UserRepresentative";
                }
                else
                    user.role = "Employee";


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

        List<string> IUserDAO.FindAllRole()
        {
            using (StationeryModel entity = new StationeryModel())
            {
                return (from u in entity.User select u.role).ToList<string>();
            }

        }

        bool IUserDAO.FindRole(string dept)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                User u = (from d in entity.User where d.role == dept select d).First();
                if (u == null)
                    return false;
                return true;
            }
        }

    }
}