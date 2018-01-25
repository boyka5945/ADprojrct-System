using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Utilities;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.DAO
{
    public class ChartDAO : IChartDAO
    {
        public void RetrieveQty(DateTime? date)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                var purchaseRecords = (from x in entity.Purchase_Order_Records
                                       where x.date == date
                                       select x).ToList();

                List<Purchase_Detail> details = new List<Purchase_Detail>();

                foreach (var record in purchaseRecords)
                {
                    details.AddRange(record.Purchase_Detail);
                }

                var results = from d in details
                              group d.Stationery by d.itemCode into g
                              select new { ItemCode = g.Key, Items = g.ToList() };

            }

        }

        public List<ReportViewModel> RetrieveQty()
        {
            using (StationeryModel entity = new StationeryModel())
            {
                DateTime date = DateTime.Parse("2018-01-12");

                var purchaseRecords = (from x in entity.Purchase_Order_Records
                                       where x.date == date
                                       select x).ToList();

                List<Purchase_Detail> details = new List<Purchase_Detail>();

                foreach (var record in purchaseRecords)
                {
                    details.AddRange(record.Purchase_Detail);
                }

                List<ReportViewModel> vmList = new List<ReportViewModel>();
                foreach (var d in details)
                {
                    ReportViewModel vm = new ReportViewModel();
                    vm.Stationery = d.Stationery;
                    vm.ItemCode = d.itemCode;
                    vm.Qty = d.qty;
                    vm.CategoryName = d.Stationery.Category.categoryName;

                    //bool contain = false;

                    //foreach(var i in vmList)
                    //{
                    //    if(i.ItemCode == vm.ItemCode)
                    //    {
                    //        i.Qty += vm.Qty;
                    //        contain = true;
                    //        break;
                    //    }
                    //}

                    //if(!contain)
                    //{
                    vmList.Add(vm);
                    //}                   
                }

                return vmList;
            }



        }
    }
}