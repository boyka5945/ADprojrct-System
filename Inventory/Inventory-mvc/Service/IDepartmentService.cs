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

        bool isExistingCode(string departmentCode);
    }
}
