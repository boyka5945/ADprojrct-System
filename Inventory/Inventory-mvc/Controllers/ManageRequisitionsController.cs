﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Service;
using Inventory_mvc.Models;

namespace Inventory_mvc.Controllers
{
    public class ManageRequisitionsController : Controller
    {
        IStationeryService ss = new StationeryService();
        IRequisitionRecordService rs = new RequisitionRecordService();
        // GET: RequisitionRecord
        public ActionResult Index()
        {
            return View();
        }

        //for manager
        [HttpGet]
        public ActionResult ManagerRequisition()
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
            
            return View(model1);
        }

        //for clerk
        [HttpGet]
        public ActionResult ClerkRequisition() { 
            List<string> itemCodes = rs.GetItemCodeList();
            
            List<BigModelView> blist = new List<BigModelView>();
            foreach(var itemCode in itemCodes)
            {
                BigModelView bigModel;
                List<Requisition_Record> list = rs.GetRecordByItemCode(itemCode);
                for (int i = 0; i < list.Count; i++)
                {
                    bigModel = new BigModelView();
                    if (i < 1)
                    {
                        bigModel.description = ss.FindStationeryByItemCode(itemCode).description;
                        bigModel.itemCode = itemCode;
                        bigModel.retrievedQuantity = "100";
                    }
                    else
                    {
                        bigModel.description = "";
                        bigModel.itemCode = "";
                        bigModel.retrievedQuantity = "";
                    }
                    bigModel.requisitionRecord = list[i];
                    bigModel.unfulfilledQty = rs.FindUnfulfilledQtyBy2Key(itemCode, list[i].requisitionNo);
                    bigModel.allocateQty = (int)rs.FindDetailsBy2Key(itemCode, list[i].requisitionNo).allocatedQty;
                    blist.Add(bigModel);
                }
                TempData["BigModel"] = blist;
            }
            return View(blist);
        }

        [HttpPost]
        public ActionResult AllocateRequisition(IEnumerable<BigModelView> model)
        {
            if (ModelState.IsValid)
            {
                List<BigModelView> l = model.ToList();
                List<BigModelView> l2 = (List<BigModelView>)TempData["BigModel"];
                for (int i = 0; i < l.Count; i++)
                {
                    l2[i].allocateQty = l[i].allocateQty;
                }
                for (int i = 0; i < l2.Count; i++) {
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
                return RedirectToAction("ClerkRequisition");
            }
            else
            {
                return RedirectToAction("ManagerRequisition");
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
            List<Requisition_Details> model = new List<Requisition_Details>();
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
    }
}