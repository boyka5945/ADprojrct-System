using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.DAO
{
    interface IAdjustmentVoucherDAO
    {

        Adjustment_Voucher_Record FindByVoucherID(int voucherID);

        bool AddNewAdjustmentVoucher(Adjustment_Voucher_Record voucher);

        int UpdateAdjustmentVoucherInfo(Adjustment_Voucher_Record voucher);

        List<Adjustment_Voucher_Record> GetVouchersByCriteria(string status, string sortOrder);
        int GetPendingVoucherCount();
    }
}
