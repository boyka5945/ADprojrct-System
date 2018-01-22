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
                context.Inventory_Status_Records.AddRange(records);
                if(context.SaveChanges() == 0)
                {
                    return false;
                }

                return true;                
            }
        }

        public List<DateTime> GetAllStockCheckDate()
        {
            // TODO: WRITE METHOD
            throw new NotImplementedException();
        }

        public Inventory_Status_Record FindInventoryStatusRecordByDate(DateTime date)
        {
            // TODO: WRITE METHOD
            throw new NotImplementedException();

        }
    }
}