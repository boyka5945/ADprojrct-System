using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_mvc.Function
{
    public class EmailException : Exception
    {
        public EmailException() : base()
        {

        }

        public EmailException(string s) : base(s)
        {

        }

    }
}