using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using System.Data.Entity;

namespace Inventory_mvc.DAO
{
    public class AdjustmentVoucherDAO : IAdjustmentVoucherDAO
    {
        // TODO : REMOVE UNNESESSARY METHODS
        public bool AddNewAdjustmentVoucher(Adjustment_Voucher_Record voucher)
        {
            using (StationeryModel context = new StationeryModel())
            {
                try
                {
                    context.Adjustment_Voucher_Records.Add(voucher);
                    context.SaveChanges();
                    return true;
                }
                catch(Exception e)
                {
                    return false;
                }
            }
        }

        public bool DeleteAdjustmentVoucher(int voucherID)
        {
            throw new NotImplementedException();
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

        public List<Adjustment_Voucher_Record> GetAllAdjustmentVoucher()
        {
            throw new NotImplementedException();
        }

        public int UpdateAdjustmentVoucherInfo(Adjustment_Voucher_Record voucher)
        {
            throw new NotImplementedException();
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
                    case "Requester":
                        vouchers = vouchers.OrderBy(v => v.User.name);
                        break;
                    case "requester_desc":
                        vouchers = vouchers.OrderByDescending(v => v.User.name);
                        break;
                    default:
                        vouchers = vouchers.OrderBy(v => v.voucherID);
                        break;
                }

                return vouchers.ToList();
            }
        }
    }
}