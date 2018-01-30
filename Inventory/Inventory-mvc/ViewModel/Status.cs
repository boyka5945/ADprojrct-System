using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_mvc.ViewModel
{
    public class Status
    {
    
        public Boolean status;

       
        public string itemCode;

        public Status(string itemCode)
        {
            this.status = true;
            this.itemCode = itemCode;
        }
    }
}