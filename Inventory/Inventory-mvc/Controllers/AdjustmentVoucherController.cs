using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using Inventory_mvc.Function;
using Inventory_mvc.Utilities;
using PagedList;

namespace Inventory_mvc.Controllers
{
    public class AdjustmentVoucherController : Controller
    {
        IStationeryService stationeryService = new StationeryService();
        IUserService userService = new UserService();
        IAdjustmentVoucherService adjustmentVoucherService = new AdjustmentVoucherService();

        // GET: AdjustmentVoucherRecord
        public ActionResult Index(string status, int? page, string sortOrder)
        {
            // TODO: REMOVE HARD CODED APPROVER ID
            //string approverID = HttpContext.User.Identity.Name;
            string approverID = "S1016"; // supervisor
            //string approverID = "S1015"; // store manager


            // Set Filter criteria
            status = (String.IsNullOrEmpty(status)) ? AdjustmentVoucherStatus.PENDING : status;
            ViewBag.Status = status;

            List<SelectListItem> statusSelectList = new List<SelectListItem>();
            statusSelectList.Add(new SelectListItem { Text = AdjustmentVoucherStatus.PENDING, Value = AdjustmentVoucherStatus.PENDING });
            statusSelectList.Add(new SelectListItem { Text = AdjustmentVoucherStatus.APPROVED, Value = AdjustmentVoucherStatus.APPROVED });
            statusSelectList.Add(new SelectListItem { Text = AdjustmentVoucherStatus.REJECTED, Value = AdjustmentVoucherStatus.REJECTED });
            foreach (var s in statusSelectList)
            {
                if (s.Value == ViewBag.Status)
                {
                    s.Selected = true;
                }
            }
            ViewBag.SelectStatus = statusSelectList;

            if (String.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "number_desc"; // make default sorting by voucher number desc
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NumberSortParm = (sortOrder == "Voucher Number") ? "number_desc" : "Voucher Number";
            ViewBag.RequesterSortParm = (sortOrder == "Issued By") ? "issued_by_desc" : "Issued By";

            List<AdjustmentVoucherViewModel> vouchers = adjustmentVoucherService.GetVoucherRecordsByCriteria(approverID, status, sortOrder);

            int pageSize = 7;
            int pageNumber = (page ?? 1);
            return View(vouchers.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult NewVoucher(string type, string itemCode = null)
        {
            List<AdjustmentVoucherViewModel> vmList = Session["NewVoucher"] as List<AdjustmentVoucherViewModel>;

            if (vmList == null)
            {
                vmList = new List<AdjustmentVoucherViewModel>();
                Session["NewVoucher"] = vmList;
            }

            if (!String.IsNullOrEmpty(itemCode) && type == "remove") // use to display message if remove item from voucher
            {
                TempData["SuccessMessage"] = String.Format("{0} was removed.", itemCode);
            }

            return View(vmList);
        }

        [HttpPost]
        public ActionResult AddItemIntoVoucher(string itemCode, int quantity, string reason)
        {
            Stationery s = stationeryService.FindStationeryByItemCode(itemCode);

            if ((s.stockQty + quantity) < 0)
            {
                // cannot minus more than current stock
                TempData["ErrorMessage"] = String.Format("Negative adjustment quantity cannot greater than current stock.");
            }
            else
            {
                List<AdjustmentVoucherViewModel> vmList = Session["NewVoucher"] as List<AdjustmentVoucherViewModel>;

                AdjustmentVoucherViewModel vm = new AdjustmentVoucherViewModel();
                vm.ItemCode = itemCode;
                vm.Quantity = quantity;
                vm.Reason = reason;
                vm.StationeryDescription = s.description;
                vm.UOM = s.unitOfMeasure;
                vm.Price = s.price;

                bool contain = false;
                bool zeroQuantity = (quantity == 0) ? true : false;

                foreach (var item in vmList)
                {
                    if (item.ItemCode == vm.ItemCode)
                    {
                        item.Quantity += vm.Quantity;
                        contain = true;
                        zeroQuantity = (item.Quantity == 0) ? true : false; // notify user if quantity become 0
                        break;
                    }
                }
                if (!contain)
                {
                    vmList.Add(vm);
                }

                Session["NewVoucher"] = vmList;

                TempData["SuccessMessage"] = String.Format("{0} was added.", itemCode);
                TempData["WarningMessage"] = (zeroQuantity) ? String.Format("Warning! Quantity of {0} is 0, which will not be submitted.", itemCode) : null;
            }

            return RedirectToAction("NewVoucher");
        }

        [HttpPost]
        public void SaveTemporaryValue(List<AdjustmentVoucherViewModel> vmList)
        {
            // to reserve edited quantity value when press Add Item button
            if (vmList != null)
            {
                Session["NewVoucher"] = vmList;
            }
        }


        [HttpPost]
        public ActionResult RemoveVoucherItem(string itemCode, List<AdjustmentVoucherViewModel> vmList)
        {
            AdjustmentVoucherViewModel vm = vmList.Find(x => x.ItemCode == itemCode);
            vmList.Remove(vm);
            Session["NewVoucher"] = vmList;

            TempData["SuccessMessage"] = String.Format("{0} was removed.", itemCode);

            return RedirectToAction("NewVoucher");
        }

        [HttpPost]
        public ActionResult SubmitVoucher(List<AdjustmentVoucherViewModel> vmList)
        {
            // TODO: REMOVE HARD CODED REQUESTER ID
            //string requesterID = HttpContext.User.Identity.Name;

            string requesterID = "S1013";
            string errorMessage;

            if (adjustmentVoucherService.ValidateNewAdjustmentVoucher(vmList, out errorMessage))
            {
                // Valid voucher
                if (adjustmentVoucherService.SubmitNewAdjustmentVoucher(vmList, AdjustmentVoucherRemarks.RECONCILE, requesterID))
                {
                    // clear list
                    Session["NewVoucher"] = new List<AdjustmentVoucherViewModel>();
                    TempData["SuccessMessage"] = String.Format("Discrepancy report has been submitted for approval.");
                }
                else
                {
                    TempData["ErrorMessage"] = String.Format("Error writing to database");
                }
            }
            else
            {
                TempData["ErrorMessage"] = errorMessage;
            }

            return RedirectToAction("NewVoucher");
        }


        public ActionResult ClearAllItemInVoucher()
        {
            List<AdjustmentVoucherViewModel> vmList = Session["NewVoucher"] as List<AdjustmentVoucherViewModel>;
            vmList.Clear();
            Session["RequestList"] = vmList;

            TempData["SuccessMessage"] = String.Format("All items were removed.");

            return RedirectToAction("NewVoucher");
        }

        public ActionResult GetStationeryListJSON(string term = null)
        {
            List<StationeryJSONForCombobox> options = new List<StationeryJSONForCombobox>();

            List<Stationery> stationeries = stationeryService.GetStationeriesBasedOnCriteria(term);
            foreach (var s in stationeries)
            {
                StationeryJSONForCombobox option = new StationeryJSONForCombobox();
                option.id = s.itemCode;
                option.text = String.Format("{0} ({1})", s.itemCode, s.description);
                options.Add(option);
            }
            return Json(options, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ShowDetail(int? id)
        {
            int voucherNo = (id == null) ? -1 : (int)id;

            if (voucherNo == -1)
            {
                return RedirectToAction("Index");
            }


            // TODO: REMOVE HARD CODED APPROVER ID
            //string approverID = HttpContext.User.Identity.Name;
            string approverID = "S1016"; // supervisor
            //string approverID = "S1015"; // store manager

            string errorMessage;
            List<AdjustmentVoucherViewModel> vmList = adjustmentVoucherService.IsUserAuthorizedToViewVoucherDetail(voucherNo, approverID, out errorMessage);

            if (!String.IsNullOrEmpty(errorMessage))
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction("Index");
            }

            return View(vmList);
        }

        [HttpGet]
        public ActionResult MakeApproval(int? id)
        {
            int voucherNo = (id == null) ? -1 : (int)id;

            if (voucherNo == -1)
            {
                return RedirectToAction("Index");
            }

            // TODO: REMOVE HARD CODED APPROVER ID
            //string approverID = HttpContext.User.Identity.Name;
            string approverID = "S1016"; // supervisor
            //string approverID = "S1015"; // store manager

            string errorMessage;
            List<AdjustmentVoucherViewModel> vmList = adjustmentVoucherService.IsUserAuthorizedToViewVoucherDetail(voucherNo, approverID, out errorMessage);

            if (!String.IsNullOrEmpty(errorMessage))
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction("Index");
            }

            return View(vmList);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="submitButton"></param>
        /// <param name="remark">For email notifcation</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MakeApproval(int id, string submitButton, string remark)
        {
            string errorMessage = null;

            // TODO: REMOVE HARD CODED APPROVER ID
            //string approverID = HttpContext.User.Identity.Name;
            string approverID = "S1016"; // supervisor
            //string approverID = "S1015"; // store manager

            switch (submitButton)
            {
                case "Approve":
                    if(adjustmentVoucherService.ValidateAdjustmentVoucherBeforeApprove(id, out errorMessage))
                    {
                        // valid voucher
                        if (!adjustmentVoucherService.ApproveVoucherRecord(id, approverID, remark))
                        {
                            errorMessage = String.Format("Error occur while updating voucher detail. Please try again later.");
                        }
                    }
                    break;

                case "Reject":
                    if (!adjustmentVoucherService.RejectVoucherRecord(id, approverID, remark))
                    {
                        errorMessage = String.Format("Error occur while updating voucher detail. Please try again later");
                    }
                    break;

                default:
                    errorMessage = String.Format("Opps...some error occured. Please try again later.");
                    break;
            }

            if(!String.IsNullOrEmpty(errorMessage))
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction("MakeApproval", id);
            }
            else
            {
                string statusMessage = (submitButton == "Approve") ? "approved" : "rejected";
                TempData["SuccessMessage"] = String.Format("Voucher number {0} was {1}.", id, statusMessage);
            }

            return RedirectToAction("Index");
        }


    }
}