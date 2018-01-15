using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.DAO;
using Inventory_mvc.Models;

namespace Inventory_mvc.Service
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        PurchaseOrderDAO dao = new PurchaseOrderDAO();

        public List<Purchase_Order_Record> GetAllPurchaseOrder()
        {
            
            return dao.GetAllPurchaseOrder();

        }

        public List<Purchase_Details> GetPurchaseDetailsByOrderNo(int orderNo)
        {
            return dao.GetPurchaseDetailsByOrderNo(orderNo);
            
        }
    }
}