﻿using System;
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
            return entity.Department.ToList();
        }

        public Department FindByDepartmentCode(string deptCode)
        {
            StationeryModel entity = new StationeryModel();
            return entity.Department.Where(x => x.departmentCode == deptCode).First();
        }

        public Boolean AddNewDepartment(Department dept)
        {

            try
            {
                StationeryModel entity = new StationeryModel();
                Department department = new Department();
                department.departmentCode = dept.departmentCode;
                department.departmentName = dept.departmentName;
                department.contactName = dept.contactName;
                department.phoneNo = dept.phoneNo;
                department.faxNo = dept.faxNo;
                department.departmentHeadID = dept.departmentHeadID;
                department.collectionPointID = dept.collectionPointID;
                department.representativeID = dept.representativeID;
                entity.Department.Add(department);
                entity.SaveChanges();
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
            return 10;
        }

        public Boolean DeleteDepartment(string deptCode)
        {
            return true;
        }
    }
}