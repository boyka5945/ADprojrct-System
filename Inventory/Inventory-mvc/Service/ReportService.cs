using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.DAO;
using System.Web.Mvc;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.Service
{
    public class ReportService : IReportService
    {
        IReportDAO reportDAO = new ReportDAO();
        IStationeryService stationeryService = new StationeryService();
        
        public List<ReportViewModel> RetrieveQty(DateTime ds,DateTime de)
        {
            List<Purchase_Detail> details = reportDAO.RetrieveQty(ds,de);

            List<ReportViewModel> vmList = new List<ReportViewModel>();
            foreach (var d in details)
            {
                Stationery s = stationeryService.FindStationeryByItemCode(d.itemCode);

                ReportViewModel vm = new ReportViewModel();
                vm.Stationery = s;
                vm.ItemCode = d.itemCode;
                vm.Qty = d.qty;
                vm.CategoryName = s.Category.categoryName;

                vmList.Add(vm);
            }
            return vmList;
        }
    }
}