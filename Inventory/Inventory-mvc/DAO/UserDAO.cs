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
        public List<string> GetStoreRoles()
        {
            StationeryModel entity = new StationeryModel();
            List<string> roles = (from role in entity.roleInfoes where (role.roleID != 1 && role.roleID != 2 && role.roleID != 3 && role.roleID != 4 && role.roleID != 5 && role.roleID != 8) select role.roleName).ToList<string>();
            return roles;
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
            //using (StationeryModel context = new StationeryModel())
            //{
            //    User u = (from x in context.Users
            //                    where x.userID == user.userID
            //                    select x).FirstOrDefault();

            //    //u.userID = user.userID;
            //    //u.password = user.password;
            //    u.address = user.address;
            //    //u.role = user.role;
            //    u.userEmail = user.userEmail;
            //    u.name = user.name;
            //    u.contactNo = user.contactNo;
            //    //u.delegationStart = user.delegationStart;
            //    //u.delegationEnd = user.delegationEnd;
            //    //u.departmentCode = user.departmentCode;
            //    int rowAffected = context.SaveChanges();
            //    return rowAffected;
            //}
            StationeryModel entity = new StationeryModel();
            try
            {
                User u = (from a in entity.Users where a.userID == user.userID select a).FirstOrDefault();
                //u.userID = user.userID;
                //u.password = user.password;
                u.address = user.address;
                //u.role = user.role;
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
                int UR = 0;
                int supervisor = 0;
                User user = (from u in entity.Users where u.userID == userID select u).First();
                List<User> emplist = (from emps in entity.Users where (emps.userID != userID && emps.departmentCode == user.departmentCode) select emps).ToList<User>();
                foreach (User u in emplist)
                {
                    if (u.role == (int)UserRoles.RoleID.UserRepresentative)
                    {
                       UR++;
                    }
                    else if(u.role==(int )UserRoles.RoleID.StoreSupervisor)
                    {
                        supervisor++;
                    }
                }
                if (UR < 1) // no userrepresentative in list
                {
                    user.role = (int) UserRoles.RoleID.UserRepresentative;
                }
                else
                    user.role = 3; //assign as employee

                if (supervisor < 1)
                {
                    user.role = 6;
                }
                else
                    user.role = 7;

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

        //List<string> IUserDAO.RoleForEditAndCreate(string userID)
        //{
        //    StationeryModel entity = new StationeryModel();
        //    List<string> roles;
        //    User u = (from a in entity.Users where a.userID == userID select a).First();
        //    //List<int> temp = (from user in entity.Users where (user.departmentCode == u.departmentCode && user.role != 2 && user.role != 8) select user.role).ToList<int>();
        //   // List<int> roleids = (from user in entity.Users where (user.departmentCode == u.departmentCode && user.role != 2 && user.role != 8) select user).ToList<int>()
        //    if (u.departmentCode== "STORE")
        //    {
        //        roles=(from role in entity.roleInfoes where (role.roleID!=1 && role.roleID!=2 && role.roleID!=3 && role.roleID!=8)
        //    }


        //    return roles;
        //}

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

        public void AutoRomove(User user)
        {

            using (StationeryModel entity = new StationeryModel())
            {
                int UR = 0;
                int supervisor = 0;
                User actDeptHead = new User();
                actDeptHead = null;
                List<User> users = (from u in entity.Users where u.departmentCode==user.departmentCode select u).ToList<User>();

                foreach(User check in users)
                {
                    if(check.role==8)
                    {
                        actDeptHead = (from user1 in entity.Users where (user1.role == 8 && user1.departmentCode == user.departmentCode) select user1).First();
                    }
                }
                if(actDeptHead!=null)
                {
                    DateTime endDate = (DateTime)actDeptHead.delegationEnd;
                    DateTime tdy = DateTime.Today;
                    if (tdy.CompareTo(endDate) > 0)
                    {
                        foreach (User u in users)
                        {
                            switch (u.role)
                            {
                                case 4:
                                    UR++;
                                    break;
                                case 6:
                                    supervisor++;
                                    break;
                            }
                        }

                        if (user.role == 2) // if otherdepts
                        {
                            if (UR < 1) // no userrepresentative in list
                            {
                                actDeptHead.role = 4; //assign as ur
                            }
                            else
                                actDeptHead.role = 3; //assign as employee
                        }
                        else        // if store
                        {
                            if (supervisor < 1)
                            {
                                actDeptHead.role = 6;
                            }
                            else
                                actDeptHead.role = 7;
                        }

                        actDeptHead.delegationStart = null;
                        actDeptHead.delegationEnd = null;
                    }
                    
                    entity.SaveChanges();
                }
            }
                
        }

        public int changePassword(User user)
        {
            using (StationeryModel context = new StationeryModel())
            {
                User u = (from x in context.Users
                                where x.userID == user.userID
                                select x).FirstOrDefault();

                u.password = user.password;

                int rowAffected = context.SaveChanges();

                return rowAffected;
            }

           
        }
    }
}