using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_mvc.ViewModel
{
    public class CollectionPointViewModel
    {
        [Key]
        [Required]
        [Display(Name = "collectionPointID")]
        public int collectionPointID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Collection_Point")]
        public string collectionPointName { get; set; }

    }
}