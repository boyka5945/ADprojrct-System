namespace Inventory_mvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Transaction Record")]
    public partial class Transaction_Record
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Transaction_Record()
        {
            Transaction_Details = new HashSet<Transaction_Detail>();
        }

        [Key]
        [Display(Name = "Transaction Number")]
        public int transactionNo { get; set; }

        [StringLength(50)]
        [Display(Name = "Clerk ID")]
        public string clerkID { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Date")]
        public DateTime? date { get; set; }

        [StringLength(50)]
        [Display(Name = "Transaction Type")]
        public string type { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "Transaction Details")]
        public virtual ICollection<Transaction_Detail> Transaction_Details { get; set; }

        public virtual User User { get; set; }
    }
}
