using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public interface ITransactionRecordDAO
    {
        // TODO : REMOVE UNNESESSARY METHODS
        List<Transaction_Record> GetAllTransaction();

        Transaction_Record FindByTransactionNO(int TransactionNO);

        Boolean AddNewTransaction(Transaction_Record transaction_record);

        int UpdateTransactionInfo(Transaction_Record transaction_record);

        Boolean DeleteTransaction(int TransactionNO);

        List<Transaction_Detail> GetTransaciontDetailsByCriteria(int year, int month, string itemCode);
    }
}