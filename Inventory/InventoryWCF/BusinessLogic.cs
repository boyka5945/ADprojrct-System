using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventoryWCF.Model;

namespace InventoryWCF
{
    public class BusinessLogic
    {
       
        public static Boolean validateUser(string userID, string password)
        {
            StationeryModel entity = new StationeryModel();
            if (entity.User.Where(x => x.userID == userID && x.password == password).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean changePassWord(string userID, string CurrentPassword, string NewPassWord)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                if (entity.User.Where(x => x.userID == userID && x.password == CurrentPassword).Count() > 0)
                {
                    InventoryWCF.Model.User updateUser = entity.User.Where(x => x.userID == userID && x.password == CurrentPassword).First();
                    updateUser.password = NewPassWord;
                    entity.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static List<string> getAllItemCode()
        {
            StationeryModel entity = new StationeryModel();
            List<string> ItemCodes = new List<string>();
            var groups = entity.Requisition_Details.Select(x => new
            {
                itemcode = x.itemCode
            }).ToList();

            foreach (var group in groups)
            {
                ItemCodes.Add(group.itemcode);
            }
            return ItemCodes;

        }

        public static List<Requisition_Details> getRequisitionDetailsByItemCode(string itemCode)
        {
            StationeryModel entity = new StationeryModel();
            return entity.Requisition_Details.Where(x => x.itemCode == itemCode).ToList();
        }

        public static Requisition_Details getRequisitionDetailsBy2Keys(string itemCode, int requisitionNo)
        {
            StationeryModel entity = new StationeryModel();
            return entity.Requisition_Details.Where(x => x.itemCode == itemCode && x.requisitionNo == requisitionNo).First();
        }

        public static Boolean updateRequisitionDetails(int requisitionNo, string ItemCode, int allocateQty)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                if (entity.Requisition_Details.Where(x => x.requisitionNo == requisitionNo && x.itemCode == ItemCode).Count() > 0)
                {
                    var model = entity.Requisition_Details.Where(x => x.requisitionNo == requisitionNo && x.itemCode == ItemCode).First();
                    model.allocatedQty = allocateQty;
                    model.Stationery.stockQty = model.Stationery.stockQty - allocateQty;
                    entity.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }



    }
}