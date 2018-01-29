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
        [Display(Name = "Collection Point ID")]
        public int collectionPointID { get; set; }

        [Required]
        
        [StringLength(50)]
        [Display(Name = "Collection Point")]
        public string collectionPointName { get; set; }

    }
}