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
            Transaction_Details = new HashSet<Transaction_Details>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int transactionNo { get; set; }

        [StringLength(50)]
        public string clerkID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? date { get; set; }

        [StringLength(50)]
        public string type { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction_Details> Transaction_Details { get; set; }

        public virtual User User { get; set; }
    }
}
