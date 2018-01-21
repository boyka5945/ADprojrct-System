using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_mvc.Utilities
{
    public static class TransactionTypes
    {
        public const string STOCK_ADJUSTMENT = "Stock Adjustment";
        public const string STOCK_RECIEVE = "Stock Received";
        public const string DISBURSEMENT = "Disbursement";
    }
}