using Inventory_mvc.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using Inventory_mvc.Utilities;
using PagedList;


namespace Inventory_mvc.Controllers
{
    public class ListRequisitionsController : Controller
    {
        IStationeryService stationeryService = new StationeryService();
        IRequisitionRecordService requisitionService = new RequisitionRecordService();
        IUserService userService = new UserService();

        // GET: ListRequisitions
        public ActionResult Index(int? page, string sortOrder)
        {
            // Retrieve all requisitions made by current user
            string requesterID = HttpContext.User.Identity.Name;

            if(String.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "date_desc"; // defaut sorting by date desc
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = (sortOrder == "Request Date") ? "date_desc" : "Request Date";
            ViewBag.NumberSortParm = (sortOrder == "Requisition Form Number") ? "number_desc" : "Requisition Form Number";
            ViewBag.StatusSortParm = (sortOrder == "Status") ? "status_desc" : "Status";

            List<Requisition_Record> records = requisitionService.GetSortedRecordsByRequesterID(requesterID, sortOrder);

            int pageSize = 7;
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

            string requesterID = HttpContext.User.Identity.Name;

            string errorMessage;
            Requisition_Record record = requisitionService.IsUserAuthorizedForRequisition(recordNo, requesterID, out errorMessage);

            if (!String.IsNullOrEmpty(errorMessage))
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction("Index");
            }


            if (record.status != RequisitionStatus.PENDING_APPROVAL)
            {
                TempData["ErrorMessage"] = String.Format("Cannot remove requisition no. {0}", record.requisitionNo);
            }
            else
            {
                if (requisitionService.DeleteRequisition(recordNo))
                {
                    TempData["SuccessMessage"] = String.Format("Requisition no. {0} was removed.", record.requisitionNo);
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

            string requesterID = HttpContext.User.Identity.Name;

            string errorMessage;
            Requisition_Record record = requisitionService.IsUserAuthorizedForRequisition(recordNo, requesterID, out errorMessage);

            if (!String.IsNullOrEmpty(errorMessage))
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction("Index");
            }

            if (record.status != RequisitionStatus.PENDING_APPROVAL)
            {
                TempData["ErrorMessage"] = String.Format("Requisition has been approved and cannot be edited.");
                return RedirectToAction("Index");
            }


            List<RequisitionDetailViewModel> vmList = requisitionService.GetViewModelFromRequisitionRecord(record);

            // Set string for display
            string approvalStatus = (record.status == RequisitionStatus.PENDING_APPROVAL) ? RequisitionStatus.PENDING_APPROVAL : "Approved";

            ViewBag.RequisitionFormNo = record.requisitionNo;
            ViewBag.ApprovalStatus = approvalStatus;

            return View(vmList);
        }

        [HttpPost]
        public ActionResult EditRecord(List<RequisitionDetailViewModel> vmList)
        {
            foreach(var vm in vmList)
            {
                if (vm.RequestQty < 1)
                {
                    TempData["ErrorMessage"] = String.Format("Quantity of {0} must be greater than or equal to 1", vm.Description);
                    return RedirectToAction("EditRecord", vmList);
                }
            }

            string errorMessage;
            
            if (requisitionService.UpdateRequisitionDetails(vmList, out errorMessage))
            {
                TempData["SuccessMessage"] = String.Format("Requisition form no. {0} has been updated.", vmList.First().RequisitionNo);
                return RedirectToAction("ShowDetail", new { id = vmList.First().RequisitionNo });
            }
            else
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction("EditRecord", vmList);
            }
        }

        //[HttpPost]
        //public ActionResult EditRecord(int id, string itemCode, string description, int quantity)
        //{
        //    Requisition_Detail requisitionDetail = requisitionService.FindDetailsBy2Key(itemCode, id);

        //    if(quantity < 1)
        //    {
        //        TempData["ErrorMessage"] = String.Format("Quantity must be greater than or equal to 1.");
        //    }
        //    else
        //    {
        //        requisitionDetail.qty = quantity;

        //        if (requisitionService.UpdateDetails(requisitionDetail))
        //        {
        //            TempData["SuccessMessage"] = String.Format("Quantity of {0} was updated.", description);
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = String.Format("Error Writing to Database");
        //        }
        //    }

        //    return RedirectToAction("EditRecord", new { id = id });
        //}

        public ActionResult ShowDetail(int? id)
        {
            int recordNo = (id == null) ? -1 : (int) id;

            if(recordNo == -1)
            {
                return RedirectToAction("Index");
            }

            string requesterID = HttpContext.User.Identity.Name;

            string errorMessage;
            Requisition_Record record = requisitionService.IsUserAuthorizedForRequisition(recordNo, requesterID, out errorMessage);

            if (!String.IsNullOrEmpty(errorMessage))
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction("Index");
            }

            List<RequisitionDetailViewModel> vmList = requisitionService.GetViewModelFromRequisitionRecord(record);

            // set text for display
            string approvalStatus = (record.status == RequisitionStatus.PENDING_APPROVAL) ? RequisitionStatus.PENDING_APPROVAL : "Approved";

            ViewBag.RequisitionFormNo = record.requisitionNo;
            ViewBag.ApprovalStatus = approvalStatus;

            return View(vmList);
        }


        
    }
}