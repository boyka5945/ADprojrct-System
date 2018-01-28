using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using System.Data.Entity;

namespace Inventory_mvc.DAO
{
    public class ReportDAO : IReportDAO
    {
        public List<Requisition_Detail> GetRequisitionDetailsByItemCodeAndYear(string itemCode, int[] years)
        {
            using (StationeryModel context = new StationeryModel())
            {
                var details = (from d in context.Requisition_Detail
                               where d.itemCode == itemCode
                               select d);

                details = (from d in details
                           where years.Contains(d.Requisition_Record.requestDate.Value.Year)
                           select d);

                return details.Include(d => d.Requisition_Record).ToList();                                        
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