namespace InventoryWCF.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Supplier")]
    public partial class Supplier
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Supplier()
        {
            Stationery = new HashSet<Stationery>();
            Stationery1 = new HashSet<Stationery>();
            Stationery2 = new HashSet<Stationery>();
        }

        [Key]
        [StringLength(50)]
        public string supplierCode { get; set; }

        [Required]
        [StringLength(50)]
        public string GSTNo { get; set; }

        [Required]
        [StringLength(50)]
        public string supplierName { get; set; }

        [Required]
        [StringLength(50)]
        public string contactName { get; set; }

        public int phoneNo { get; set; }

        public int? faxNo { get; set; }

        [Required]
        [StringLength(200)]
        public string address { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stationery> Stationery { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stationery> Stationery1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stationery> Stationery2 { get; set; }
    }
}
