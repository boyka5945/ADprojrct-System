using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.DAO;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.Service
{
    public class TransactionRecordService : ITransactionRecordService
    {
        ITransactionRecordDAO transactionRecordDAO = new TransactionRecordDAO();

        public int[] GetSelectableTransactionYear(int year)
        {
            int baseYear = 2017;

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

            List<int> months = new List<int>();

            for(int i = 1; i<= 12; i++)
            {
                months.Add(i);
            }


            return months.ToArray();
        }

        public List<Transaction_Detail> GetTransaciontDetailsByCriteria(int year, int month, string itemCode)
        {
            return transactionRecordDAO.GetTransaciontDetailsByCriteria(year, month, itemCode);
        }

        public List<ItemTransactionRecordViewModel> GetTransaciontDetailsViewModelByCriteria(int selectedYear, int selectedMonth, string id)
        {
            List<Transaction_Detail> records = GetTransaciontDetailsByCriteria(selectedYear, selectedMonth, id);
            List<ItemTransactionRecordViewModel> vmList = new List<ItemTransactionRecordViewModel>();

            foreach (var r in records)
            {
                ItemTransactionRecordViewModel vm = new ItemTransactionRecordViewModel();

                vm.TransactionNo = r.transactionNo;
                vm.TransactionDate = (DateTime)r.Transaction_Record.date;
                vm.ItemCode = r.itemCode;
                vm.Quantity = r.adjustedQty;
                vm.BalanceQty = r.balanceQty;
                vm.TransactionType = r.Transaction_Record.type;
                vm.Remarks = r.remarks;

                vmList.Add(vm);
            }

            return vmList;
        }

        public bool AddNewTransactionRecord(Transaction_Record record)
        {
            if(transactionRecordDAO.AddNewTransaction(record))
            {
                return true;
            }
            else
            {
                throw new Exception("Error writing new transaction record into database");
            }
        }
    }
}