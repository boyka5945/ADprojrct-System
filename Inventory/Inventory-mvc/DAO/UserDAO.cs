using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Utilities;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public class UserDAO : IUserDAO
    {
        public List<User> GetAllUser()
        {
            StationeryModel entity = new StationeryModel();
            return (from u in entity.Users select u).ToList<User>();
            
        }

        public List<User> GetUserByDept(User user)
        {
            StationeryModel entity = new StationeryModel();

            return (from u in entity.Users where u.departmentCode == user.departmentCode select u).ToList();

        }

        public User FindByUserID(string userID)
        {
            StationeryModel entity = new StationeryModel();
            var a = (from user in entity.Users where user.userID == userID select user).First();

            return a;
        }

        public bool AddNewUser(User user)
        {
            StationeryModel entity = new StationeryModel();
            
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

        public int UpdateUserInfo(User user)
        {
            StationeryModel entity = new StationeryModel();
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

        public void DelegateEmp(string userid, DateTime? from, DateTime? to)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                DateTime start = (DateTime)from;
                DateTime end = (DateTime)to;
                User u = (from user in entity.Users where user.userID == userid select user).FirstOrDefault();
                u.delegationStart = start.Date;
                u.delegationEnd = end.Date;
                u.role = (int) UserRoles.RoleID.ActingDepartmentHead;

                entity.SaveChanges();

            }
        }

        public List<string> GetAllUserID()
        {

            StationeryModel context = new StationeryModel();
            return (from s in context.Users select s.userID).ToList();
            
        }

        public bool AssignRep(string userID)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                User rep = (from r in entity.Users where r.role == (int) UserRoles.RoleID.UserRepresentative select r).First();
                rep.role = (int) UserRoles.RoleID.Employee;
                User user = (from u in entity.Users where u.userID == userID select u).First();
                if (user.role != (int)UserRoles.RoleID.ActingDepartmentHead)
                {
                    user.role = (int)UserRoles.RoleID.UserRepresentative;
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

        public bool Remove_Delegate(string userID)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                int i = 0;
                User user = (from u in entity.Users where u.userID == userID select u).First();
                List<User> emplist = (from emps in entity.Users where (emps.userID != userID && emps.departmentCode == user.departmentCode) select emps).ToList<User>();
                foreach (User u in emplist)
                {
                    if (u.role == (int)UserRoles.RoleID.UserRepresentative)
                    {
                        i++;
                    }
                }
                if (i < 1)
                {
                    user.role = (int) UserRoles.RoleID.UserRepresentative;
                }
                else
                    user.role = (int)UserRoles.RoleID.Employee;
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

        public List<int> FindAllRole(string id)
        {
            StationeryModel entity = new StationeryModel();
            User user = (from u in entity.Users where u.userID == id select u).First();
            return (from a in entity.Users where a.departmentCode == user.departmentCode select a.role).ToList<int>();
            

        }

        public bool FindRole(int role)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                User u = (from d in entity.Users where d.role == role select d).First();
                if (u == null)
                    return false;
                return true;
            }
        }

        public List<int> RoleForEditAndCreate(string userID)
        {
            StationeryModel entity = new StationeryModel();
            User u = (from a in entity.Users where a.userID == userID select a).First();
            List<int> roles = (from user in entity.Users where (user.departmentCode == u.departmentCode && user.role != 2 && user.role != 8) select user.role).ToList<int>();
            return roles;
        }

        public bool AlrDelegated(string id)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                User user = (from u in entity.Users where u.userID == id select u).First();
                if (user.role == (int)UserRoles.RoleID.ActingDepartmentHead)
                {
                    return true;
                }
                return false;
            }
        }

        public string[] FindApprovingStaffsEmailByRequesterID(string requesterID)
        {
            using (StationeryModel context = new StationeryModel())
            {
                string deptCode = (from u in context.Users
                                   where u.userID == requesterID
                                   select u.departmentCode).First();

                int requesterRole = (from u in context.Users
                                        where u.userID == requesterID
                                        select u.role).First();

                int[] approvingRoleID = { -1, -1 };

                // Employee = 3 | UR = 4 ==> DeptHead = 2, ActingDeptHead = 8
                // StoreClerk = 7 ==> Manager = 5 | Supervisor? = 6

                if (requesterRole == (int)UserRoles.RoleID.Employee || requesterRole == (int)UserRoles.RoleID.UserRepresentative)
                {
                    approvingRoleID = new int[] { (int)UserRoles.RoleID.DepartmentHead, (int)UserRoles.RoleID.ActingDepartmentHead };
                }
                else if(requesterRole == (int)UserRoles.RoleID.StoreClerk)
                {
                    approvingRoleID = new int[] { (int)UserRoles.RoleID.StoreManager };
                }

                string[] deptHeadEmail = (from u in context.Users
                                          where u.departmentCode == deptCode &
                                          (approvingRoleID.Contains(u.role))
                                          select u.userEmail).ToArray();

                return deptHeadEmail;
            }
        }

        public int GetRoleByID(string userID)
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from u in context.Users
                        where u.userID == userID
                        select u.role).FirstOrDefault();
            }
        }
    }
}