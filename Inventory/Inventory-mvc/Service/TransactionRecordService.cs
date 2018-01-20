using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.DAO;

namespace Inventory_mvc.Service
{
    public class TransactionRecordService : ITransactionRecordService
    {
        ITransactionRecordDAO transactionRecordDAO = new TransactionRecordDAO();

        public int[] GetSelectableTransactionYear(int year)
        {
            int baseYear = 2018;

            List<int> years = new List<int>();

            for (int i = baseYear; i <= year; i++)
            {
                years.Add(i);
            }

            years.Reverse();

            return years.ToArray();
        }

        public int[] GetSelectableTransactionMonth(int month)
        {
            int baseMonth = 1;

            List<int> months = new List<int>();

            for (int i = baseMonth; i <= month; i++)
            {
                months.Add(i);
            }

            months.Reverse();

            return months.ToArray();
        }

        public List<Transaction_Detail> GetTransaciontDetailsByCriteria(int year, int month, string itemCode)
        {
            return transactionRecordDAO.GetTransaciontDetailsByCriteria(year, month, itemCode);
        }
    }
}