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
        // TODO : IMPLEMENT NESSESSARY METHOD
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

        public bool DeleteTransaction(int TransactionNO)
        {
            throw new NotImplementedException();
        }

        public Transaction_Record FindByTransactionNO(int TransactionNO)
        {
            throw new NotImplementedException();
        }

        public List<Transaction_Record> GetAllTransaction()
        {
            throw new NotImplementedException();
        }

        public int UpdateTransactionInfo(Transaction_Record transaction_record)
        {
            throw new NotImplementedException();
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