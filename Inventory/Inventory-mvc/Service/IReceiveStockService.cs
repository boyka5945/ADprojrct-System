﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
namespace Inventory_mvc.Service
{
    public interface IReceiveStockService
    {
        List<Purchase_Detail> GetAllPurchaseDetail();
    }
}