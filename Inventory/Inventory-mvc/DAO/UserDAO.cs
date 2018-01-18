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
                return (from u in entity.Users select u).ToList<User>();
            }
        }

        List<User> IUserDAO.GetUserByDept(User user)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                return (from u in entity.Users where u.departmentCode == user.departmentCode select u).ToList();
            }
        }

        User IUserDAO.FindByUserID(string userID)
        {
            string userid = userID.ToUpper().Trim();
            using (StationeryModel entity = new StationeryModel())
            {
                return (from user in entity.Users where user.userID == userid select user).FirstOrDefault();
            }
        }

        bool IUserDAO.AddNewUser(User user)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                entity.Users.Add(user);
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
                    User u = (from a in entity.Users where a.userID == user.userID select a).First();
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
                User u = (from user in entity.Users where user.userID == userid select user).FirstOrDefault();
                u.delegationStart = from;
                u.delegationEnd = to;
                u.role = 8;

                entity.SaveChanges();

            }
        }

        List<string> IUserDAO.GetAllUserID()
        {

            using (StationeryModel context = new StationeryModel())
            {
                return (from s in context.Users
                        select s.userID).ToList();
            }
        }

        bool IUserDAO.AssignRep(string userID)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                User rep = (from r in entity.Users where r.role == 4 select r).First();
                rep.role = 3;
                User user = (from u in entity.Users where u.userID == userID select u).First();
                if(user.role!= 8)
                {
                    user.role = 4;
                }
                

                int rowAffected = entity.SaveChanges();

                if (rowAffected <= 2)
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
                User user = (from u in entity.Users where u.userID == userID select u).First();
                List<User> emplist = (from emps in entity.Users where (emps.userID != userID && emps.departmentCode==user.departmentCode) select emps).ToList<User>();
                foreach (User u in emplist)
                {
                    if (u.role == 4)
                    {
                        i++;
                    }
                }
                if (i < 1)
                {
                    user.role = 4;
                }
                else
                    user.role = 3;
                user.delegationStart = null;
                user.delegationEnd = null;


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

        List<int> IUserDAO.FindAllRole()
        {
            using (StationeryModel entity = new StationeryModel())
            {
                return (from u in entity.Users select u.role).ToList<int>();
            }

        }

        bool IUserDAO.FindRole(int role)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                User u = (from d in entity.Users where d.role == role select d).First();
                if (u == null)
                    return false;
                return true;
            }
        }

        string[] IUserDAO.FindApprovingStaffsEmailByRequesterID(string requesterID)
        {
            using (StationeryModel context = new StationeryModel())
            {
                string deptCode = (from u in context.Users
                                   where u.userID == requesterID
                                   select u.departmentCode).First();

                // DeptHead = 2, ActingDeptHead = 8
                // TODO: Update this method after database update

                string[] deptHeadEmail = (from u in context.Users
                                          where u.departmentCode == deptCode &
                                          (u.role == 2 || u.role == 8)                                         
                                          select u.userEmail).ToArray();

                return deptHeadEmail;
            }
        }


    }
}