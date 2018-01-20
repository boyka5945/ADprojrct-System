using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using PagedList;

namespace Inventory_mvc.Controllers
{
    public class ManageRequisitionsController : Controller
    {
        IStationeryService ss = new StationeryService();
        IRequisitionRecordService rs = new RequisitionRecordService();
        IDepartmentService ds = new DepartmentService();
        IUserService us = new UserService();
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
            List<Requisition_Record> model = rs.GetAllRequisition();
            List<Requisition_Record> model1 = new List<Requisition_Record>();
            foreach (var m in model)
            {
                if (m.status == "Approved and Processing" || m.status == "Rejected" || m.status == "Pending")
                {
                    model1.Add(m);
                }
            }
            
            //return View(model1);
            int pageSize = 1;
            int pageNumber = (page ?? 1);
            return View(model1.ToPagedList(pageNumber, pageSize));
        }

        //for clerk
        [HttpGet]
        public ActionResult ClerkRequisition(int? page) { 
            List<string> itemCodes = rs.GetItemCodeList();
            List<BigModelView> blist = new List<BigModelView>();
            foreach(var itemCode in itemCodes)
            {
                BigModelView bigModel;
                List<Requisition_Record> list = rs.GetRecordByItemCode(itemCode);
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
                            bigModel.retrievedQuantity = "Havent retrieve yet.";
                        }
                    }
                    else
                    {
                        bigModel.description = "";
                        bigModel.itemCode = "";
                        bigModel.retrievedQuantity = "";
                    }
                    bigModel.requisitionRecord = list[i];
                    bigModel.unfulfilledQty = rs.FindUnfulfilledQtyBy2Key(itemCode, list[i].requisitionNo);
                    bigModel.allocateQty = rs.FindDetailsBy2Key(itemCode, list[i].requisitionNo).allocatedQty;
                    blist.Add(bigModel);
                }
                TempData["BigModel"] = blist;
            }
            
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            TempData["page"] = (page ?? 1);
            return View(blist.ToPagedList(pageNumber, pageSize));
            //return View(blist);
        }

        [HttpPost]
        public ActionResult AllocateRequisition(IEnumerable<BigModelView> model, int? page)
        {
            var page1 = (int)TempData["page"];
            if (ModelState.IsValid)
            {
                List<BigModelView> l = model.ToList();
                List<BigModelView> l2 = (List<BigModelView>)TempData["BigModel"];
                for (int i = 0; i < l.Count; i++)
                {
                    l2[i + (int)(page1-1)*4].allocateQty = l[i].allocateQty;
                }
                for (int i = 0; i < l2.Count - (page1 - 1) * 4; i++) {
                    if (l2[i + (int)(page1 - 1) * 4].itemCode == "")
                    {
                        for (int j = i; j >= 0; j--)
                        {
                            if (l2[j + (int)(page1 - 1) * 4].itemCode != "")
                            {
                                rs.UpdateDetails(l2[j + (int)(page1 - 1) * 4].itemCode, l2[i + (int)(page1 - 1) * 4].requisitionRecord.requisitionNo, l2[i + (int)(page1 - 1) * 4].allocateQty);
                                break;
                            }
                        }
                    }
                    else
                    {
                        rs.UpdateDetails(l2[i + (int)(page1 - 1) * 4].itemCode, l2[i + (int)(page1 - 1) * 4].requisitionRecord.requisitionNo, l2[i + (int)(page1 - 1) * 4].allocateQty);
                    }
                }
                int pageSize = 4;
                int pageNumber = (page ?? 1);
                //return View(model.ToPagedList(pageNumber, pageSize));
                return RedirectToAction("ClerkRequisition", model.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                int pageSize = 4;
                int pageNumber = (page ?? 1);
                //return View(model.ToPagedList(pageNumber, pageSize));
                return RedirectToAction("ManagerRequisition", model.ToPagedList(pageNumber, pageSize));
            }
        }

        [HttpGet]
        public ActionResult AllocateRequisition()
        {
            
            return RedirectToAction("ClerkRequisition");
        }

        [HttpGet]
        public ActionResult ApproveRequisition(int id)
        {
          
            Requisition_Record model = new Requisition_Record();
            model = rs.GetRequisitionByID(id);
            rs.UpdateRequisition(model, "Approved and Processing");
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
            rs.UpdateRequisition(model, "Rejected");
            return RedirectToAction("ManagerRequisition");
        }

        [HttpGet]
        public ActionResult DisbursementList(int? page)
        {

            string deptCode = "ZOOL";
            if (Session["deptCode"] != null)
            {
                ViewBag.Select = ds.GetDepartmentByCode(Session["deptCode"].ToString()).departmentName;
                deptCode = Session["deptCode"].ToString();
            }
            List<Disbursement> list;

            list = rs.GetRequisitionByDept(deptCode);
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
            foreach (var b in departments) {
                departmentlist.Add(new SelectListItem{Value = b.departmentCode.ToString(),Text = b.departmentName.ToString()});
            }

            ViewData["list"] = departmentlist;

            //return View(list);

            int pageSize = 1;
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
            //return View(list);
            int pageSize = 1;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult GenerateRetrieveForm(int? page)
        {
            if (page == null)
            {
                return View();
            }
            if (Session["retrieveList"] != null)
            {
                List < RetrieveForm > model = (List<RetrieveForm>)Session["retrieveList"];
                int pageSize = 1;
                int pageNumber = (page ?? 1);
                return View(model.ToPagedList(pageNumber, pageSize));
            }
            return View();
        }

        [HttpPost]
        public ActionResult GenerateRetrieveForm(FormCollection form, int? page)
        {
            DateTime? from = Convert.ToDateTime(form["from"]);
            RetrieveForm rf = new RetrieveForm();
            List<RetrieveForm> model = rs.GetRetrieveFormByDateTime(from);
            
            Session["retrieveList"] = model;
                        //return View(model);
            int pageSize = 1;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult GetApplication(IEnumerable<RetrieveForm> form)
        {

            HttpContext.Application["retrieveform"] = form.ToList();
            foreach (var item in form) {

                StationeryViewModel model = ss.FindStationeryViewModelByItemCode(item.ItemCode);
                model.StockQty = model.StockQty - (int)item.retrieveQty;
                ss.UpdateStationeryInfo(model);
            }
            return RedirectToAction("GenerateRetrieveForm");
        }


    }
}