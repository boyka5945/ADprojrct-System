using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_mvc.ViewModel
{
    public class UserViewModel
    {
        [Key]
        [Required]
        [StringLength(50)]
        [Display(Name = "UserID")]
        public string UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Contact No")]
        public int ContactNo { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Role")]
        public int Role { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Email")]
        public string UserEmail { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime? DelegationStart { get; set; }

        [Required]
        [Display(Name = "End date")]
        public DateTime? DelegationEnd { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
    }
}