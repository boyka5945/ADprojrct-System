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
using Rotativa.MVC;


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

        [RoleAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [RoleAuthorize]
        //DEPTHEAD
        [HttpGet]
        //get all requisition list
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
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(model1.ToPagedList(pageNumber, pageSize));
        }

        [RoleAuthorize]
        //Clerk
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

            //control the following retrieval item have not itemcode ..
            foreach (var itemCode in itemCodes)
            {
                BigModelView bigModel;
                List<Requisition_Record> list = rs.GetRecordByItemCode(itemCode).Where(x => x.status == RequisitionStatus.APPROVED_PROCESSING || x.status == RequisitionStatus.PARTIALLY_FULFILLED).ToList();
                var retrieve = (List<RetrieveForm>)(HttpContext.Application["retrieveList"]);
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
            int pageSize = 100;
            int pageNumber = (page ?? 1);

            Session["page"] = (page ?? 1);
            return View(blist.ToPagedList(pageNumber, pageSize));
            //return View(blist);
        }

        [RoleAuthorize]
        //Clerk
        [HttpPost]
        
        public ActionResult AllocateRequisition(IEnumerable<BigModelView> model, int? page)
        {
            var page1 = (int)Session["page"];
            List<string> itemCodes = rs.GetItemCodeList();
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
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            TempData["Successful"] = " Allocated quantity successful";
            //return View(model.ToPagedList(pageNumber, pageSize));
            return RedirectToAction("ClerkRequisition", l2.ToPagedList(pageNumber, pageSize));


        }

        [RoleAuthorize]
        //Clerk
        [HttpGet]
        public ActionResult AllocateRequisition()
        {

            return RedirectToAction("ClerkRequisition");
        }

        [RoleAuthorize]
        //DEPTDEAD
        [HttpGet]
        public ActionResult ApproveRequisition(int id)
        {
            var RequisitionNO = Convert.ToInt32(Request.QueryString["ID"]); 
            var remarks = Request.QueryString["remark"];
            if (remarks == "")
            {
                TempData["WarningMessage"] = "please input the remarks.";
                return RedirectToAction("ManagerRequisition");
            }
            var userID = HttpContext.User.Identity.Name;
            Requisition_Record model = new Requisition_Record();
            model = rs.GetRequisitionByID(RequisitionNO);
            rs.UpdateRequisition(model, RequisitionStatus.APPROVED_PROCESSING, userID);
            try
            {
                EmailNotification.EmailNotificatioForRequisitionApprovalStatus(RequisitionNO, RequisitionStatus.APPROVED_PROCESSING, remarks);
            }
            catch (Exception e)
            {
                TempData["WarningMessage"] = "Failure to send email notification. Kindly contact IT personnel.";
            }

            return RedirectToAction("ManagerRequisition");
        }

        [RoleAuthorize]
        //Clerk | DEPTHEAD
        [HttpGet]
        public ActionResult RequisitionDetails(int id)
        {
            var RequisitionNO = Convert.ToInt32(Request.QueryString["ID"]);
            RequisitionRecordService rs = new RequisitionRecordService();
            List<Requisition_Detail> model = new List<Requisition_Detail>();
            model = rs.GetDetailsByNo(RequisitionNO);
            return View(model);
        }

        [RoleAuthorize]
        //DEPTHEAD
        [HttpGet]
        public ActionResult RejectRequisition(int id)
        {
            var RequisitionNO = Convert.ToInt32(Request.QueryString["ID"]);
            var remarks = Request.QueryString["remark"];
            if (remarks == "")
            {
                TempData["WarningMessage"] = "please input the remarks.";
                return RedirectToAction("ManagerRequisition");
            }
            Requisition_Record model = new Requisition_Record();
            model = rs.GetRequisitionByID(RequisitionNO);
            rs.UpdateRequisition(model, RequisitionStatus.REJECTED, "");
            try
            {
                EmailNotification.EmailNotificatioForRequisitionApprovalStatus(RequisitionNO, RequisitionStatus.REJECTED, remarks);
            }
            catch (Exception e)
            {
                TempData["WarningMessage"] = "Failure to send email notification. Kindly contact IT personnel.";
            }

            return RedirectToAction("ManagerRequisition");
        }

        [RoleAuthorize]
        //Clerk
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
            if (HttpContext.Application["tempDisbursement"] != null)
            {
                List<Disbursement> l = (List<Disbursement>)HttpContext.Application["tempDisbursement"];
                foreach (var item in list)
                {
                    foreach (var i in l)
                    {
                        if (item.itemCode == i.itemCode && item.departmentCode == i.departmentCode)
                        {
                            item.actualQty = i.actualQty;
                        }
                    }
                }
            }
            HttpContext.Application["TotalItem"] = list.Count();
            StationeryModel entity = new StationeryModel();

            User user = null;

            try
            {
                user = entity.Users.Where(x => x.departmentCode == deptCode && x.role == (int)UserRoles.RoleID.UserRepresentative).First();
            }
            catch (Exception e)
            {
                // get depthead as UR if not assign
                if(deptCode != "STORE")
                {
                    user = entity.Users.Where(x => x.departmentCode == deptCode && x.role == (int)UserRoles.RoleID.DepartmentHead).First();
                }
                else if (deptCode == "STORE")
                {
                    user = entity.Users.Where(x => x.departmentCode == deptCode && x.role == (int)UserRoles.RoleID.StoreManager).First();
                }
            }

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
                SelectListItem item = new SelectListItem();
                item.Value = b.departmentCode.ToString();
                item.Text = b.departmentName.ToString();
                if (b.departmentCode == deptCode)
                {
                    item.Selected = true;
                }
                departmentlist.Add(item);
            }
            Session["deptCode"] = deptCode;
            ViewData["list"] = departmentlist;

            //return View(list);
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        [RoleAuthorize]
        //Clerk
        [HttpGet]
        public ActionResult GenerateDisbursementListPDF(string ID) // id = departmentCode
        {
            string deptCode = ID;

            List<Disbursement> list = rs.GetRequisitionByDept(deptCode);

            HttpContext.Application["TotalItem"] = list.Count();

            StationeryModel entity = new StationeryModel();

            User user = null;

            try
            {
                user = entity.Users.Where(x => x.departmentCode == deptCode && x.role == (int)UserRoles.RoleID.UserRepresentative).First();
            }
            catch (Exception e)
            {
                // get depthead as UR if not assign
                if(deptCode != "STORE")
                {
                    user = entity.Users.Where(x => x.departmentCode == deptCode && x.role == (int)UserRoles.RoleID.DepartmentHead).First();
                }
                else if (deptCode == "STORE")
                {
                    user = entity.Users.Where(x => x.departmentCode == deptCode && x.role == (int)UserRoles.RoleID.StoreManager).First();
                }
            }


            foreach (var a in ds.GetAllDepartment().ToList())
            {
                if (a.departmentCode == deptCode)
                {
                    ViewBag.Point = a.Collection_Point.collectionPointName;
                    ViewBag.rp = user.name;
                    ViewBag.Dept = a.departmentName;
                    break;
                }
            }

            string fileName = String.Format("Disbursement_List_of_{0}_on_{1}.pdf", deptCode, DateTime.Today.ToShortDateString());
            return new ViewAsPdf("_GeneratePDF", list) { FileName = fileName };
        }

        [RoleAuthorize]
        //Clerk
        [HttpPost]
        public ActionResult DisbursementList(FormCollection form, int? page)
        {
            Session["deptCode"] = form["ID"];
            var deptCode = form["ID"].ToString();
            StationeryModel entity = new StationeryModel();


            User user = null;

            try
            {
                user = entity.Users.Where(x => x.departmentCode == deptCode && x.role == (int)UserRoles.RoleID.UserRepresentative).First();
            }
            catch (Exception e)
            {
                // get depthead as UR if not assign
                if(deptCode != "STORE")
                {
                    user = entity.Users.Where(x => x.departmentCode == deptCode && x.role == (int)UserRoles.RoleID.DepartmentHead).First();
                }
                else if (deptCode == "STORE")
                {
                    user = entity.Users.Where(x => x.departmentCode == deptCode && x.role == (int)UserRoles.RoleID.StoreManager).First();
                }
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
                SelectListItem item = new SelectListItem();
                item.Value = b.departmentCode.ToString();
                item.Text = b.departmentName.ToString();
                if (b.departmentCode == deptCode)
                {
                    item.Selected = true;
                }
                departmentlist.Add(item);
            }
            ViewData["list"] = departmentlist;
            var list = rs.GetRequisitionByDept(deptCode);
            if (HttpContext.Application["tempDisbursement"] != null)
            {
                List<Disbursement> l = (List<Disbursement>)HttpContext.Application["tempDisbursement"];
                foreach (var item in list)
                {
                    foreach (var i in l)
                    {
                        if (item.itemCode == i.itemCode && item.departmentCode == i.departmentCode)
                        {
                            item.actualQty = i.actualQty;
                        }
                    }
                }
            }
            //return View(list);
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        [RoleAuthorize]
        //Clerk
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

        [RoleAuthorize]
        //Clerk
        [HttpGet]
        public ActionResult updateRetrieve()
        {
            
            StationeryModel entity = new StationeryModel();
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
            var status = Request.QueryString["key7"];
            if (status != null)
            {
                List<Requisition_Detail> list = entity.Requisition_Detail.Where(x => x.itemCode == itemCode).ToList();
                foreach (var item in list)
                {
                    item.allocatedQty = 0;
                }
                entity.SaveChanges();

            }
            var rlist = (List<RetrieveForm>)HttpContext.Application["retrieveList"];
            rlist.Where(x => x.ItemCode == itemCode).First().retrieveQty = RetrieveQty;
            HttpContext.Application["retrieveList"] = rlist;
            TempData["Successful"] = "Retrieve successfully.";

            return RedirectToAction("GenerateRetrieveForm", new { pagenumber = page });
        }

        [RoleAuthorize]
        //Clerk
        [HttpGet]
        public ActionResult UpdateDisbursement()
        {
            //update disbursement one by one.
            if (HttpContext.Application["tempDisbursement"] != null)
            {
                List<Disbursement> l = (List<Disbursement>)HttpContext.Application["tempDisbursement"];
                for (int i = 0; i < l.Count; i++)
                {
                    rs.UpdateDisbursement(l[i].itemCode, (int)l[i].actualQty, l[i].departmentCode, (int)l[i].quantity, i, HttpContext.User.Identity.Name);
                    
                }
            }
            HttpContext.Application.Lock();
            HttpContext.Application["tempDisbursement"] = null;
            HttpContext.Application["retrieveList"] = null;
            HttpContext.Application.UnLock();
            TempData["Successful"] = "submit successful.";
            return RedirectToAction("DisbursementList");
        }

        [RoleAuthorize]
        //Clerk
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
            Boolean status = true;
            List<Disbursement> list = new List<Disbursement>();
            if (HttpContext.Application["tempDisbursement"] != null)
            {

                list = (List<Disbursement>)HttpContext.Application["tempDisbursement"];
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].departmentCode == deptCode && list[i].itemCode == itemCode)
                    {
                        list[i].actualQty = actualQty;
                        status = false;
                        break;
                    }
                }
                if (status)
                {
                    Disbursement d = new Disbursement();
                    d.itemCode = itemCode;
                    d.quantity = allocateQty;
                    d.departmentCode = deptCode;
                    d.actualQty = actualQty;
                    list.Add(d);
                }
            }
            else
            {
                Disbursement d = new Disbursement();
                d.itemCode = itemCode;
                d.quantity = allocateQty;
                d.departmentCode = deptCode;
                d.actualQty = actualQty;
                list.Add(d);
            }

            HttpContext.Application.Lock();
            HttpContext.Application["tempDisbursement"] = list;
            HttpContext.Application.UnLock();

            TempData["Successful"] = "Save disbursement successful.";
            ViewBag.Select = deptCode;
            return RedirectToAction("DisbursementList");
        }

        [RoleAuthorize]
        //Clerk
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
                int pageSize = 8;
                int pageNumber = (page ?? 1);
                return View(model.ToPagedList(pageNumber, pageSize));
            }
            else if (HttpContext.Application["retrieveList"] != null)
            {
                List < RetrieveForm > list  = (List<RetrieveForm>)HttpContext.Application["retrieveList"];
                List<RetrieveForm> model = rs.GetRetrieveFormByDateTime(DateTime.Now);
                foreach (var item in model)
                {
                    foreach(var i in list)
                    {
                        if (i.ItemCode == item.ItemCode)
                        {
                            item.retrieveQty = i.retrieveQty;
                        }
                    }
                }
                HttpContext.Application["retrieveList"] = model;
                int pageSize = 8;
                int pageNumber = (page ?? 1);
                return View(model.ToPagedList(pageNumber, pageSize));
            }
            return View();
        }

        public ActionResult GenerateRetrieveFormPDF()
        {
            List<RetrieveForm> model = (List<RetrieveForm>) HttpContext.Application["retrieveList"];

            string fileName = String.Format("Stationery_Retrieval_List_of_on_{0}.pdf", DateTime.Today.ToShortDateString());
            return new ViewAsPdf("_GenerateRetrieveFormPDF", model) { FileName = fileName };
        }


        [RoleAuthorize]
        //Clerk
        [HttpPost]
        public ActionResult GenerateRetrieveForm(FormCollection form, int? page)
        {
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
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }
    }
}