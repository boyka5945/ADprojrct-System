using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using Inventory_mvc.Service;
using PagedList;
using Inventory_mvc.Function;
using Inventory_mvc.Utilities;

namespace Inventory_mvc.Controllers
{
    public class RaiseRequisitionController : Controller
    {
        IStationeryService stationeryService = new StationeryService();
        IRequisitionRecordService requisitionService = new RequisitionRecordService();
        IUserService userService = new UserService();

        [RoleAuthorize]
        // CK - Employee | User Representative | Store Clerk | Store Supervisor
        public ActionResult BrowseCatalogue(string searchString, int? page, string categoryID = "-1")
        {
            List<Stationery> stationeries = stationeryService.GetStationeriesBasedOnCriteria(searchString, categoryID);

            ViewBag.CategoryList = stationeryService.GetAllCategory();
            ViewBag.CategoryID = (categoryID == "-1") ? "-1" : categoryID;

            ViewBag.SearchString = searchString;
            ViewBag.Page = page;

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(stationeries.ToPagedList(pageNumber, pageSize));
        }

        [RoleAuthorize]
        // CK - Employee | User Representative | Store Clerk | Store Supervisor
        public ActionResult ResetCatalogue()
        {
            return RedirectToAction("BrowseCatalogue", new { searchString = "", categoryID = "-1" });
        }

        [RoleAuthorize]
        // CK - Employee | User Representative | Store Clerk | Store Supervisor
        public ActionResult NewRequisition(string type, string itemCode = null)
        {
            List<RaiseRequisitionViewModel> requestList = Session["RequestList"] as List<RaiseRequisitionViewModel>;

            if(requestList == null)
            {
                // to fix weird situation where when session start the list didnt initialize
                requestList = new List<RaiseRequisitionViewModel>();
                Session["RequestList"] = requestList;
            }

            if (!String.IsNullOrEmpty(itemCode) && type == "remove") // to show message after remove item
            {
                Stationery s = stationeryService.FindStationeryByItemCode(itemCode);
                TempData["SuccessMessage"] = String.Format("{0} was removed.", s.description);
            }

            return View(requestList);
        }

        [RoleAuthorize]
        // CK - Employee | User Representative | Store Clerk | Store Supervisor
        [HttpPost]
        public void SaveTemporaryValue(List<RaiseRequisitionViewModel> requestList)
        {
            // to reserve edited quantity value when press Add Item button
            if(requestList != null)
            {
                Session["RequestList"] = requestList;
            }
        }

        [RoleAuthorize]
        // CK - Employee | User Representative | Store Clerk | Store Supervisor
        [HttpPost]
        public ActionResult AddNewRequestItem(string itemCode, int quantity, string searchString, int? page, string categoryID)
        {
            List<RaiseRequisitionViewModel> requestList = Session["RequestList"] as List<RaiseRequisitionViewModel>;

            if (requestList == null)
            {
                // to fix weird situation where when session start the list didnt initialize
                requestList = new List<RaiseRequisitionViewModel>();
                Session["RequestList"] = requestList;
            }


            // Server side validation
            if (quantity < 1)
            {
                string addItemErrorMessage = String.Format("Quantity must be greater than or equal to 1.");
                TempData["ErrorMessage"] = addItemErrorMessage;
            }
            else
            {
                Stationery stationery = stationeryService.FindStationeryByItemCode(itemCode);

                RaiseRequisitionViewModel vm = new RaiseRequisitionViewModel();
                vm.Description = stationery.description;
                vm.ItemCode = stationery.itemCode;
                vm.Quantity = quantity;
                vm.UOM = stationery.unitOfMeasure;

                bool contain = false;
                foreach (var request in requestList)
                {
                    if (request.ItemCode == vm.ItemCode)
                    {
                        request.Quantity += vm.Quantity;
                        contain = true;
                        break;
                    }
                }
                if (!contain)
                {
                    requestList.Add(vm);
                }
                Session["RequestList"] = requestList;

                string addItemMessage = String.Format("{0} x {1} was added into requisition form.", vm.Quantity, vm.Description);
                TempData["SuccessMessage"] = addItemMessage;
            }

            ViewBag.SearchString = searchString;
            ViewBag.CategoryID = categoryID;
            ViewBag.Page = page;

            string path = Request.UrlReferrer.ToString();
            if(path.Contains("NewRequisition"))
            {
                return RedirectToAction("NewRequisition");
            }
            else
            {
                // to return to same search page after go back to catalogue
                return RedirectToAction("BrowseCatalogue",
                        new { searchString = searchString, categoryID = categoryID, page = page });
            }
        }

