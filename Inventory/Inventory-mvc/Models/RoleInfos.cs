using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.Entity
{
    public class RoleInfos
    {
        public virtual int RoleId
        {
            set;
            get;
        }
        public virtual string RoleName
        {
            set;
            get;
        }
        public virtual string Description
        {
            set;
            get;
        }
        public virtual ICollection<permissionInfo> Permissions
        {
            set;
            get;
        }
    }
}