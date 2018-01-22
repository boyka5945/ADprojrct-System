using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public class InventoryStatusRecordDAO : IInventoryStatusRecordDAO
    {
        public bool AddNewInventoryStatusRecords(List<Inventory_Status_Record> records)
        {
            using (StationeryModel context = new StationeryModel())
            {
                try
                {
                    context.Inventory_Status_Records.AddRange(records);
                    if (context.SaveChanges() == 0)
                    {
                        return false;
                    }

                    return true;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        public List<DateTime> GetAllStockCheckDate()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from r in context.Inventory_Status_Records
                        select r.date).Distinct().ToList();
            }
        }

        public List<Inventory_Status_Record> FindInventoryStatusRecordsByDate(DateTime date)
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from r in context.Inventory_Status_Records
                        where r.date == date
                        select r).ToList();
            }
        }
    }
}