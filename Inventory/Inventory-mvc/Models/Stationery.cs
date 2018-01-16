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
            Purchase_Details = new HashSet<Purchase_Details>();
            Requisition_Details = new HashSet<Requisition_Details>();
            Transaction_Details = new HashSet<Transaction_Details>();
            Voucher_Details = new HashSet<Voucher_Details>();
        }

        [Key]
        [StringLength(50)]
        public string itemCode { get; set; }

        public int categoryID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Description")]
        public string description { get; set; }

        public int reorderLevel { get; set; }

        public int reorderQty { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "UOM")]
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
        public virtual ICollection<Purchase_Details> Purchase_Details { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requisition_Details> Requisition_Details { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual Supplier Supplier1 { get; set; }

        public virtual Supplier Supplier2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction_Details> Transaction_Details { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Voucher_Details> Voucher_Details { get; set; }
    }
}
