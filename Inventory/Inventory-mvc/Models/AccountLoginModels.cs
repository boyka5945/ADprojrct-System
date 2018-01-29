using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Inventory_mvc.Models
{
    public class AccountLoginModels
    {
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Username can not be empty.")]
        [Required(ErrorMessage = "Username can not be empty.")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Password can not be empty.")]
        [Required(ErrorMessage = "Password can not be empty.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; }


        
    }
}