using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.Controllers
{
    public class AdjustmentVoucherController : Controller
    {
        IStationeryService stationeryService = new StationeryService();


        // GET: AdjustmentVoucherRecord
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewVoucher(string itemCode = null)
        {
            List<NewVoucherViewModel> vmList = Session["NewVoucher"] as List<NewVoucherViewModel>;

            if (vmList == null)
            {
                vmList = new List<NewVoucherViewModel>();
                Session["NewVoucher"] = vmList;
            }

            if(!String.IsNullOrEmpty(itemCode)) // use to display message once remove item from voucher
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
            vmList.Add(vm);

            Session["NewVoucher"] = vmList;

            TempData["SuccessMessage"] = String.Format("{0} was added.", itemCode);

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
            // LOGIC

            // Submit Form
            TempData["SuccessMessage"] = String.Format("Discrepancy report has been submitted.");

            List<NewVoucherViewModel> list = new List<NewVoucherViewModel>();
            Session["NewVoucher"] = list;


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