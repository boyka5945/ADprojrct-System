using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using Inventory_mvc.Utilities;
using PagedList;
using Inventory_mvc.Function;

namespace Inventory_mvc.Controllers
{
    public class ManageRequisitionsController : Controller
    {
        IStationeryService ss = new StationeryService();
        IRequisitionRecordService rs = new RequisitionRecordService();
        IDepartmentService ds = new DepartmentService();
        IUserService us = new UserService();
        ITransactionRecordService ts = new TransactionRecordService();
        IInventoryStatusRecordService Is = new InventoryStatusRecordService();
        IAdjustmentVoucherService ivs = new AdjustmentVoucherService();
        // GET: RequisitionRecord
        public ActionResult Index()
        {
            return View();
        }

        //for manager
        [HttpGet]
        public ActionResult ManagerRequisition(int? page)
        {
            string name = HttpContext.User.Identity.Name;

            List<Requisition_Record> list = new List<Requisition_Record>();
            List<Requisition_Record> model = rs.GetAllRequisition();
            foreach (var item in model)
            {
                if (item.deptCode == us.FindDeptCodeByID(name))
                {
                    list.Add(item);
                }
            }
            List<Requisition_Record> model1 = new List<Requisition_Record>();
            foreach (var m in list)
            {
                if (m.status == RequisitionStatus.APPROVED_PROCESSING || m.status == RequisitionStatus.REJECTED || m.status == RequisitionStatus.PENDING_APPROVAL)
                {
                    model1.Add(m);
                }
            }

            //return View(model1);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(model1.ToPagedList(pageNumber, pageSize));
        }

        //for clerk
        [HttpGet]
        public ActionResult ClerkRequisition(int? page)
        {
            if (Request.QueryString["ErrorMessage"] != null)
            {
                TempData["ErrorMessage"] = Request.QueryString["ErrorMessage"];
                return RedirectToAction("ClerkRequisition");
            }
            List<BigModelView> blist = new List<BigModelView>();
            List<string> itemCodes = rs.GetItemCodeList();
            if (HttpContext.Application["BigModel"] != null)
            {
                blist = (List<BigModelView>)HttpContext.Application["BigModel"];
            }
            else
            {
                foreach (var itemCode in itemCodes)
                {
                    BigModelView bigModel;
                    List<Requisition_Record> list = rs.GetRecordByItemCode(itemCode).Where(x => x.status == RequisitionStatus.APPROVED_PROCESSING || x.status == RequisitionStatus.PARTIALLY_FULFILLED).ToList();
                    var retrieve = (List<RetrieveForm>)(HttpContext.Application["retrieveform"]);
                    for (int i = 0; i < list.Count; i++)
                    {
                        bigModel = new BigModelView();
                        if (i < 1)
                        {
                            bigModel.description = ss.FindStationeryByItemCode(itemCode).description;
                            bigModel.itemCode = itemCode;
                            if (retrieve != null)
                            {
                                foreach (var r in retrieve)
                                {
                                    if (r.ItemCode == itemCode)
                                        bigModel.retrievedQuantity = r.retrieveQty.ToString();
                                }
                            }
                            else
                            {
                                bigModel.retrievedQuantity = "0";
                            }
                        }
                        else
                        {
                            bigModel.description = "";
                            bigModel.itemCode = "";
                            bigModel.retrievedQuantity = "";
                        }
                        bigModel.requisitionRecord = list[i];
                        if (rs.FindUnfulfilledQtyBy2Key(itemCode, list[i].requisitionNo) == null)
                            bigModel.unfulfilledQty = 0;
                        else
                            bigModel.unfulfilledQty = rs.FindUnfulfilledQtyBy2Key(itemCode, list[i].requisitionNo);
                        if (rs.FindDetailsBy2Key(itemCode, list[i].requisitionNo).allocatedQty == null)
                            bigModel.allocateQty = 0;
                        else
                            bigModel.allocateQty = rs.FindDetailsBy2Key(itemCode, list[i].requisitionNo).allocatedQty;
                        blist.Add(bigModel);
                    }
                    HttpContext.Application["BigModel"] = blist;
                }
            }
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            Session["page"] = (page ?? 1);
            return View(blist.ToPagedList(pageNumber, pageSize));
            //return View(blist);
        }

