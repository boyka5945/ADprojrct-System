using Inventory_mvc.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using PagedList;


namespace Inventory_mvc.Controllers
{
    public class ListRequisitionsController : Controller
    {
        IStationeryService stationeryService = new StationeryService();
        IRequisitionRecordService requisitionService = new RequisitionRecordService();
        IUserService userService = new UserService();

        // GET: ListRequisitions
        public ActionResult Index(int? page)
        {
            // Retrieve all requisitions made by current user

            // TODO: REMOVE HARD CODED REQUESTER ID
            //string requesterID = HttpContext.User.Identity.Name;

            string requesterID = "S1013";
            List<Requisition_Record> records = requisitionService.GetRecordsByRequesterID(requesterID);

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(records.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult RemoveRecord(int? id)
        {
            int recordNo = (id == null) ? -1 : (int)id;

            if (recordNo == -1)
            {
                return RedirectToAction("Index");
            }

            // TODO: REMOVE HARD CODED REQUESTER ID
            //string requesterID = HttpContext.User.Identity.Name;

            string requesterID = "S1013";
            string errorMessage;
            Requisition_Record record = requisitionService.IsUserAuthorizedForRequisition(recordNo, requesterID, out errorMessage);

            if (!String.IsNullOrEmpty(errorMessage))
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction("Index");
            }


            if (record.status != "Pending Approval")
            {
                TempData["ErrorMessage"] = String.Format("Cannot remove requisition no. {0}", record.requisitionNo);
            }
            else
            {
                if (requisitionService.DeleteRequisition(recordNo))
                {
                    TempData["RemoveMessage"] = String.Format("Requisition no. {0} was removed.", record.requisitionNo);
                }
                else
                {
                    TempData["ErrorMessage"] = String.Format("Error Writing to Database");
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditRecord(int? id)
        {
            int recordNo = (id == null) ? -1 : (int)id;

            if (recordNo == -1)
            {
                return RedirectToAction("Index");
            }

            // TODO: REMOVE HARD CODED REQUESTER ID
            //string requesterID = HttpContext.User.Identity.Name;

            string requesterID = "S1013";
            string errorMessage;
            Requisition_Record record = requisitionService.IsUserAuthorizedForRequisition(recordNo, requesterID, out errorMessage);

            if (!String.IsNullOrEmpty(errorMessage))
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction("Index");
            }

            if (record.status != "Pending Approval")
            {
                TempData["ErrorMessage"] = String.Format("Requisition has been approved and cannot be edited.");
                return RedirectToAction("Index");
            }


            List<RequisitionDetailViewModel> vmList = new List<RequisitionDetailViewModel>();
            foreach (var item in record.Requisition_Detail)
            {
                RequisitionDetailViewModel vm = new RequisitionDetailViewModel();

                vm.Item = item.Stationery;
                vm.RequestQty = (item.qty == null) ? 0 : (int)item.qty;
                vm.Record = record;
                vm.ReceivedQty = (item.fulfilledQty == null) ? 0 : (int)item.fulfilledQty;

                vmList.Add(vm);
            }

            string approvalStatus = (record.status == "Pending Approval") ? "Pending Approval" : "Approved";

            ViewBag.RequisitionFormNo = record.requisitionNo;
            ViewBag.ApprovalStatus = approvalStatus;

            return View(vmList);
        }


        [HttpPost]
        public ActionResult EditRecord(RequisitionDetailViewModel model)
        {
            Requisition_Detail requisitionDetail = requisitionService.FindDetailsBy2Key(model.Item.itemCode, model.Record.requisitionNo);

            if(model.RequestQty < 1)
            {
                TempData["ErrorMessage"] = String.Format("Quantity must be greater than or equal to 1.");
            }
            else
            {
                if (requisitionService.UpdateDetails(requisitionDetail))
                {
                    TempData["EditMessage"] = String.Format("Quantity of {0} was updated.", model.Item.description);
                }
                else
                {
                    TempData["ErrorMessage"] = String.Format("Error Writing to Database");
                }
            }

            return RedirectToAction("EditRecord", new { requisitionNo = model.Record.requisitionNo });
        }

        public ActionResult ShowDetail(int? id)
        {
            int recordNo = (id == null) ? -1 : (int) id;

            if(recordNo == -1)
            {
                return RedirectToAction("Index");
            }

            // TODO: REMOVE HARD CODED REQUESTER ID
            //string requesterID = HttpContext.User.Identity.Name;

            string requesterID = "S1013";
            string errorMessage;
            Requisition_Record record = requisitionService.IsUserAuthorizedForRequisition(recordNo, requesterID, out errorMessage);

            if (!String.IsNullOrEmpty(errorMessage))
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction("Index");
            }

            List<RequisitionDetailViewModel> vmList = new List<RequisitionDetailViewModel>();
            foreach (var item in record.Requisition_Detail)
            {
                RequisitionDetailViewModel vm = new RequisitionDetailViewModel();

                vm.Item = item.Stationery;
                vm.RequestQty = (item.qty == null) ? 0 : (int)item.qty;
                vm.Record = record;
                vm.ReceivedQty = (item.fulfilledQty == null) ? 0 : (int) item.fulfilledQty;

                vmList.Add(vm);
            }

            string approvalStatus = (record.status == "Pending Approval") ? "Pending Approval" : "Approved";

            ViewBag.RequisitionFormNo = record.requisitionNo;
            ViewBag.ApprovalStatus = approvalStatus;

            return View(vmList);
        }


        
    }
}