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
    }
}