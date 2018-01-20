using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.Service
{
    public interface ITransactionRecordService
    {
        int[] GetSelectableTransactionYear(int year);

        int[] GetSelectableTransactionMonth(int month);

        List<Transaction_Detail> GetTransaciontDetailsByCriteria(int year, int month, string itemCode);

    }
}