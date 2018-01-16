using Inventory_mvc.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_mvc.Service
{
    public class IStationeryService
    {
        List<StationeryViewModel> GetAllStationery();

        public static implicit operator IStationeryService(StationeryService v)
        {
            throw new NotImplementedException();
        }


        //  SupplierViewModel FindByStationeryCode(string supplierCode);

        //  bool UpdateStationeryInfo(SupplierViewModel supplierVM);


        /// <summary>
        /// Return true if the code has already been used
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        //  bool isExistingCode(string itemCode);
    }
}