using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_mvc.ViewModel
{
    public class JSONForCombobox
    {
        public string id { get; set; }
        public string text { get; set; }

        public JSONForCombobox() { }

        public JSONForCombobox(string id, string text)
        {
            this.id = id;
            this.text = text;
        }
    }
}