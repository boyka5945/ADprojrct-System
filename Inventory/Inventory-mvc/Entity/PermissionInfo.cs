using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_mvc.Entity
{
    public class PermissionInfo
    {
        public virtual int PermissonId
        {
            set;
            get;
        }
        public virtual string Controller
        {
            set;
            get;
        }
        public virtual string Action
        {
            set;
            get;
        }
        public virtual string description
        {
            set;
            get;
        }
    }
}