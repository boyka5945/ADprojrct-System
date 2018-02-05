using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.DAO;
using System.Web.Mvc;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using Inventory_mvc.Utilities;

namespace Inventory_mvc.Service
{
    public class ReportService : IReportService
    {
        IReportDAO reportDAO = new ReportDAO();
        IStationeryService stationeryService = new StationeryService();
        IUserService userService = new UserService();
        IRequisitionRecordService requisitionService = new RequisitionRecordService();
        IPurchaseOrderService purchaseService = new PurchaseOrderService();
        ISupplierService supplierSerivce = new SupplierService();

        public List<ReportViewModel> GetItemRequestTrend(string categoryID, string itemCode, int[] years)
        {
            List<Requisition_Detail> details = reportDAO.GetApprovedRequisitionDetailsByCriteria(categoryID, itemCode, null, years, null);
            List<ReportViewModel> vmList = new List<ReportViewModel>();
            vmList.AddRange(ConvertToReportViewModel(details));
            return vmList;
        }

        public List<ReportViewModel> GetApprovedRequisitionDetialsBasedCriteria(string categoryID, string itemCode, string deptCode, int[] years, int[] months)
        {
            List<Requisition_Detail> details = reportDAO.GetApprovedRequisitionDetailsByCriteria(categoryID, itemCode, deptCode, years, months);
            List<ReportViewModel> vmList = new List<ReportViewModel>();
            vmList.AddRange(ConvertToReportViewModel(details));
            return vmList;
        }

        public List<ReportViewModel> GetReorderAmountBasedOnCriteria(string categoryID, string itemCode, string supplierCode, int[] years, int[] months)
        {
            List<Purchase_Detail> details = reportDAO.GetPurchaseDetailsByCriteria(categoryID, itemCode, supplierCode, years, months);
            List<ReportViewModel> vmList = new List<ReportViewModel>();
            vmList.AddRange(ConvertToReportViewModel(details));
            return vmList;
        }


        public List<ReportViewModel> GetReorderAmountBasedOnCriteria(string categoryID, string itemCode, string supplierCode, string[] yearAndMonths)
        {
            List<Purchase_Detail> details = reportDAO.GetPurchaseDetailsByCriteria(categoryID, itemCode, supplierCode, yearAndMonths);
            List<ReportViewModel> vmList = new List<ReportViewModel>();
            vmList.AddRange(ConvertToReportViewModel(details));
            return vmList;
        }



        private ReportViewModel ConvertToReportViewModel(Requisition_Detail detail)
        {
            Stationery stationery = stationeryService.FindStationeryByItemCode(detail.itemCode);
            Requisition_Record record = detail.Requisition_Record;
            
            ReportViewModel vm = new ReportViewModel();
            vm.CategoryName = stationery.Category.categoryName;
            vm.Cost = stationery.price;
            vm.ItemCode = detail.itemCode;
            vm.ItemDescription = stationery.description;
            vm.Month = record.requestDate.Value.Month;
            vm.RequestQuantity = (detail.qty == null) ? 0 : (int)detail.qty;
            vm.RequesterDepartment = userService.FindDeptCodeByID(record.requesterID);
            vm.Year = record.requestDate.Value.Year;
            vm.Status = record.status;

            return vm;
        }


        private List<ReportViewModel> ConvertToReportViewModel(List<Requisition_Detail> details)
        {
            List<ReportViewModel> vmList = new List<ReportViewModel>();
            foreach(var d in details)
            {
                vmList.Add(ConvertToReportViewModel(d));
            }

            return vmList;
        }


        private ReportViewModel ConvertToReportViewModel(Purchase_Detail detail)
        {
            Stationery stationery = stationeryService.FindStationeryByItemCode(detail.itemCode);
            Purchase_Order_Record record = detail.Purchase_Order_Record;

            ReportViewModel vm = new ReportViewModel();
            vm.CategoryName = stationery.Category.categoryName;
            vm.Cost = detail.price;
            vm.ItemCode = stationery.itemCode;
            vm.ItemDescription = stationery.description;
            vm.Month = record.date.Month;
            vm.OrderQuantity = detail.qty;
            vm.Status = record.status;
            vm.Supplier = record.supplierCode;
            vm.Year = record.date.Year;

            return vm;
        }


        private List<ReportViewModel> ConvertToReportViewModel(List<Purchase_Detail> details)
        {
            List<ReportViewModel> vmList = new List<ReportViewModel>();
            foreach (var d in details)
            {
                vmList.Add(ConvertToReportViewModel(d));
            }

            return vmList;
        }


        public int GetEarliestYear()
        {
            return reportDAO.GetEarliestYear();
        }

        public List<int> GetSelectableYears(int baseYear)
        {
            List<int> years = new List<int>();
            int thisYear = DateTime.Today.Year;

            for(int i = baseYear; i <= thisYear; i++)
            {
                years.Add(i);
            }

            return years;
        }

        public List<int> GetSelectableMonths(int year)
        {
            int currentMonths = DateTime.Today.Month;
            int currentYear = DateTime.Today.Year;

            List<int> months = new List<int>();

            if(year < currentYear)
            {
                for(int i = 1; i <= 12; i++)
                {
                    months.Add(i);
                }
            }
            else if (year == currentYear)
            {
                for(int i = 1; i <= currentMonths; i++)
                {
                    months.Add(i);
                }
            }

            return months;
        }




    }
}