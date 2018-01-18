using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Entity;
using Inventory_mvc.Models;

namespace Inventory_mvc.Function
{
    public class RoleService
    {
        public RoleInfos GetById(int id)
        { 
            List<rolePermission> rolePermissionList = new List<rolePermission>();
            List<permissionInfo> permissions = new List<permissionInfo>();
            string rolename = "";
            string description = "";
            using (StationeryModel context = new StationeryModel())
            {
                rolePermissionList = context.rolePermissions.Where(x => x.roleID == id).ToList();
                if (context.roleInfoes.Where(x => x.roleID == id).ToList().Count > 0)
                {
                    rolename = context.roleInfoes.Where(x => x.roleID == id).First().roleName.ToString();
                    description = context.roleInfoes.Where(x => x.roleID == id).First().description.ToString();

                    foreach (var p in rolePermissionList)
                    {
                        permissions.Add(context.permissionInfoes.Where(x => x.permissionID == p.permissionID).First());
                    }
                }
            
            }
            return new RoleInfos
            {
                RoleId = id,
                RoleName = rolename,
                Description = description,
                Permissions = permissions,
            };
        }

        //saving the permission into DB
        public void CreatePermissions(int permissionID, string controller, string action)
        {
            using (StationeryModel context = new StationeryModel())
            {
                permissionInfo p = new permissionInfo();
                p.permissionID = permissionID;
                p.action = action;
                p.controller = controller;
                p.description = "";
                context.permissionInfoes.Add(p);
                context.SaveChanges();
            }
        }

        //get permission from DB
        public List<permissionInfo> GetDefinedPermissions()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return context.permissionInfoes.ToList();
            }   
        }

        //Add role in DB
        public void AddRole(RoleInfos role)
        {
            using (StationeryModel context = new StationeryModel())
            {
                roleInfo r = new roleInfo();
                r.roleID = role.RoleId;
                r.roleName = role.RoleName;
                r.description = role.Description;
                context.roleInfoes.Add(r);
                foreach (var ps in role.Permissions)
                {
                    rolePermission rp = new rolePermission();
                    rp.roleID = role.RoleId;
                    rp.permissionID = ps.permissionID;

                    context.rolePermissions.Add(rp);
                }
                context.SaveChanges();
            }
        }

    }
}