using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public interface IInventoryStatusRecord
    {
        
        List<Inventory_Status_Record> GetAllInventoryStatusRecord();
        Inventory_Status_Record FindInventoryStatusRecordByDate(DateTime date);
        Inventory_Status_Record FindInventoryStatusRecordByItem(string itemCode);
        Boolean AddNewInventoryStatusRecord(Inventory_Status_Record record);
        int UpdateInventoryStatusRecord(Inventory_Status_Record record);
        Boolean DeleteInventoryStatusRecord(Inventory_Status_Record record);
    }
}