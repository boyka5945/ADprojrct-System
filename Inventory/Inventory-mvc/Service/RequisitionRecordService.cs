using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.DAO;

namespace Inventory_mvc.Service
{
    public class RequisitionRecordService : IRequisitionRecordService
    {
        public List<Requisition_Record> GetAllRequisition()
        {
            RequisitionRecordDAO rDAO = new RequisitionRecordDAO();
            return rDAO.GetAllRequisition();
        }

        public Requisition_Record GetRequisitionByID(int id)
        {
            RequisitionRecordDAO rDAO = new RequisitionRecordDAO();
            return rDAO.FindByRequisitionNo(id);
        }

        public List<Requisition_Details> GetDetailsByNo(int No=0)
        {
            RequisitionRecordDAO rDAO = new RequisitionRecordDAO();
            return rDAO.GetDetailsByNO(No);  
        }

        public void  UpdateRequisition(Requisition_Record rr, string status) 
        {
            RequisitionRecordDAO rDAO = new RequisitionRecordDAO();
            rDAO.UpdateRequisition(rr, status);

        }

        public List<string> GetItemCodeList()
        {
            RequisitionRecordDAO rDAO = new RequisitionRecordDAO();
            return rDAO.GetDetailsByGroup();
        }

        public List<Requisition_Record> GetRecordByItemCode(string itemCode)
        {
            RequisitionRecordDAO rDAO = new RequisitionRecordDAO();
            return rDAO.GetRecordByItemCode(itemCode);
        }

        public int FindUnfulfilledQtyBy2Key(string itemCode, int requisitionNo) 
        {
            RequisitionRecordDAO rDAO = new RequisitionRecordDAO();
            return rDAO.FindUnfulfilledQtyBy2Key(itemCode, requisitionNo);
        }

        public Requisition_Details FindDetailsBy2Key(string itemCode, int requisitionNo)
        {
            RequisitionRecordDAO rDAO = new RequisitionRecordDAO();
            return rDAO.FindDetailsBy2Key(itemCode, requisitionNo);
        }

        public void UpdateDetails(string itemcode, int requisitionNo, int allocateQty)
        {
            RequisitionRecordDAO rDAO = new RequisitionRecordDAO();
            rDAO.UpdateRequisitionDetails(itemcode, requisitionNo, allocateQty);

        }
    }
}