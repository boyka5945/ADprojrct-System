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

        public Purchase_Order_Record FindByOrderID(int orderID)
        {
            return dao.FindByOrderID(orderID);
        }

        public bool AddNewPurchaseOrder(Purchase_Order_Record purchase_order_record)
        {
            return dao.AddNewPurchaseOrder(purchase_order_record);
        }

        public bool DeletePurchaseOrder(int orderID)
        {
            dao.DeletePurchaseOrder(orderID);
            return true;
        }

        public int UpdatePurchaseOrderInfo(Purchase_Order_Record purchase_order_record)
        {
            return dao.UpdatePurchaseOrderInfo(purchase_order_record);
            

        }

        public List<Purchase_Detail> GetPurchaseDetailsByOrderNo(int orderNo)
        {
            return dao.GetPurchaseDetailsByOrderNo(orderNo);
            
        }

        public bool AddPurchaseDetail(string deliveryOrderNo, string itemCode, int qty, string remarks, decimal price)
        {
            return dao.AddPurchaseDetail(deliveryOrderNo, itemCode, qty, remarks, price);

        }

        public bool AddPurchaseDetail(Purchase_Detail pd)
        {
            return dao.AddPurchaseDetail(pd);
        }

        public List<Purchase_Order_Record> FindByStatus(string status)
        {
            return dao.FindByStatus(status);

        }
        public List<Purchase_Order_Record> FindBySupplier(string supplier)
        {
            return dao.FindBySupplier(supplier);
        }

        public int UpdatePurchaseDetailsInfo(Purchase_Detail pd)
        {
            return dao.UpdatePurchaseDetailsInfo(pd);
        }
    }
}