using Inventory_mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_mvc.ViewModel
{  
    public class BrowseStationeryCatalogueViewModel
    {
        public BrowseStationeryCatalogueViewModel()
        {
            this.StationeryList = new List<Stationery>();
        }

        [Display(Name = "Description")]
        public string SearchDescription { get; set; }

        [Display(Name = "Category")]
        public string SearchCategoryID { get; set; }

        public List<Stationery> StationeryList { get; set; }
    }
}