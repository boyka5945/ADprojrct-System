using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;


namespace InventoryWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService
    {
        public Boolean ValidateUser(string userid, string password)
        {
            return BusinessLogic.validateUser(userid, password);
        }


        public Boolean changePassWord(string userid, string currentpassword, string newpassword)
        {
            return BusinessLogic.changePassWord(userid, currentpassword, newpassword);
        }


        public List<string> getAllItemCode()
        {
            return BusinessLogic.getAllItemCode();

        }
        public List<RequisitionDetails> getRequisitionDetailsByItemCode(string itemCode)
        {

            List<RequisitionDetails> list = new List<RequisitionDetails>();
            var rr = BusinessLogic.getRequisitionDetailsByItemCode(itemCode);
            foreach (var item in rr)
            {
                RequisitionDetails r = new RequisitionDetails();
                r.RequisitonNo = item.requisitionNo;
                r.RetrievedDate = item.retrievedDate;
                r.ItemCode = item.itemCode;
                r.NextCollectionDate = item.nextCollectionDate;
                r.Qty = item.qty;
                r.ClerkID = item.clerkID;
                r.AllocateQty = (int)item.allocatedQty;
                r.FulfilledQty = (int)item.fulfilledQty;
                r.Remarks = item.remarks;
                list.Add(r);
            }
            return list;
        }

        public RequisitionDetails getRequisitionDetailsBy2Keys(string itemCode, string requisitionNO)
        {

            int No = Convert.ToInt32(requisitionNO);
            var rr = BusinessLogic.getRequisitionDetailsBy2Keys(itemCode, No);
            RequisitionDetails r = new RequisitionDetails();
            r.RequisitonNo = rr.requisitionNo;
            r.RetrievedDate = rr.retrievedDate;
            r.ItemCode = rr.itemCode;
            r.NextCollectionDate = rr.nextCollectionDate;
            r.Qty = rr.qty;
            r.ClerkID = rr.clerkID;
            r.AllocateQty = (int)rr.allocatedQty;
            r.FulfilledQty = (int)rr.fulfilledQty;
            r.Remarks = rr.remarks;

            return r;
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
