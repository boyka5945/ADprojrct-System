using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_mvc.ViewModel
{
    public class ChangePasswordViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "User ID")]
        public string UserID { get; set; }

        [Required]
        [Display(Name = "Old Password")]
        [StringLength(50)]
        public string OldPassword { get; set; }

        [Required]
        [Display(Name = "New Password")]
        [StringLength(50)]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Confirm New Password")]
        [StringLength(50)]
        public string ConfirmPassword { get; set; }



    }
}