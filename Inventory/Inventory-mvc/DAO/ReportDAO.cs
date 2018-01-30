using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using Inventory_mvc.Utilities;
using System.Data.Entity;

namespace Inventory_mvc.DAO
{
    public class ReportDAO : IReportDAO
    {
        private string[] criteria = { RequisitionStatus.PENDING_APPROVAL, RequisitionStatus.REJECTED };


        public List<Requisition_Detail> GetApprovedRequisitionDetailsByCriteria(string categoryID, string itemCode, string deptCode, int[] years, int[] months)
        {
            using (StationeryModel context = new StationeryModel())
            {
                var details = (from d in context.Requisition_Detail select d);

                if(categoryID != "-1") // -1 => retrieve from all category
                {
                    details = (from d in details where d.Stationery.categoryID.ToString() == categoryID select d);
                }

                if(itemCode != "-1") // -1 => retrieve all stationery from the same category
                {
                    details = (from d in details where d.itemCode == itemCode select d);
                }

                if(!String.IsNullOrEmpty(deptCode))
                {
                    details = (from d in details
                               where d.Requisition_Record.User.departmentCode == deptCode
                               select d);
                }

                if(years != null)
                {
                    details = (from d in details
                               where years.Contains(d.Requisition_Record.requestDate.Value.Year)
                               select d);
                }

                if (months != null)
                {
                    details = (from d in details
                               where months.Contains(d.Requisition_Record.requestDate.Value.Month)
                               select d);
                }

                details = (from d in details
                           where !criteria.Contains(d.Requisition_Record.status)
                           select d);

                return details.Include(d => d.Requisition_Record).ToList();
            }

        }

        public int GetEarliestYear()
        {
            using (StationeryModel context = new StationeryModel())
            {
                var year = (from r in context.Requisition_Records
                             select r.requestDate).Min();

                return year.Value.Year;
            }
        }

        public List<Purchase_Detail> GetPurchaseDetailsByCriteria(string categoryID, string itemCode, string supplierCode, int[] years, int[] months)
        {
            using (StationeryModel context = new StationeryModel())
            {
                var details = (from d in context.Purchase_Detail select d);

                if(categoryID != "-1") // -1 => retrieve from all category
                {
                    details = (from d in details where d.Stationery.categoryID.ToString() == categoryID select d);
                }

                if (itemCode != "-1") // -1 => retrieve all stationery from the same category
                {
                    details = (from d in details where d.itemCode == itemCode select d);
                }

                if (!String.IsNullOrEmpty(supplierCode))
                {
                    details = (from d in details
                               where d.Purchase_Order_Record.supplierCode == supplierCode
                               select d);
                }

                if (years != null)
                {
                    details = (from d in details
                               where years.Contains(d.Purchase_Order_Record.date.Year)
                               select d);
                }

                if (months != null)
                {
                    details = (from d in details
                               where months.Contains(d.Purchase_Order_Record.date.Month)
                               select d);
                }

               
                return details.Include(d => d.Purchase_Order_Record).ToList();
            }
        }


        public List<Purchase_Detail> GetPurchaseDetailsByCriteria(string categoryID, string itemCode, string supplierCode, string[] yearAndMonths)
        {
            using (StationeryModel context = new StationeryModel())
            {
                var details = (from d in context.Purchase_Detail select d);

                if (categoryID != "-1") // -1 => retrieve from all category
                {
                    details = (from d in details where d.Stationery.categoryID.ToString() == categoryID select d);
                }

                if (itemCode != "-1") // -1 => retrieve all stationery from the same category
                {
                    details = (from d in details where d.itemCode == itemCode select d);
                }

                if (!String.IsNullOrEmpty(supplierCode))
                {
                    details = (from d in details
                               where d.Purchase_Order_Record.supplierCode == supplierCode
                               select d);
                }

                List<Purchase_Detail> resultList = new List<Purchase_Detail>();

                foreach(string ym in yearAndMonths) // eg: ["5-2017", "12-2017", "1-2018"]
                {
                    string month = ym.Split('-')[0]; // eg: 5, 12, 1
                    string year = ym.Split('-')[1]; // eg: 2017, 2017, 2018

                    List<Purchase_Detail> results = new List<Purchase_Detail>();
                    results = (from d in details
                               where d.Purchase_Order_Record.date.Month.ToString() == month
                               && d.Purchase_Order_Record.date.Year.ToString() == year
                               select d).Include(d => d.Purchase_Order_Record).ToList();

                    resultList.AddRange(results);
                }

                return resultList;
            }

        }

    }
}