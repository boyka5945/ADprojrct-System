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
        [Display(Name = "Item Code")]
        public string itemCode { get; set; }

        [Display(Name = "Category ID")]
        public int categoryID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Description")]
        public string description { get; set; }

        [Display(Name = "Reorder Level")]
        public int reorderLevel { get; set; }

        [Display(Name = "Reorder Quantity")]
        public int reorderQty { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "UOM")]
        public string unitOfMeasure { get; set; }

        [Display(Name = "Stock Quantity")]
        public int stockQty { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Location")]
        public string location { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Supplier Code")]
        public string firstSupplierCode { get; set; }

        [StringLength(50)]
        [Display(Name = "Second Supplier Code")]
        public string secondSupplierCode { get; set; }

        [StringLength(50)]
        [Display(Name = "Third Supplier Code")]
        public string thirdSupplierCode { get; set; }

        [Display(Name = "Price")]
        public decimal price { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventory_Status_Record> Inventory_Status_Record { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Purchase_Detail> Purchase_Detail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requisition_Detail> Requisition_Detail { get; set; }

        public virtual Supplier Supplier { get; set; }

        [Display(Name = "First Supplier")]
        public virtual Supplier Supplier1 { get; set; }

        [Display(Name = "Second Supplier")]
        public virtual Supplier Supplier2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction_Detail> Transaction_Details { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Voucher_Detail> Voucher_Details { get; set; }
    }
}
