using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory_mvc.Models;

namespace Inventory_mvc.Service
{
    interface IDepartmentService
    {
        List<Department> GetAllDepartment();
        Department GetDepartmentByCode(string deptCode);
        int UpdateDepartmentByCode(Department dept);
        Boolean CreateDepartment(Department dept);
        bool isExistingCode(string departmentCode);
    }
}
