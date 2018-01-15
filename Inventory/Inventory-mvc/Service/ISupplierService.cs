﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.Service
{
    public interface ISupplierService
    {
        List<Supplier> GetAllSuppliers();
    }
}