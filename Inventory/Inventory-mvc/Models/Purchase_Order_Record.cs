namespace Inventory_mvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Purchase Order Record")]
    public partial class Purchase_Order_Record
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Purchase_Order_Record()
        {
            Purchase_Details = new HashSet<Purchase_Detail>();
        }

        [Key]
        public int orderNo { get; set; }

        [Required]
        [StringLength(50)]
        public string supplierCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime date { get; set; }

        [Required]
        [StringLength(50)]
        public string clerkID { get; set; }

        [Required]
        [StringLength(50)]
        public string status { get; set; }

        [Column(TypeName = "date")]
        public DateTime expectedDeliveryDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Purchase_Detail> Purchase_Details { get; set; }

        public virtual User User { get; set; }
    }
}
