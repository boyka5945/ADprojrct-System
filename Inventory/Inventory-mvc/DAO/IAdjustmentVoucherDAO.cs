using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    interface IAdjustmentVoucher
    {
        List<Adjustment_Voucher_Record> GetAllAdjustmentVoucher();

        Adjustment_Voucher_Record FindByVoucherID(int voucherID);

        bool AddNewAdjustmentVoucher(Adjustment_Voucher_Record voucher);

        int UpdateAdjustmentVoucherInfo(Adjustment_Voucher_Record voucher);

        bool DeleteAdjustmentVoucher(int voucherID);
    }
}
