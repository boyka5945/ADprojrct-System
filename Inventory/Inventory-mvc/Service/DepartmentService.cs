using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.DAO;

namespace Inventory_mvc.Service
{
    public class DepartmentService : IDepartmentService
    {
        public List<Department> GetAllDepartment()
        {
            DepartmentDAO dDAO= new DepartmentDAO();
            return dDAO.GetAllDepartments();
        }

        public Department GetDepartmentByCode(string deptCode)
        {
            DepartmentDAO dDAO = new DepartmentDAO();
            return dDAO.FindByDepartmentCode(deptCode);
        }
    }
}