        [HttpPost]
        public ActionResult AllocateRequisition(IEnumerable<BigModelView> model, int? page)
        {
            //if (HttpContext.Application["retrieveform"] == null)
            //{
            //    TempData["ErrorMessage"] = "Have not retrieve yet.";
            //    return RedirectToAction("ClerkRequisition");
            //}
            var page1 = (int)Session["page"];
            List<string> itemCodes = rs.GetItemCodeList();
            var ls = (List<RetrieveForm>)HttpContext.Application["retrieveform"];
            List<BigModelView> l = model.ToList();
            List<BigModelView> l2 = (List<BigModelView>)HttpContext.Application["BigModel"];
            int qty = 0;
            for (int i = 0; i < itemCodes.Count(); i++)
            {
                qty = 0;
                int length = rs.DetailsCountOfOneItemcode(itemCodes[i]);
                for (int k = 0; k < l2.Count(); k++)
                {
                    if (l2[k].itemCode == itemCodes[i])
                    {
                        int check = k;
                        for (int p = 0; p < length; p++)
                        {
                            qty = qty + (int)l2[k + p].allocateQty;
                        }
                        if (l2[check].retrievedQuantity == "")
                        {
                            TempData["ErrorMessage"] = l2[check].description + " have not retrieve yet.";
                            return RedirectToAction("ClerkRequisition");
                        }
                        if (qty > Convert.ToInt32(l2[check].retrievedQuantity))
                        {
                            ViewBag.Message = "";
                            TempData["ErrorMessage"] = l2[check].description + " allocated quantity more than retrieved quantity.";
                            return RedirectToAction("ClerkRequisition");
       
                        }
                    }
                }
            }
            for (int i = 0; i < l2.Count(); i++)
            {
                if (l2[i].itemCode == "")
                {
                    for (int j = i; j >= 0; j--)
                    {
                        if (l2[j].itemCode != "")
                        {
                            rs.UpdateDetails(l2[j].itemCode, l2[i].requisitionRecord.requisitionNo, l2[i].allocateQty);
                            break;
                        }
                    }
                }
                else
                {
                    rs.UpdateDetails(l2[i].itemCode, l2[i].requisitionRecord.requisitionNo, l2[i].allocateQty);
                }
            }
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            TempData["Successful"] = " Allocated quantity successful";
            //return View(model.ToPagedList(pageNumber, pageSize));
            return RedirectToAction("ClerkRequisition", l2.ToPagedList(pageNumber, pageSize));


        }

        [HttpGet]
        public ActionResult AllocateRequisition()
        {

            return RedirectToAction("ClerkRequisition");
        }

        [HttpGet]
        public ActionResult ApproveRequisition(int id)
        {
            var userID = HttpContext.User.Identity.Name;
            Requisition_Record model = new Requisition_Record();
            model = rs.GetRequisitionByID(id);
            rs.UpdateRequisition(model, RequisitionStatus.APPROVED_PROCESSING, userID);
            EmailNotification.EmailNotificatioForRequisitionApprovalStatus(id, RequisitionStatus.APPROVED_PROCESSING, "no reason");

            return RedirectToAction("ManagerRequisition");
        }

        [HttpGet]
        public ActionResult RequisitionDetails(int id)
        {
            RequisitionRecordService rs = new RequisitionRecordService();
            List<Requisition_Detail> model = new List<Requisition_Detail>();
            model = rs.GetDetailsByNo(id);
            return View(model);
        }

        [HttpGet]
        public ActionResult RejectRequisition(int id)
        {
            Requisition_Record model = new Requisition_Record();
            model = rs.GetRequisitionByID(id);
            rs.UpdateRequisition(model, RequisitionStatus.REJECTED, "");
            EmailNotification.EmailNotificatioForRequisitionApprovalStatus(id, RequisitionStatus.REJECTED, "no reason");
            return RedirectToAction("ManagerRequisition");
        }


