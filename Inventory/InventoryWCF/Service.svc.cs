using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Inventory_mvc.Service;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;

namespace InventoryWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService
    {
        IUserService userService = new UserService();
        IStationeryService stationeryService = new StationeryService();
        IRequisitionRecordService requisitionRecordService = new RequisitionRecordService();
        
               
        public Boolean ValidateUser(string userid, string password)
        {
            //return BusinessLogic.validateUser(userid, password);

            try
            {
                User user = userService.FindByUserID(userid);
                return (user.password == password); // wrong password
            }
            catch (Exception e)
            {
                // non existing userID
                return false;
            }           
        }


        public Boolean ChangePassword(string userid, string currentpassword, string newpassword)
        {
            // TODO : IMPLEMENT METHOD
            //return BusinessLogic.changePassWord(userid, currentpassword, newpassword);
            try
            {
                if(ValidateUser(userid, currentpassword))
                {
                    throw new NotImplementedException();
                }
                else
                {
                    // invalid user
                    return false;
                }
            }
            catch (Exception e)
            {
                // error
                return false;
            }

        }


        //public List<string> GetAllItemCode()
        //{
        //    
        //    //return BusinessLogic.getAllItemCode();
        //    try
        //    {
                
        //    }
        //    catch (Exception e)
        //    {

        //    }

        //}

        public List<WCFRequisitionDetail> GetRequisitionDetailsByItemCode(string itemCode)
        {
            // TODO : IMPLEMENT METHOD

            throw new NotImplementedException();

            //List<WCFRequisitionDetail> list = new List<WCFRequisitionDetail>();
            //var rr = BusinessLogic.getRequisitionDetailsByItemCode(itemCode);
            //foreach (var item in rr)
            //{
            //    WCFRequisitionDetail r = new WCFRequisitionDetail();
            //    r.RequisitonNo = item.requisitionNo;
            //    r.RetrievedDate = item.retrievedDate;
            //    r.ItemCode = item.itemCode;
            //    r.NextCollectionDate = item.nextCollectionDate;
            //    r.Qty = (int) item.qty;
            //    r.ClerkID = item.clerkID;
            //    r.AllocateQty = (int)item.allocatedQty;
            //    r.FulfilledQty = (int)item.fulfilledQty;
            //    r.Remarks = item.remarks;
            //    list.Add(r);
            //}
            //return list;
        }

        public WCFRequisitionDetail GetRequisitionDetailsBy2Keys(string itemCode, string requisitionNO)
        {
            try
            {
                Requisition_Detail detail = requisitionRecordService.FindDetailsBy2Key(itemCode, Convert.ToInt32(requisitionNO));
                return WCFModelConvertUtility.ConvertToWCFRequisitionDetail(detail);
            }
            catch (Exception e)
            {
                throw new Exception("Requisition detail not found");
            }
        }

        public List<WCFStationery> GetAllStationeries()
        {
            List<Stationery> stationeries = stationeryService.GetAllStationery();
            return WCFModelConvertUtility.ConvertToWCFStationery(stationeries);
        }

        public WCFStationery GetStationery(string itemCode)
        {
            try
            {
                Stationery s = stationeryService.FindStationeryByItemCode(itemCode);
                return WCFModelConvertUtility.ConvertToWCFStationery(s);
            }
            catch (Exception e)
            {
                throw new Exception("Stationery not found");
            }
        }

        //public Boolean updateRequisitionDetails(int requisitionNo, string ItemCode, int allocateQty)
        //{
        //    return BusinessLogic.updateRequisitionDetails(requisitionNo, ItemCode, allocateQty);
        //}

        //public List<RetrievalFrom> getRetrievalList()
        //{
        //    throw new NotImplementedException();
        //}

        //public List<Disbursement> getDisbursementList()
        //{
        //    throw new NotImplementedException();
        //}

        //public List<RequisitionRecord> getRequisitionListByUserID(string UserID)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<RequisitionDetails> getrequisitionDetailsByNO(int requisitionNo)
        //{
        //    throw new NotImplementedException();
        //}

    }
}
