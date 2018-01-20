using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using Inventory_mvc.Function;

namespace Inventory_mvc.Controllers
{
    public class AdjustmentVoucherController : Controller
    {
        IStationeryService stationeryService = new StationeryService();
        IUserService userService = new UserService();
        IAdjustmentVoucherService adjustmentVoucherService = new AdjustmentVoucherService();

        // GET: AdjustmentVoucherRecord
        public ActionResult Index(string status)
        {
            //List<NewVoucherViewModel> pendingApprovalList = adjustmentVoucherService.FindVoucherPendingApproval(string userID);
            ViewBag.Status = (String.IsNullOrEmpty(status)) ? "Pending" : status;

            List<SelectListItem> statusSelectList = new List<SelectListItem>();
            statusSelectList.Add(new SelectListItem { Text = "Pending", Value = "Pending"});
            statusSelectList.Add(new SelectListItem { Text = "Approved", Value = "Approved"});
            statusSelectList.Add(new SelectListItem { Text = "Rejected", Value = "Rejected" });
            foreach(var s in statusSelectList)
            {
                if(s.Value == ViewBag.Status)
                {
                    s.Selected = true;
                }
            }
            ViewBag.SelectStatus = statusSelectList;

            return View();
        }

        public ActionResult NewVoucher(string type, string itemCode = null)
        {
            List<NewVoucherViewModel> vmList = Session["NewVoucher"] as List<NewVoucherViewModel>;

            if (vmList == null)
            {
                vmList = new List<NewVoucherViewModel>();
                Session["NewVoucher"] = vmList;
            }

            if(!String.IsNullOrEmpty(itemCode) && type == "remove") // use to display message if remove item from voucher
            {
                TempData["SuccessMessage"] = String.Format("{0} was removed.", itemCode);
            }

            return View(vmList);
        }

        [HttpPost]
        public ActionResult AddItemIntoVoucher(string itemCode, int quantity, string reason)
        {
            Stationery s = stationeryService.FindStationeryByItemCode(itemCode);
            List<NewVoucherViewModel> vmList = Session["NewVoucher"] as List<NewVoucherViewModel>;

            NewVoucherViewModel vm = new NewVoucherViewModel();
            vm.ItemCode = itemCode;
            vm.Quantity = quantity;
            vm.Reason = reason;
            vm.Description = s.description;
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
            TempData["WarningMessage"] = (zeroQuantity)? String.Format("Warning! Quantity of {0} is 0, which will not be submitted.", itemCode) : null;

            return RedirectToAction("NewVoucher");
        }

        [HttpPost]
        public void SaveTemporaryValue(List<NewVoucherViewModel> vmList)
        {
            // to reserve edited quantity value when press Add Item button
            if (vmList != null)
            {
                Session["NewVoucher"] = vmList;
            }
        }


        [HttpPost]
        public ActionResult RemoveVoucherItem(string itemCode, List<NewVoucherViewModel> vmList)
        {
            NewVoucherViewModel vm = vmList.Find(x => x.ItemCode == itemCode);
            vmList.Remove(vm);
            Session["NewVoucher"] = vmList;

            TempData["SuccessMessage"] = String.Format("{0} was removed.", itemCode);

            return RedirectToAction("NewVoucher");
        }

        [HttpPost]
        public ActionResult SubmitVoucher(List<NewVoucherViewModel> vmList)
        {
            // TODO : IMPLEMENT LOGIC

            // TODO: REMOVE HARD CODED REQUESTER ID
            //string requesterID = HttpContext.User.Identity.Name;

            string requesterID = "S1013";

            if(adjustmentVoucherService.SubmitNewAdjustmentVoucher(vmList,3,requesterID))
            {
                // clear list
                Session["NewVoucher"] = new List<NewVoucherViewModel>();
                TempData["SuccessMessage"] = String.Format("Discrepancy report has been submitted for approval.");

                // TODO: TEST EMAIL NOTIFICATION
                // send email notification
             
                decimal voucherAmount = 0.00M;
                foreach(var item in vmList)
                {
                    if(item.Quantity < 0) // count amount for only -ve quantity
                    {
                        voucherAmount += item.Quantity * item.Price;
                    }
                }

                EmailNotification.EmailNotificationForNewAdjustmentVoucher(requesterID, voucherAmount);
            }
            else
            {
                TempData["ErrorMessage"] = String.Format("Error writing to database");
            }

            return RedirectToAction("NewVoucher");
        }


        public ActionResult ClearAllItemInVoucher()
        {
            List<NewVoucherViewModel> vmList = Session["NewVoucher"] as List<NewVoucherViewModel>;
            vmList.Clear();
            Session["RequestList"] = vmList;

            TempData["SuccessMessage"] = String.Format("All items were removed.");

            return RedirectToAction("NewVoucher");
        }

        public ActionResult GetStationeryListJSON(string term = null)
        {
            List<StationeryJSONForCombobox> options = new List<StationeryJSONForCombobox>();

            List<Stationery> stationeries = stationeryService.GetStationeriesBasedOnCriteria(term);
            foreach(var s in stationeries)
            {
                StationeryJSONForCombobox option = new StationeryJSONForCombobox();
                option.id = s.itemCode;
                option.text = String.Format("{0} ({1})", s.itemCode, s.description);
                options.Add(option);
            }
            return Json(options, JsonRequestBehavior.AllowGet);
        }
    }

 
}