using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.DAO;
using System.Web.Mvc;

namespace Inventory_mvc.Service
{
    public class DepartmentService : IDepartmentService
    {
        private IDepartmentDAO departmentDAO = new DepartmentDAO();
        public List<Department> GetAllDepartment()
        {
            DepartmentDAO dDAO = new DepartmentDAO();
            return dDAO.GetAllDepartments();
        }

        public Department GetDepartmentByCode(string deptCode)
        {
            string dCode = deptCode.ToUpper().Trim();
            DepartmentDAO dDAO = new DepartmentDAO();
            return dDAO.FindByDepartmentCode(dCode);
        }

        public int UpdateDepartmentByCode(Department dept)
        {
            DepartmentDAO dDAO = new DepartmentDAO();
            return dDAO.UpdateDepartmentInfo(dept);
        }

        public Boolean CreateDepartment(Department dept)
        {
            DepartmentDAO dDAO = new DepartmentDAO();
            return dDAO.AddNewDepartment(dept);
        }

        bool IDepartmentService.isExistingCode(string departmentCode)
        {
            string code = departmentCode.ToUpper().Trim();
            //string code = departmentCode.Trim();

            return departmentDAO.GetAllDepartmentCode().Contains(code);
        }

        

    }
}