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

namespace Inventory_mvc.Controllers
{
    public class RaiseRequisitionController : Controller
    {
        IStationeryService stationeryService = new StationeryService();
        IRequisitionRecordService requisitionService = new RequisitionRecordService();
        IUserService userService = new UserService();

        // GET: RaiseRequisition/BrowseCatalogue
        public ActionResult BrowseCatalogue(string searchString, int? page, string categoryID = "All")
        {
            List<Stationery> stationeries = stationeryService.GetStationeriesBasedOnCriteria(searchString, categoryID);

            ViewBag.CategoryList = stationeryService.GetAllCategory();
            ViewBag.CategoryID = (categoryID == "All") ? "All" : categoryID;
            ViewBag.SearchString = searchString;
            ViewBag.Page = page;

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(stationeries.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ResetCatalogue()
        {
            return RedirectToAction("BrowseCatalogue", new { searchString = "", categoryID = "All" });
        }

        public ActionResult NewRequisition(string type, string itemCode = null)
        {
            List<RaiseRequisitionViewModel> requestList = Session["RequestList"] as List<RaiseRequisitionViewModel>;

            if(!String.IsNullOrEmpty(itemCode) && type == "remove") // to show message after remove item
            {
                Stationery s = stationeryService.FindStationeryByItemCode(itemCode);
                TempData["SuccessMessage"] = String.Format("{0} was removed.", s.description);
            }

            return View(requestList);
        }

        [HttpPost]
        public void SaveTemporaryValue(List<RaiseRequisitionViewModel> requestList)
        {
            // to reserve edited quantity value when press Add Item button
            if(requestList != null)
            {
                Session["RequestList"] = requestList;
            }
        }


        [HttpPost]
        public ActionResult AddNewRequestItem(string itemCode, int quantity, string searchString, int? page, string categoryID)
        {
            // Server side validation
            if(quantity < 1)
            {
                string addItemErrorMessage = String.Format("Quantity must be greater than or equal to 1.");
                TempData["ErrorMessage"] = addItemErrorMessage;
            }
            else
            {
                List<RaiseRequisitionViewModel> requestList = Session["RequestList"] as List<RaiseRequisitionViewModel>;

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
                return RedirectToAction("BrowseCatalogue",
                        new { searchString = searchString, categoryID = categoryID, page = page });
            }
        }

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


        [HttpPost]
        public ActionResult SubmitRequisition(List<RaiseRequisitionViewModel> requestList)
        {
            // TODO: REMOVE HARD CODED REQUESTER ID
            // string requesterID = HttpContext.User.Identity.Name;

            string requesterID = "S1013";

            string requesterName = userService.FindNameByID(requesterID);
            string deptCode = userService.FindDeptCodeByID(requesterID);
            string status = "Pending Approval";
            DateTime requestDate = DateTime.Today;

            Requisition_Record requisition = new Requisition_Record();
            requisition.requesterID = requesterID;
            requisition.deptCode = deptCode;
            requisition.status = status;
            requisition.requestDate = requestDate;
            
            foreach(RaiseRequisitionViewModel request in requestList)
            {
                Requisition_Detail rd = new Requisition_Detail();
                rd.itemCode = request.ItemCode;
                rd.qty = request.Quantity;

                requisition.Requisition_Detail.Add(rd);
            }


            if(requisitionService.ValidateRequisition(requisition))
            {
                // Valid request, submit to database
                if (requisitionService.SubmitNewRequisition(requisition))
                {
                    // clear requestlist
                    Session["RequestList"] = new List<RaiseRequisitionViewModel>();
                    TempData["SuccessMessage"] = "New stationery requisition has been submitted.";

                    // go to user requisition list
                    return RedirectToAction("Index", "ListRequisitions");
                }
                else
                {
                    // error when write to database
                    Session["RequestList"] = requestList;
                    TempData["ErrorMessage"] = "Error Writing to Database";
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

        public ActionResult ClearAllRequestItem()
        {
            List<RaiseRequisitionViewModel> requestList = Session["RequestList"] as List<RaiseRequisitionViewModel>;
            requestList.Clear();
            Session["RequestList"] = requestList;

            TempData["SuccessMessage"] = String.Format("All items were removed.");

            return RedirectToAction("NewRequisition");
        }


        public ActionResult GetStationeryListJSON(string term = null)
        {
            List<StationeryJSONForCombobox> options = new List<StationeryJSONForCombobox>();

            List<Stationery> stationeries = stationeryService.GetStationeriesBasedOnCriteria(term);
            foreach (var s in stationeries)
            {
                StationeryJSONForCombobox option = new StationeryJSONForCombobox();
                option.id = s.itemCode;
                option.text = String.Format("{0} ({1})", s.description, s.unitOfMeasure);
                options.Add(option);
            }
            return Json(options, JsonRequestBehavior.AllowGet);
        }


    }
}