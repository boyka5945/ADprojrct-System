using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    interface IDepartmentDAO
    {
        List<Department> GetAllDepartments();

        Department FindByDepartmentCode(string deptCode);

        bool AddNewDepartment(Department dept);

        int UpdateDepartmentInfo(Department dept);

        bool DeleteDepartment(string deptCode);

        List<string> GetAllDepartmentCode();
    }
}
