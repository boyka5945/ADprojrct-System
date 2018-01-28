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

        public List<Requisition_Detail> GetApprovedRequisitionDetailsByItemCodeAndYear(string itemCode, int[] years)
        {
            using (StationeryModel context = new StationeryModel())
            {
                var details = (from d in context.Requisition_Detail
                               where d.itemCode == itemCode
                               select d);

                details = (from d in details
                           where years.Contains(d.Requisition_Record.requestDate.Value.Year)
                           && !criteria.Contains(d.Requisition_Record.status)
                           select d);

                return details.Include(d => d.Requisition_Record).ToList();                                        
            }
        }


        public List<Requisition_Detail> GetApprovedRequisitionDetailsByCriteria(string categoryID, string itemCode, int[] years)
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

                details = (from d in details
                           where years.Contains(d.Requisition_Record.requestDate.Value.Year)
                           && !criteria.Contains(d.Requisition_Record.status)
                           select d);

                return details.Include(d => d.Requisition_Record).ToList();
            }

        }


        public List<Requisition_Detail> GetApprovedRequisitionDetailsOfYear(int year)
        {
            using (StationeryModel context = new StationeryModel())
            {
                var details = (from d in context.Requisition_Detail
                               where d.Requisition_Record.requestDate.Value.Year == year
                               && !criteria.Contains(d.Requisition_Record.status)
                               select d);

                return details.Include(d => d.Requisition_Record).ToList();
            }
        }

        public List<Requisition_Detail> GetApprovedRequisitionDetialsBasedOnYearAndMonth(int year, int month)
        {
            using (StationeryModel context = new StationeryModel())
            {
                var details = (from d in context.Requisition_Detail
                               where d.Requisition_Record.requestDate.Value.Year == year
                               && d.Requisition_Record.requestDate.Value.Month == month
                               && !criteria.Contains(d.Requisition_Record.status)
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

        public List<Purchase_Detail> RetrieveQty(DateTime ds, DateTime de)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                DateTime dateStart = ds;
                DateTime dateEnd = de;
                
                var purchaseRecords = (from x in entity.Purchase_Order_Records
                                       where x.date >= dateStart && x.date <= dateEnd
                                       select x).Include(x => x.Purchase_Detail).ToList();

                List<Purchase_Detail> details = new List<Purchase_Detail>();

                foreach (var record in purchaseRecords)
                {
                    details.AddRange(record.Purchase_Detail);
                }



                return details; 
            }
        }

        public List<Purchase_Detail> RetrieveQtyByEachSupplier(DateTime ds, DateTime de,string suppCode)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                // TODO - FIX DATE
                DateTime dateStart = ds;
                DateTime dateEnd = de;

                var purchaseRecords = (from x in entity.Purchase_Order_Records
                                       where x.date >= dateStart && x.date <= dateEnd && x.supplierCode == suppCode
                                       select x).Include(x => x.Purchase_Detail).ToList();

                List<Purchase_Detail> details = new List<Purchase_Detail>();

                foreach (var record in purchaseRecords)
                {
                    details.AddRange(record.Purchase_Detail);
                }



                return details;
            }
        }
    }
}