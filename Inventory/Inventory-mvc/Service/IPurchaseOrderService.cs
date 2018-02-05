using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.Service
{
    public interface IPurchaseOrderService
    {
        List<Purchase_Order_Record> GetAllPurchaseOrder();

        List<Purchase_Detail> GetPurchaseDetailsByOrderNo(int orderNo);

        Purchase_Order_Record FindByOrderID(int orderID);

        bool DeletePurchaseOrder(int orderID);

        int UpdatePurchaseOrderInfo(Purchase_Order_Record purchase_order_record);

        bool AddNewPurchaseOrder(Purchase_Order_Record purchase_order_record);

        bool AddPurchaseDetail(string deliveryOrderNo, string itemCode, int qty, string remarks, decimal price);

        bool AddPurchaseDetail(Purchase_Detail pd);

        List<Purchase_Order_Record> FindByStatus(string status);
        List<Purchase_Order_Record> FindBySupplier(string supplier);

        int UpdatePurchaseDetailsInfo(Purchase_Detail pd);
    }
}