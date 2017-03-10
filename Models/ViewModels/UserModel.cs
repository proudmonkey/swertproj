using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Swertres.Web.Models.ViewModels
{
    public class UserModel
    {
    }

    public class UserLoginView
    {
        [Key]
        public long UserID { get; set; }
        [Required(ErrorMessage = "* required")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "* required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}