using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using System.Data.Entity;

namespace Inventory_mvc.DAO
{
    public class TransactionRecordDAO : ITransactionRecordDAO
    {
        public bool AddNewTransaction(Transaction_Record transaction_record)
        {
            using (StationeryModel context = new StationeryModel())
            {
                try
                {
                    context.Transaction_Records.Add(transaction_record);
                    context.SaveChanges();
                    return true;
                }
                catch(Exception e)
                {
                    return false;
                }
            }
        }

        public Transaction_Record FindByTransactionNo(int transactionNo)
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from r in context.Transaction_Records
                        where r.transactionNo == transactionNo
                        select r).Include(r => r.Transaction_Details).FirstOrDefault();
            }
        }


        public List<Transaction_Detail> GetTransaciontDetailsByCriteria(int year, int month, string itemCode)
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from r in context.Transaction_Details
                        where r.Transaction_Record.date.Value.Year == year
                        & r.Transaction_Record.date.Value.Month == month
                        & r.itemCode == itemCode
                        select r).Include(r => r.Transaction_Record).ToList();
            }
        }

    }
}