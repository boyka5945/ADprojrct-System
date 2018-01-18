﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using System.ComponentModel.DataAnnotations;

namespace Inventory_mvc.ViewModel
{
    public class RaiseRequisitionViewModel
    {
        [Key]
        [Required]
        public string ItemCode { get; set; }

        public string Description { get; set; }

        public string UOM { get; set; }

        [Required]
        [Range(1,9999)]
        public int Quantity { get; set; }


    }
}