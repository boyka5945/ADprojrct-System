namespace Inventory_mvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Stationery")]
    public partial class Stationery
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Stationery()
        {
            Inventory_Status_Record = new HashSet<Inventory_Status_Record>();
            Purchase_Detail = new HashSet<Purchase_Detail>();
            Requisition_Detail = new HashSet<Requisition_Detail>();
            Transaction_Details = new HashSet<Transaction_Detail>();
            Voucher_Details = new HashSet<Voucher_Detail>();
        }

        [Key]
        [StringLength(50)]
        public string itemCode { get; set; }

        public int categoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string description { get; set; }

        public int reorderLevel { get; set; }

        public int reorderQty { get; set; }

        [Required]
        [StringLength(50)]
        public string unitOfMeasure { get; set; }

        public int stockQty { get; set; }

        [Required]
        [StringLength(50)]
        public string location { get; set; }

        [Required]
        [StringLength(50)]
        public string firstSupplierCode { get; set; }

        [StringLength(50)]
        public string secondSupplierCode { get; set; }

        [StringLength(50)]
        public string thirdSupplierCode { get; set; }

        public decimal price { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventory_Status_Record> Inventory_Status_Record { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Purchase_Detail> Purchase_Detail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requisition_Detail> Requisition_Detail { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual Supplier Supplier1 { get; set; }

        public virtual Supplier Supplier2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction_Detail> Transaction_Details { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Voucher_Detail> Voucher_Details { get; set; }
    }
}
