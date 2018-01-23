using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace InventoryWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/ValidateUser/{userid}/{password}", ResponseFormat = WebMessageFormat.Json)]
        Boolean ValidateUser(string userid, string password);

        [OperationContract]
        [WebGet(UriTemplate = "/ChangePassWord/{userid}/{currentpassword}/{newpassword}", ResponseFormat = WebMessageFormat.Json)]
        Boolean changePassWord(string userid, string currentpassword, string newpassword);

        [OperationContract]
        [WebGet(UriTemplate = "/All", ResponseFormat = WebMessageFormat.Json)]
        List<string> getAllItemCode();

        [OperationContract]
        [WebGet(UriTemplate = "/getRequisitionByItemCode/{itemCode}", ResponseFormat = WebMessageFormat.Json)]
        List<RequisitionDetails> getRequisitionDetailsByItemCode(string itemCode);

        [OperationContract]
        [WebGet(UriTemplate = "/getRequisitionDetailsBy2Keys/{itemCode}/{requisitionNo}", ResponseFormat = WebMessageFormat.Json)]
        RequisitionDetails getRequisitionDetailsBy2Keys(string itemCode, string requisitionNO);

        [OperationContract]
        [WebGet(UriTemplate = "/GetAllStationeries", ResponseFormat = WebMessageFormat.Json)]
        List<Stationery> GetAllStationeries();

        [OperationContract]
        [WebGet(UriTemplate = "/GetStationery/{itemCode}", ResponseFormat = WebMessageFormat.Json)]
        Stationery GetStationery(string itemCode);

        //[OperationContract]
        //Boolean updateRequisitionDetails(int requisitionNo, string ItemCode, int allocateQty);

        //// TODO: Add your service operations here

        //[OperationContract]
        //List<RetrievalFrom> getRetrievalList();

        //[OperationContract]
        //List<Disbursement> getDisbursementList();
        ////the follwing is for employee
        //[OperationContract]
        //List<RequisitionRecord> getRequisitionListByUserID(string UserID);

        //[OperationContract]
        //List<RequisitionDetails> getrequisitionDetailsByNO(int requisitionNo);


    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class Disbursement
    {
        string stationeryDescription;
        int? qty;
        int? returnQty;

        [DataMember]
        public string StationeryDescription
        {
            get { return stationeryDescription; }
            set { stationeryDescription = value; }
        }
        [DataMember]
        public int? Qty
        {
            get { return qty; }
            set { qty = value; }
        }
        [DataMember]
        public int? ReturnQty
        {
            get { return returnQty; }
            set { returnQty = value; }
        }
    }

    [DataContract]
    public class RetrievalFrom
    {
        string description;
        int? qty;

        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [DataMember]
        public int? Qty
        {
            get { return qty; }
            set { qty = value; }
        }
    }

    [DataContract]
    public class Stationery
    {
        string itemcode;
        string description;
        string uom;
        string category;
        string location;

        [DataMember]
        public string ItemCode
        {
            get { return itemcode; }
            set { itemcode = value; }
        }


        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [DataMember]
        public string UOM
        {
            get { return uom; }
            set { uom = value; }
        }

        [DataMember]
        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        [DataMember]
        public string Location
        {
            get { return location; }
            set { location = value; }
        }
    }

    [DataContract]
    public class User
    {
        string userid;
        string password;
        string username;
        int role;

        [DataMember]
        public int Role
        {
            get { return role; }
            set { role = value; }
        }

        [DataMember]
        public string UserID
        {
            get { return userid; }
            set { userid = value; }
        }

        [DataMember]
        public string PassWord
        {
            get { return password; }
            set { password = value; }
        }

        [DataMember]
        public string UserName
        {
            get { return username; }
            set { username = value; }
        }
    }

    [DataContract]
    public class RequisitionRecord
    {
        int requisitionNo;
        string deptCode;
        string requesterID;
        string approvingStaffID;
        DateTime? approveDate;
        string status;
        DateTime? requestDate;

        [DataMember]
        public int RequisitionNo
        {
            get { return requisitionNo; }
            set { requisitionNo = value; }
        }

        [DataMember]
        public string DeptCode
        {
            get { return deptCode; }
            set { deptCode = value; }
        }


        [DataMember]
        public string RequesterID
        {
            get { return requesterID; }
            set { requesterID = value; }
        }

        [DataMember]
        public string ApprovingStaffID
        {
            get { return approvingStaffID; }
            set { approvingStaffID = value; }
        }

        [DataMember]
        public DateTime? ApproveDate
        {
            get { return approveDate; }
            set { approveDate = value; }
        }

        [DataMember]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        [DataMember]
        public DateTime? RequestDate
        {
            get { return requestDate; }
            set { requestDate = value; }
        }
    }

    [DataContract]
    public class RequisitionDetails
    {
        int requisitonNo;
        string itemCode;
        string description;
        string remarks;
        int qty;
        int fulfilledQty;
        string clerkID;
        DateTime? retrievedDate;
        int allocateQty;
        DateTime? nextCollectionDate;

        [DataMember]
        public int RequisitonNo
        {
            get { return requisitonNo; }
            set { requisitonNo = value; }
        }

        [DataMember]
        public string ItemCode
        {
            get { return itemCode; }
            set { itemCode = value; }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        [DataMember]
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        [DataMember]
        public int Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        [DataMember]
        public int FulfilledQty
        {
            get { return fulfilledQty; }
            set { fulfilledQty = value; }
        }

        [DataMember]
        public string ClerkID
        {
            get { return clerkID; }
            set { clerkID = value; }
        }

        [DataMember]
        public DateTime? RetrievedDate
        {
            get { return retrievedDate; }
            set { retrievedDate = value; }
        }

        [DataMember]
        public int AllocateQty
        {
            get { return allocateQty; }
            set { allocateQty = value; }
        }

        [DataMember]
        public DateTime? NextCollectionDate
        {
            get { return nextCollectionDate; }
            set { nextCollectionDate = value; }
        }
    }
}
