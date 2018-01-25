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
        public List<Purchase_Detail> RetrieveQty()
        {
            using (StationeryModel entity = new StationeryModel())
            {
                // TODO - FIX DATE
                DateTime date = DateTime.Parse("2018-01-24");

                var purchaseRecords = (from x in entity.Purchase_Order_Records
                                       where x.date == date
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