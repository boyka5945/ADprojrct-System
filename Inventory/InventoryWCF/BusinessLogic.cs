using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace InventoryWCF
{
    public class BusinessLogic
    {
        public static Boolean validateUser(string userID, string password)
        {
            StationeryModel entity = new StationeryModel();
            if (entity.Users.Where(x => x.userID == userID && x.password == password).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public WCFUser GetUser(string userID)
        {
            StationeryModel entity = new StationeryModel();
            User user = entity.Users.Where(x => x.userID == userID).First();
            WCFUser wcfUser = WCFModelConvertUtility.ConvertToWCFUser(user);

            return wcfUser ;
        }

        public static Boolean changePassWord(string userID, string CurrentPassword, string NewPassWord)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                if (entity.Users.Where(x => x.userID == userID && x.password == CurrentPassword).Count() > 0)
                {
                    User updateUser = entity.Users.Where(x => x.userID == userID && x.password == CurrentPassword).First();
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
            var groups = entity.Requisition_Detail.Select(x => new
            {
                itemcode = x.itemCode
            }).ToList();
            var newgroups = groups.Distinct().ToList();
            foreach (var group in newgroups)
            {
                ItemCodes.Add(group.itemcode);
            }
            return ItemCodes;

        }

        public static List<Requisition_Detail> getRequisitionDetailsByItemCode(string itemCode)
        {
            StationeryModel entity = new StationeryModel();
            return entity.Requisition_Detail.Where(x => x.itemCode == itemCode).ToList();
        }

        public static Requisition_Detail getRequisitionDetailsBy2Keys(string itemCode, int requisitionNo)
        {
            StationeryModel entity = new StationeryModel();
            return entity.Requisition_Detail.Where(x => x.itemCode == itemCode && x.requisitionNo == requisitionNo).First();
        }

        public static Boolean updateRequisitionDetails(int requisitionNo, string ItemCode, int allocateQty)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                if (entity.Requisition_Detail.Where(x => x.requisitionNo == requisitionNo && x.itemCode == ItemCode).Count() > 0)
                {
                    var model = entity.Requisition_Detail.Where(x => x.requisitionNo == requisitionNo && x.itemCode == ItemCode).First();
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

        public static void updateRequisition(int requisitionNo, string status, string approveStaffID)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                if (entity.Requisition_Records.Where(x => x.requisitionNo == requisitionNo).Count() > 0)
                {
                    var model = entity.Requisition_Records.Where(x => x.requisitionNo == requisitionNo).First();
                    model.status = status;

                    if (approveStaffID == "")
                    {
                        model.approvingStaffID = null;
                        model.approveDate = null;
                    }
                    else
                    {
                        model.approvingStaffID = approveStaffID;
                        model.approveDate = DateTime.Now;
                    }
                    
                    entity.SaveChanges();
                    
                }
                
            }
        }

        public static void updateCollectionPoint(string deptCode, int newcp)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                if (entity.Departments.Where(x => x.departmentCode == deptCode).Count() > 0)
                {
                    try
                    {
                        var model = entity.Departments.Where(x => x.departmentCode == deptCode).First();


                        model.collectionPointID = newcp;

                        entity.SaveChanges();
                    }
                    catch
                    {
                        throw new Exception("Database Error.");
                    }

                }

            }
        }


    }
}