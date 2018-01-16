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

        List<Purchase_Details> GetPurchaseDetailsByOrderNo(int orderNo);

        Purchase_Order_Record FindByOrderID(int orderID);

        //void AddPurchaseDetail(int deliveryOrderNo, string itemCode, int qty, string remarks);

    }
}