        [HttpGet]
        public ActionResult DisbursementList(int? page)
        {
            var alllist = rs.GetRequisitionByDept("");
            string deptCode = "ZOOL";
            if (Session["deptCode"] != null)
            {
                ViewBag.Select = ds.GetDepartmentByCode(Session["deptCode"].ToString()).departmentName;
                deptCode = Session["deptCode"].ToString();
            }
            List<Disbursement> list;
            list = rs.GetRequisitionByDept(deptCode);
            if (HttpContext.Application["DisbursementQty"] != null)
            {
                List<Disbursement> l = (List<Disbursement>)HttpContext.Application["DisbursementQty"];
                foreach (var item in list)
                {
                    foreach (var i in l)
                    {
                        if (item.itemCode == i.itemCode)
                        {
                            item.actualQty = i.actualQty;
                        }
                    }
                }
            }
            HttpContext.Application["TotalItem"] = list.Count();
            StationeryModel entity = new StationeryModel();
            var user = entity.Users.Where(x => x.departmentCode == deptCode).First();
            foreach (var a in ds.GetAllDepartment().ToList())
            {
                if (a.departmentCode == deptCode)
                {
                    ViewBag.Point = a.Collection_Point.collectionPointName;
                    ViewBag.rp = user.name;
                    break;
                }
            }
            List<SelectListItem> departmentlist = new List<SelectListItem>();
            var departments = ds.GetAllDepartment();
            foreach (var b in departments)
            {
                departmentlist.Add(new SelectListItem { Value = b.departmentCode.ToString(), Text = b.departmentName.ToString() });
            }
            Session["deptCode"] = "ZOOL";
            ViewData["list"] = departmentlist;

            //return View(list);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult DisbursementList(FormCollection form, int? page)
        {
            Session["deptCode"] = form["ID"];
            var deptCode = form["ID"].ToString();
            StationeryModel entity = new StationeryModel();
            User user;
            if (entity.Users.Where(x => x.departmentCode == deptCode).ToList().Count != 0)
            {
                user = entity.Users.Where(x => x.departmentCode == deptCode).First();
            }
            else
            {
                user = null;
            }
            foreach (var a in ds.GetAllDepartment().ToList())
            {
                if (a.departmentCode == deptCode)
                {
                    ViewBag.Select = a.departmentName;
                    ViewBag.Point = a.Collection_Point.collectionPointName;
                    if (user != null)
                        ViewBag.rp = user.name;
                    else
                        ViewBag.rp = "";
                    break;
                }
            }

            List<SelectListItem> departmentlist = new List<SelectListItem>();
            var departments = ds.GetAllDepartment();
            foreach (var b in departments)
            {
                departmentlist.Add(new SelectListItem { Value = b.departmentCode.ToString(), Text = b.departmentName.ToString() });
            }
            ViewData["list"] = departmentlist;
            var list = rs.GetRequisitionByDept(deptCode);
            if (HttpContext.Application["DisbursementQty"] != null)
            {
                List<Disbursement> l = (List<Disbursement>)HttpContext.Application["DisbursementQty"];
                foreach (var item in list)
                {
                    foreach (var i in l)
                    {
                        if (item.itemCode == i.itemCode)
                        {
                            item.actualQty = i.actualQty;
                        }
                    }
                }
            }
            //return View(list);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }


        [HttpPost]
        public void KeepTempData(List<BigModelView> list)
        {
            int page = (int)Session["page"];
            if (list != null || list.Count != 0)
            {
                HttpContext.Application.Lock();
                List<BigModelView> wholeList = (List<BigModelView>)HttpContext.Application["BigModel"];
                HttpContext.Application.UnLock();

                for (int i = 0; i < list.Count(); i++)
                {

                    wholeList[i + (page - 1) * 4].allocateQty = list[i].allocateQty;
                }

                HttpContext.Application.Lock();
                HttpContext.Application["BigModel"] = wholeList;
                HttpContext.Application.UnLock();
            }
        }

        [HttpGet]
        public ActionResult updateRetrieve()
        {
            if (Request.QueryString["ErrorMessage"] != null)
            {
                TempData["ErrorMessage"] = Request.QueryString["ErrorMessage"];
                return RedirectToAction("GenerateRetrieveForm");
            }
            var RetrieveQty = Convert.ToInt32((Request.QueryString["key1"]));
            var itemCode = Request.QueryString["key2"];
            var description = Request.QueryString["key4"];
            var Qty = Convert.ToInt32(Request.QueryString["key3"]);
            var page = Convert.ToInt32(Request.QueryString["key5"]);
            var stock = Convert.ToInt32(Request.QueryString["key6"]);
            RetrieveForm rf = new RetrieveForm();
            rf.description = description;
            rf.ItemCode = itemCode;
            rf.Qty = Qty;
            rf.retrieveQty = RetrieveQty;
            rf.StockQty = stock;
            //StationeryViewModel stationery = ss.FindStationeryViewModelByItemCode(itemCode);

            //stationery.StockQty -= RetrieveQty;
            //ss.UpdateStationeryInfo(stationery);

            var rlist = (List<RetrieveForm>)HttpContext.Application["retrieveList"];
            rlist.Where(x => x.ItemCode == itemCode).First().retrieveQty = RetrieveQty;
            HttpContext.Application["retrieveList"] = rlist;
            if (HttpContext.Application["retrieveform"] == null)
            {
                List<RetrieveForm> list = new List<RetrieveForm>();
                list.Add(rf);
                HttpContext.Application["retrieveform"] = list;
            }
            else
            {
                List<RetrieveForm> list = (List<RetrieveForm>)HttpContext.Application["retrieveform"];
                list.Add(rf);
                HttpContext.Application["retrieveform"] = list;
            }
            TempData["Successful"] = "Retrieve successfully.";
            //Session["pagee"] = page;
            //return RedirectToAction("GenerateRetrieveForm");
            return RedirectToAction("GenerateRetrieveForm", new { pagenumber = page }) ;
        }

        [HttpGet]
        public ActionResult UpdateDisbursement()
        {
            if (HttpContext.Application["DisbursementQty"] != null)
            {
                List<Disbursement> l = (List<Disbursement>)HttpContext.Application["DisbursementQty"];
                for (int i = 0; i < l.Count; i++)
                {
                    rs.UpdateDisbursement(l[i].itemCode, (int)l[i].actualQty, l[i].departmentCode, (int)l[i].quantity, i, HttpContext.User.Identity.Name);
                }
            }
            HttpContext.Application["DisbursementQty"] = null;
            HttpContext.Application["retrieveList"] = null;
            HttpContext.Application["retrieveform"] = null;
            HttpContext.Application["BigModel"] = null;
            TempData["Successful"] = "submit successful.";
            return RedirectToAction("DisbursementList");
        }

        [HttpGet]
        public ActionResult SaveDisbursementList()
        {
            if (Request.QueryString["ErrorMessage"] != null)
            {
                TempData["ErrorMessage"] = Request.QueryString["ErrorMessage"];
                ViewBag.Select = Session["deptCode"].ToString();
                return RedirectToAction("DisbursementList");
            }
            var actualQty = Convert.ToInt32((Request.QueryString["key1"]));
            var itemCode = Request.QueryString["key2"];
            var allocateQty = Convert.ToInt32(Request.QueryString["key3"]);
            var remarks1 = Request.QueryString["key4"];
            var deptCode = Session["deptCode"].ToString();

            List<Disbursement> l;
            if (HttpContext.Application["DisbursementQty"] == null)
            {
                l = new List<Disbursement>();
            }
            else
            {
                l = (List<Disbursement>)HttpContext.Application["DisbursementQty"];
            }

            Disbursement d = new Disbursement();
            d.itemCode = itemCode;
            d.actualQty = actualQty;
            d.quantity = allocateQty;
            d.departmentCode = deptCode;

            l.Add(d);
            HttpContext.Application["DisbursementQty"] = l;
            TempData["Successful"] = "Save disbursement successful.";
            ViewBag.Select = deptCode;
            return RedirectToAction("DisbursementList");
        }

        [HttpGet]
        public ActionResult GenerateRetrieveForm(int? page, string pagenumber)
        {
            if (pagenumber != null)
            {
                page = Convert.ToInt32(pagenumber);
            }
            if (HttpContext.Application["retrieveList"] == null)
            {
                List<RetrieveForm> model = rs.GetRetrieveFormByDateTime(DateTime.Now);
                HttpContext.Application["retrieveList"] = model;
                int pageSize = 4;
                int pageNumber = (page ?? 1);
                return View(model.ToPagedList(pageNumber, pageSize));
            }
            else if (HttpContext.Application["retrieveList"] != null)
            {
                List<RetrieveForm> model = (List<RetrieveForm>)HttpContext.Application["retrieveList"];
                int pageSize = 4;
                int pageNumber = (page ?? 1);
                return View(model.ToPagedList(pageNumber, pageSize));
            }
            return View();
        }

        [HttpPost]
        public ActionResult GenerateRetrieveForm(FormCollection form, int? page)
        {
            //DateTime s = new DateTime();
            //if (form["from"] == null)
            //{
            //    return View("GenerateRetrieveForm");
            //}
            //if (!DateTime.TryParse(form["from"], out s))
            //{
            //    return View();
            //}
            DateTime? from = DateTime.Now;
            RetrieveForm rf = new RetrieveForm();
            List<RetrieveForm> model = new List<RetrieveForm>();
            Session["date"] = from;

            if (HttpContext.Application["retrieveList"] == null)
            {
                model = rs.GetRetrieveFormByDateTime(from);
                HttpContext.Application["retrieveList"] = model;
            }
            else
            {
                model = (List<RetrieveForm>)HttpContext.Application["retrieveList"];
            }
            HttpContext.Application["page3"] = (page ?? 1);
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }
    }
}