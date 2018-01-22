using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public class DepartmentDAO : IDepartmentDAO
    {
        public List<Department> GetAllDepartments()
        {
            StationeryModel entity = new StationeryModel();
            return entity.Departments.ToList();
        }

        public Department FindByDepartmentCode(string deptCode)
        {
            StationeryModel entity = new StationeryModel();
            return entity.Departments.Where(x => x.departmentCode == deptCode).First();
        }

        public Boolean AddNewDepartment(Department dept)
        {

            try
            {
                StationeryModel entity = new StationeryModel();
                Department department = new Department();
                string capDeptCode = dept.departmentCode.ToUpper();
                department.departmentCode = capDeptCode;
                department.departmentName = dept.departmentName;
                department.contactName = dept.contactName;
                department.phoneNo = dept.phoneNo;
                department.faxNo = dept.faxNo;
                //department.departmentHeadID = dept.departmentHeadID;
                if (dept.collectionPointID.ToString() == "--Select--")
                {
                    return false;
                }
                else {
                    department.collectionPointID = dept.collectionPointID;
                    //department.representativeID = dept.representativeID;
                    entity.Departments.Add(department);
                    entity.SaveChanges();
                }
                
            }
            catch
            {
                return false;
            }
            return true;
        }

        public int UpdateDepartmentInfo(Department dept)
        {

            StationeryModel entity = new StationeryModel();
            Department dt = entity.Departments.Where(x => x.departmentCode == dept.departmentCode).First();
            dt.departmentName = dept.departmentName;
            dt.contactName = dept.contactName;
            dt.phoneNo = dept.phoneNo;
            dt.faxNo = dept.faxNo;
            //dt.departmentHeadID = dept.departmentHeadID;
            dt.collectionPointID = dept.collectionPointID;
            //dt.representativeID = dept.representativeID;
            //dt = dept;
            int row = entity.SaveChanges();

            return row;
        }

        public Boolean DeleteDepartment(string deptCode)
        {
            return true;
        }

        List<string> IDepartmentDAO.GetAllDepartmentCode()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from d in context.Departments
                        select d.departmentCode).ToList();
            }
        }
    }
}