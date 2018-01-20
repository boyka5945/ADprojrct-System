using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public class AdjustmentVoucherDAO : IAdjustmentVoucherDAO
    {
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
            throw new NotImplementedException();
        }

        public List<Adjustment_Voucher_Record> GetAllAdjustmentVoucher()
        {
            throw new NotImplementedException();
        }

        public int UpdateAdjustmentVoucherInfo(Adjustment_Voucher_Record voucher)
        {
            throw new NotImplementedException();
        }
    }
}