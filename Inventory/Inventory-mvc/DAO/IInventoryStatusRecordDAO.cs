using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public interface IInventoryStatusRecordDAO
    {
        bool AddNewInventoryStatusRecords(List<Inventory_Status_Record> records);

        List<DateTime> GetAllStockCheckDate();

        List<Inventory_Status_Record> FindInventoryStatusRecordsByDate(DateTime date);


    }
}