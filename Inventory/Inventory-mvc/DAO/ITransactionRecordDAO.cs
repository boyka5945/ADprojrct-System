using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public interface ITransactionRecordDAO
    {
        Transaction_Record FindByTransactionNo(int transactionNo);

        Boolean AddNewTransaction(Transaction_Record transaction_record);

        List<Transaction_Detail> GetTransaciontDetailsByCriteria(int year, int month, string itemCode);
    }
}