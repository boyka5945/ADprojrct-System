using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using Inventory_mvc.Utilities;
using System.Data.Entity;

namespace Inventory_mvc.DAO
{
    public class AdjustmentVoucherDAO : IAdjustmentVoucherDAO
    {
        public bool AddNewAdjustmentVoucher(Adjustment_Voucher_Record voucher)
        {
            using (StationeryModel context = new StationeryModel())
            {
                context.Adjustment_Voucher_Records.Add(voucher);
                context.SaveChanges();
                return true;
            }
        }


        public Adjustment_Voucher_Record FindByVoucherID(int voucherID)
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from r in context.Adjustment_Voucher_Records
                        where r.voucherID == voucherID
                        select r).Include(r => r.Voucher_Details).FirstOrDefault();
            }
        }


        public int UpdateAdjustmentVoucherInfo(Adjustment_Voucher_Record voucher)
        {
            using (StationeryModel context = new StationeryModel())
            {
                try
                {
                    Adjustment_Voucher_Record record = (from r in context.Adjustment_Voucher_Records
                                                        where r.voucherID == voucher.voucherID
                                                        select r).First();

                    record.approvalDate = voucher.approvalDate;
                    record.authorisingStaffID = voucher.authorisingStaffID;
                    record.handlingStaffID = voucher.handlingStaffID;
                    record.issueDate = voucher.issueDate;
                    record.remarks = voucher.remarks;
                    record.status = voucher.status;

                    return context.SaveChanges();
                }
                catch(Exception e)
                {
                    string errorMessage = String.Format("Error occurs when updating adjustment voucher. ({0})", e.Message);
                    throw new Exception(errorMessage);
                }
            }
        }

        public List<Adjustment_Voucher_Record> GetVouchersByCriteria(string status, string sortOrder)
        {
            using (StationeryModel context = new StationeryModel())
            {
                var vouchers = from v in context.Adjustment_Voucher_Records
                               where v.status == status
                               select v;

                switch (sortOrder)
                {
                    case "number_desc":
                        vouchers = vouchers.OrderByDescending(v => v.voucherID);
                        break;
                    case "Voucher Number":
                        vouchers = vouchers.OrderBy(v => v.voucherID);
                        break;
                    case "Issued By":
                        vouchers = vouchers.OrderBy(v => v.User.name);
                        break;
                    case "issued_by_desc":
                        vouchers = vouchers.OrderByDescending(v => v.User.name);
                        break;
                    default:
                        vouchers = vouchers.OrderBy(v => v.voucherID);
                        break;
                }

                return vouchers.ToList();
            }
        }

        public int GetPendingVoucherCount()
        {
            using (StationeryModel context = new StationeryModel())
            {
                return (from v in context.Adjustment_Voucher_Records
                        where v.status == AdjustmentVoucherStatus.PENDING
                        select v).Count();
            }
        }
    }
}