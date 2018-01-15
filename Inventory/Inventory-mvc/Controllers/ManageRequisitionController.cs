using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;

namespace Inventory_mvc.Controllers
{
    public class ManageRequisitionController : Controller
    {
        // GET: RequisitionRecord
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListRequisition()
        {
            RequisitionRecordService rs = new RequisitionRecordService();
            List<Requisition_Record> model = rs.GetAllRequisition();
            return View(model);
        }

        public ActionResult EditRequisition(int id)
        {
            RequisitionRecordService rs = new RequisitionRecordService();
            Requisition_Record model = new Requisition_Record();
            model = rs.GetRequisitionByID(Convert.ToInt32(id));
            return View(model);
        }
    }
}