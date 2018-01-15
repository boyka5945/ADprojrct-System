namespace Inventory_mvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rolePermission")]
    public partial class rolePermission
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int roleID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int permissionID { get; set; }

        [StringLength(50)]
        public string description { get; set; }

        public virtual permissionInfo permissionInfo { get; set; }

        public virtual roleInfo roleInfo { get; set; }
    }
}
