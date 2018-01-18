using Inventory_mvc.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using PagedList;


namespace Inventory_mvc.Controllers
{
    public class ListRequisitionsController : Controller
    {
        IStationeryService stationeryService = new StationeryService();
        IRequisitionRecordService requisitionService = new RequisitionRecordService();
        IUserService userService = new UserService();

        // GET: ListRequisitions
        public ActionResult Index(int? page)
        {
            // Retrieve all requisitions made by current user

            // TODO: REMOVE HARD CODED REQUESTER ID
            //string requesterID = HttpContext.User.Identity.Name;

            string requesterID = "S1013";
            List<Requisition_Record> records = requisitionService.GetRecordsByRequesterID(requesterID);

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(records.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult RemoveRecord(string requisitionNo)
        {
            // TODO: Return to index after remove, Server side validation
            return View();
        }

        public ActionResult EditRecord(string requisitionNo)
        {
            // TODO: Get request_detail list of particular requisitionNo, Server side validation
            return View();
        }

        public ActionResult ShowDetail(string requisitionNo)
        {
            // TODO: Get request_detail list of particular requisitionNo
            return View();
        }


        
    }
}