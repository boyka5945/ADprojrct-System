namespace Inventory_mvc.Models
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
            Stationeries = new HashSet<Stationery>();
            Stationeries1 = new HashSet<Stationery>();
            Stationeries2 = new HashSet<Stationery>();
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
        public virtual ICollection<Stationery> Stationeries { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stationery> Stationeries1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stationery> Stationeries2 { get; set; }
    }
}
