using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public class PurchaseOrderDAO : IPurchaseOrderDAO
    {
        public bool AddNewPurchaseOrder(Purchase_Order_Record purchase_order_record)
        {
            using(StationeryModel Entity = new StationeryModel())
            {
                Entity.Purchase_Order_Record.Add(purchase_order_record);
                Entity.SaveChanges();
                return true;

            }

            
        }

        public bool DeletePurchaseOrder(int orderID)
        {
            using (StationeryModel Entity = new StationeryModel()) {

                Purchase_Order_Record por = Entity.Purchase_Order_Record.Where(x => x.orderNo == orderID).First();
                Entity.Purchase_Order_Record.Remove(por);
                Entity.SaveChanges();
                return true;
                    }

        }

        public Purchase_Order_Record FindByOrderID(int orderID)
        {
            using (StationeryModel Entity = new StationeryModel())
            {
                Purchase_Order_Record por = Entity.Purchase_Order_Record.Where(x => x.orderNo == orderID).First();
                return por;


            }

        }

        public List<Purchase_Order_Record> GetAllPurchaseOrder()
        {
            using (StationeryModel Entity = new StationeryModel())
            {
                List<Purchase_Order_Record> por = Entity.Purchase_Order_Record.ToList();
                return por;

            }

        }

        public int UpdatePurchaseOrderInfo(Purchase_Order_Record purchase_order_record)
        {

           
        }
    }
}