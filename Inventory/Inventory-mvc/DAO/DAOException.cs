using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_mvc.DAO
{
    public class DAOException : Exception
    {
        public DAOException() : base()
        {
            
        }

        public DAOException(string s) : base(s)
        {
            
        }
    }
}