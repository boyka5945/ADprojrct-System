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

        public List<Purchase_Details> GetPurchaseDetailsByOrderNo(int orderNo)
        {
            return dao.GetPurchaseDetailsByOrderNo(orderNo);
            
        }





        //public void AddPurchaseDetail(int deliveryOrderNo, string itemCode, int qty, string remarks)
        //{
        //    dao.AddPurchaseDetail(deliveryOrderNo, itemCode, qty, remarks);


        //}
    }
}