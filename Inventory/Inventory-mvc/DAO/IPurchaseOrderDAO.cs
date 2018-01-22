using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public interface IPurchaseOrderDAO
    {
        List<Purchase_Order_Record> GetAllPurchaseOrder();

        Purchase_Order_Record FindByOrderID(int orderID);

        Boolean AddNewPurchaseOrder(Purchase_Order_Record purchase_order_record);

        int UpdatePurchaseOrderInfo(Purchase_Order_Record purchase_order_record);

        Boolean DeletePurchaseOrder(int  orderID);

        List<Purchase_Order_Record> FindByStatus(string status);
        List<Purchase_Order_Record> FindBySupplier(string supplier);

        int UpdatePurchaseDetailsInfo(Purchase_Detail pd);
    }
}