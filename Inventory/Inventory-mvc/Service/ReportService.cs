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

        // TODO - REMOVE THIS METHOD
        public void GenerateRandomDataForRequisitionRecords()
        {
            List<User> users = new List<User>();
            users.AddRange(userService.FindUsersByRole(3));
            users.AddRange(userService.FindUsersByRole(4));
            users.AddRange(userService.FindUsersByRole(7));
            Random userR = new Random(50);

            List<string> items = stationeryService.GetListOfItemCodes();
            Random stationeryR = new Random(888);

            string[] status = { RequisitionStatus.PENDING_APPROVAL, RequisitionStatus.APPROVED_PROCESSING, RequisitionStatus.REJECTED, RequisitionStatus.PARTIALLY_FULFILLED, RequisitionStatus.COLLECTED };
            Random statusR = new Random(75);

            Random detailR = new Random(16);
            Random quantityR = new Random(115);
            Random dailyR = new Random(369);

            Random dayR = new Random(4568);

            var startTime = DateTime.Parse("2017-01-01");
            var endTime = startTime.AddYears(1);
            endTime = endTime.AddMonths(1);

            while (startTime <= endTime)
            {
                int no_of_record = dailyR.Next(8, 15);

                for (int i = 0; i <= no_of_record; i++)
                {
                    int index = userR.Next(users.Count);
                    string requesterID = users.ElementAt(index).userID;

                    Requisition_Record record = new Requisition_Record();
                    record.status = status[statusR.Next(status.Length)];
                    record.Requisition_Detail = new List<Requisition_Detail>();

                    int number_of_detail = detailR.Next(1, 11);
                    for (int j = 0; j < number_of_detail; j++)
                    {
                        Requisition_Detail d = new Requisition_Detail();
                        d.itemCode = items.ElementAt(stationeryR.Next(items.Count));
                        d.qty = quantityR.Next(1, 10);
                        d.allocatedQty = 0;
                        d.fulfilledQty = 0;

                        bool contain = false;

                        foreach (var item in record.Requisition_Detail)
                        {
                            if (item.itemCode == d.itemCode)
                            {
                                item.qty += d.qty;
                                contain = true;
                                break;
                            }
                        }

                        if (!contain)
                        {
                            record.Requisition_Detail.Add(d);
                        }

                    }

                    requisitionService.GenerateRandomRequisition(record, requesterID, startTime);
                }
                int day = dayR.Next(1, 4);
                startTime = startTime.AddDays(day);
            }
        }

        // TODO - REMOVE THIS METHOD
        public void GenerateRandomDataForPurchaseRecords()
        {
            List<User> clerks = new List<User>();
            clerks.AddRange(userService.FindUsersByRole(7));
            Random userR = new Random(50);

            List<Supplier> suppliers = supplierSerivce.GetSupplierList();
            Random supplierR = new Random(651);

            string[] status = { "incomplete", "delivered", "partially delivered"};
            Random statusR = new Random(75);

            Random stationeryR = new Random(888);
            Random detailR = new Random(16);
            Random quantityR = new Random(115);
            Random dailyR = new Random(369);
            Random dayR = new Random(4568);

            var startTime = DateTime.Parse("2017-01-01");
            var endTime = startTime.AddYears(1);
            endTime = endTime.AddMonths(1);

            while (startTime <= endTime)
            {
                if(startTime.Year == 2017)
                {
                    status = new string[] { "delivered" };
                }

                int no_of_record = dailyR.Next(1, 5);

                HashSet<Supplier> orderTo = new HashSet<Supplier>();

                for (int i = 0; i <= no_of_record; i++)
                {
                    orderTo.Add(suppliers.ElementAt(supplierR.Next(suppliers.Count))); // get random supplier from list
                }

                foreach(Supplier s in orderTo)
                {
                    Purchase_Order_Record record = new Purchase_Order_Record();
                    record.clerkID = clerks.ElementAt(userR.Next(clerks.Count)).userID;
                    record.date = startTime;
                    record.expectedDeliveryDate = record.date.AddDays(14);
                    record.status = status[statusR.Next(status.Length)];
                    record.supplierCode = s.supplierCode;
                    record.Purchase_Detail = new List<Purchase_Detail>();

                    List<Stationery> supplyList = new List<Stationery>();
                    supplyList.AddRange(s.Stationeries);
                    supplyList.AddRange(s.Stationeries1);
                    supplyList.AddRange(s.Stationeries2);

                    int no_of_items = quantityR.Next(10, 21);

                    for(int i = 0; i < no_of_items; i++)
                    {
                        Stationery orderItem = supplyList.ElementAt(stationeryR.Next(supplyList.Count));

                        Purchase_Detail detail = new Purchase_Detail();
                        detail.itemCode = orderItem.itemCode;
                        detail.qty = quantityR.Next(orderItem.reorderQty, orderItem.reorderQty * 3);
                        detail.price = orderItem.price;

                        if(record.status == "delivered")
                        {
                            detail.fulfilledQty = detail.qty;
                        }
                        else if(record.status == "partially delivered")
                        {
                            detail.fulfilledQty = quantityR.Next(1, detail.qty);
                        }
                        else
                        {
                            detail.fulfilledQty = 0;
                        }

                        bool contain = false;
                        foreach(var d in record.Purchase_Detail)
                        {
                            if(d.itemCode == detail.itemCode)
                            {
                                d.qty += detail.qty;
                                contain = true;
                                break;
                            }
                        }
                        if(!contain)
                        {
                            record.Purchase_Detail.Add(detail);
                        }
                    }

                    purchaseService.AddNewPurchaseOrder(record);
                }

                int days = dayR.Next(10, 15); // about 2 weeks
                startTime = startTime.AddDays(days);
            }
        }



    }
}