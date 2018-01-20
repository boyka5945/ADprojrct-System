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
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public Boolean validateUser(User user)
        {
            return true;
        }


        public Boolean changePassWord(User user) { return true; }


        public List<string> getAllItemCode() { return null; }

      
        public List<RequisitionRecord> getRequisitionByItemCode(string itemCode) { return null; }

        public RequisitionDetails getRequisitionDetailsBy2Keys(string itemCode, int requisitionNO) { return null; }

        public Boolean updateRequisitionDetails(RequisitionDetails rd) { return true; }
        // TODO: Add your service operations here
        public List<RetrievalFrom> getRetrievalList() { return null; }

        public List<Disbursement> getDisbursementList() { return null; }

        public bool updateRequisitionRecord(RequisitionRecord rr)
        {
            throw new NotImplementedException();
        }

        public List<RequisitionRecord> getRequisitionListByUserID(string UserID)
        {
            throw new NotImplementedException();
        }

        public List<RequisitionDetails> getrequisitionDetailsByNO(int requisitionNo)
        {
            throw new NotImplementedException();
        }
    }
}
