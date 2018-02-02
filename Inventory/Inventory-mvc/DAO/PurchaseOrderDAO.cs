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
            StationeryModel Entity = new StationeryModel();
            
                Entity.Purchase_Order_Records.Add(purchase_order_record);
                Entity.SaveChanges();
                return true;

            


        }

        public bool DeletePurchaseOrder(int orderID)
        {
            using (StationeryModel Entity = new StationeryModel())
            {

                Purchase_Order_Record por = Entity.Purchase_Order_Records.Where(x => x.orderNo == orderID).First();
                Entity.Purchase_Order_Records.Remove(por);
                Entity.SaveChanges();
                return true;
            }

        }

        public Purchase_Order_Record FindByOrderID(int orderID)
        {
            StationeryModel Entity = new StationeryModel();
            
                Purchase_Order_Record por = Entity.Purchase_Order_Records.Where(x => x.orderNo == orderID).First();
                return por;


            

        }

        public List<Purchase_Order_Record> FindByStatus(string status)
        {
            using (StationeryModel Entity = new StationeryModel())
            {
                List<Purchase_Order_Record> por = Entity.Purchase_Order_Records.Where(x => x.status == status).ToList();
                return por;

            }

        }

        public List<Purchase_Order_Record> FindBySupplier(string supplier)
        {
            using (StationeryModel Entity = new StationeryModel())
            {
                List<Supplier> s = Entity.Suppliers.Where(x => x.supplierName == supplier).ToList();

                List<Purchase_Order_Record> por = Entity.Purchase_Order_Records.Where(x => x.supplierCode == supplier).ToList();
                return por;

            }

        }

        public List<Purchase_Order_Record> GetAllPurchaseOrder()
        {
            using (StationeryModel Entity = new StationeryModel())
            {
                List<Purchase_Order_Record> por = Entity.Purchase_Order_Records.ToList();
                return por;

            }

        }

        public int UpdatePurchaseOrderInfo(Purchase_Order_Record record)
        {
            using (StationeryModel Entity = new StationeryModel())
            {
                Purchase_Order_Record por = (from x in Entity.Purchase_Order_Records
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

        public int UpdatePurchaseDetailsInfo(Purchase_Detail pd)
        {
            using (StationeryModel Entity = new StationeryModel())
            {
                Purchase_Detail purchd = (from x in Entity.Purchase_Detail
                                             where x.orderNo == pd.orderNo && x.itemCode == pd.itemCode
                                             select x).FirstOrDefault();

                purchd.fulfilledQty = pd.fulfilledQty;
                purchd.price = pd.price;
                purchd.deliveryOrderNo += " /" +pd.deliveryOrderNo;
                purchd.remarks = pd.remarks;

                int rowAffected = Entity.SaveChanges();

                return rowAffected;

            }

        }



        //PURCHASE DETAILS//
        public List<Purchase_Detail> GetPurchaseDetailsByOrderNo(int orderNo)
        {
            StationeryModel Entity = new StationeryModel();

                List<Purchase_Detail> pd = Entity.Purchase_Detail.Where(x => x.orderNo == orderNo).ToList();
                return pd;
            

        }



        public bool AddPurchaseDetail(Purchase_Detail pd)
        {
            using(StationeryModel Entity = new StationeryModel())
            {
                Entity.Purchase_Detail.Add(pd);
                Entity.SaveChanges();
                return true;
            }
        }

        public bool AddPurchaseDetail(string deliveryOrderNo, string itemCode, int qty, string remarks, decimal price)
        {
            using (StationeryModel Entity = new StationeryModel())
            {
                int maxOrderNo = 0;
                //to obtain highest order number
                List<Purchase_Order_Record> pds = Entity.Purchase_Order_Records.ToList();
                foreach (Purchase_Order_Record p in pds)
                {
                    maxOrderNo = 1;
                    if (p.orderNo > maxOrderNo)
                    {
                        maxOrderNo = p.orderNo;
                    }

                }

                Purchase_Detail pd = new Purchase_Detail();
                pd.orderNo = maxOrderNo + 1;
                pd.deliveryOrderNo = deliveryOrderNo;
                pd.itemCode = itemCode;
                pd.qty = qty;
                pd.remarks = remarks;
                pd.price = price;
               
                Entity.Purchase_Detail.Add(pd);
                Entity.SaveChanges();
                return true;
            }




        }


    }


}