        [RoleAuthorize]
        // CK - Employee | User Representative | Store Clerk | Store Supervisor
        [HttpPost]
        public ActionResult RemoveRequestItem(string itemCode, List<RaiseRequisitionViewModel> requestList)
        {
            RaiseRequisitionViewModel vm = requestList.Find(x => x.ItemCode == itemCode);
            string itemDescription = vm.Description;
            requestList.Remove(vm);
            Session["RequestList"] = requestList;

            TempData["SuccessMessage"] = String.Format("{0} was removed.", itemDescription);

            return RedirectToAction("NewRequisition");
        }

        [RoleAuthorize]
        // CK - Employee | User Representative | Store Clerk | Store Supervisor
        [HttpPost]
        public ActionResult SubmitRequisition(List<RaiseRequisitionViewModel> requestList)
        {
            string requesterID = HttpContext.User.Identity.Name;

            Requisition_Record requisition = new Requisition_Record();
            
            foreach(RaiseRequisitionViewModel request in requestList)
            {
                Requisition_Detail rd = new Requisition_Detail();
                rd.itemCode = request.ItemCode;
                rd.qty = request.Quantity;
                rd.allocatedQty = 0;
                rd.fulfilledQty = 0;

                requisition.Requisition_Detail.Add(rd);
            }

            if(!requisitionService.IsUserValidToSubmitRequisition(requesterID))
            {
                TempData["ErrorMessage"] = "Sorry, you are not allowed to submit stationery requisition.";
                return RedirectToAction("NewRequisition");
            }

            if (requisitionService.ValidateRequisition(requisition))
            {
                // Valid request, submit to database
                try
                {
                    requisitionService.SubmitNewRequisition(requisition, requesterID);

                    // clear requestlist
                    Session["RequestList"] = new List<RaiseRequisitionViewModel>();
                    TempData["SuccessMessage"] = "New stationery requisition has been submitted.";

                    // go to user requisition list
                    return RedirectToAction("Index", "ListRequisitions");
                }
                catch (EmailException e)
                {
                    // submit requisition successfully but email throw exception

                    // clear requestlist
                    Session["RequestList"] = new List<RaiseRequisitionViewModel>();
                    TempData["SuccessMessage"] = "New stationery requisition has been submitted.";
                    TempData["WarningMessage"] = "Failure to send email notification. Kindly contact IT personnel.";

                    // go to user requisition list
                    return RedirectToAction("Index", "ListRequisitions");
                }
                catch (Exception e1)
                {
                    // error
                    Session["RequestList"] = requestList;
                    TempData["ErrorMessage"] = e1.Message;
                }
            }
            else
            {
                // Invalid request
                Session["RequestList"] = requestList;
                TempData["ErrorMessage"] = "Invalid request";
            }

            //return to new requisition form if submit unsuccessful
            return RedirectToAction("NewRequisition");
        }

        [RoleAuthorize]
        // CK - Employee | User Representative | Store Clerk | Store Supervisor
        public ActionResult ClearAllRequestItem()
        {
            List<RaiseRequisitionViewModel> requestList = Session["RequestList"] as List<RaiseRequisitionViewModel>;
            requestList.Clear();
            Session["RequestList"] = requestList;

            TempData["SuccessMessage"] = String.Format("All items were removed.");

            return RedirectToAction("NewRequisition");
        }

        [RoleAuthorize]
        // CK - Employee | User Representative | Store Clerk | Store Supervisor
        public ActionResult GetStationeryListJSON(string term = null)
        {
            List<JSONForCombobox> options = new List<JSONForCombobox>();

            List<Stationery> stationeries = stationeryService.GetStationeriesBasedOnCriteria(term);
            foreach (var s in stationeries)
            {
                JSONForCombobox option = new JSONForCombobox();
                option.id = s.itemCode;
                option.text = String.Format("{0} ({1})", s.description, s.unitOfMeasure);
                options.Add(option);
            }
            return Json(options, JsonRequestBehavior.AllowGet);
        }


    }
}