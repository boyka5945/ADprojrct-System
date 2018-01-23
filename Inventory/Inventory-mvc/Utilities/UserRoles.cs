using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventory_mvc.Utilities
{
    public static class UserRoles
    {
        //roleID    roleName
        //1	        superAdmin
        //2	        DepartmentHead
        //3	        Employee
        //4	        UserRepresentative
        //5	        StoreManager
        //6	        StoreSupervisor
        //7	        StoreClerk
        //8	        ActingDepartmentHead

        public enum RoleID
        {
            superAdmin = 1,
            DepartmentHead,
            Employee,
            UserRepresentative,
            StoreManager,
            StoreSupervisor,
            StoreClerk,
            ActingDepartmentHead
        }


        public static Dictionary<string, List<SelectListItem>> GetCreatableRolesForDepartment = new Dictionary<string, List<SelectListItem>>()
        {
            { "Store", new List<SelectListItem>()
                           {
                                new SelectListItem() { Value = ((int) RoleID.StoreSupervisor).ToString(), Text = STORE_SUPERVISOR },
                                new SelectListItem() { Value = ((int) RoleID.StoreClerk).ToString(), Text = STORE_CLERK }
                           }
            },
            { "Others", new List<SelectListItem>()
                           {
                                new SelectListItem() { Value = RoleID.Employee.ToString(), Text = EMPLOYEE }
                           }
            }
        };
            
     
        public const string SUPER_ADMIN = "superAdmin";
        public const string DEPT_HEAD = "DepartmentHead";
        public const string EMPLOYEE = "Employee";
        public const string USER_REP = "UserRepresentative";
        public const string STORE_MANAGER = "StoreManager";
        public const string STORE_SUPERVISOR = "StoreSupervisor";
        public const string STORE_CLERK = "StoreClerk";
        public const string ACTING_DEPT_HEAD = "ActingDepartmentHead";
    }
}