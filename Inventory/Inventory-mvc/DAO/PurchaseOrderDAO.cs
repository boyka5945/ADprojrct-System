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
            using (StationeryModel Entity = new StationeryModel())
            {
                Entity.Purchase_Order_Record.Add(purchase_order_record);
                Entity.SaveChanges();
                return true;

            }


        }

        public bool DeletePurchaseOrder(int orderID)
        {
            using (StationeryModel Entity = new StationeryModel())
            {

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

        public int UpdatePurchaseOrderInfo(Purchase_Order_Record record)
        {
            using (StationeryModel Entity = new StationeryModel())
            {
                Purchase_Order_Record por = (from x in Entity.Purchase_Order_Record
                                             where x.orderNo == record.orderNo
                                             select x).FirstOrDefault();

                por.status = record.status;
                por.expectedDeliveryDate = record.expectedDeliveryDate;
                por.clerkID = record.clerkID;
                por.date = record.date;


                int rowAffected = Entity.SaveChanges();

                return rowAffected;

            }
        }



        //PURCHASE DETAILS//
        public List<Purchase_Details> GetPurchaseDetailsByOrderNo(int orderNo)
        {
            StationeryModel Entity = new StationeryModel();

                List<Purchase_Details> pd = Entity.Purchase_Details.Where(x => x.orderNo == orderNo).ToList();
                return pd;
            

        }



        public bool AddPurchaseDetail(Purchase_Details pd)
        {
            using(StationeryModel Entity = new StationeryModel())
            {
                Entity.Purchase_Details.Add(pd);
                Entity.SaveChanges();
                return true;
            }
        }

        public bool AddPurchaseDetail(int deliveryOrderNo, string itemCode, int qty, string remarks, decimal price)
        {
            using (StationeryModel Entity = new StationeryModel())
            {
                int maxOrderNo = 0;
                //to obtain highest order number
                List<Purchase_Order_Record> pds = Entity.Purchase_Order_Record.ToList();
                foreach (Purchase_Order_Record p in pds)
                {
                    maxOrderNo = 1;
                    if (p.orderNo > maxOrderNo)
                    {
                        maxOrderNo = p.orderNo;
                    }

                }

                Purchase_Details pd = new Purchase_Details();
                pd.orderNo = maxOrderNo + 1;
                pd.deliveryOrderNo = deliveryOrderNo;
                pd.itemCode = itemCode;
                pd.qty = qty;
                pd.remarks = remarks;
                pd.price = price;
               
                Entity.Purchase_Details.Add(pd);
                Entity.SaveChanges();
                return true;
            }




        }


    }


}