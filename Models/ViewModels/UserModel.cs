using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Swertres.Web.Models.ViewModels
{
    public class UserModel
    {
        public long UserID { get; set; }
        [Required(ErrorMessage = "* required")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "* required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "* required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "* required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        public DateTime? ResetDate { get; set; }
        public long? ResetByID { get; set; }
        public string ResetByName { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LockedDate { get; set; }
        public string RoleName { get; set; }
        public int RoleID { get; set; }
    }

    public class NewUserModel
    {
        public UserModel User {get; set;}
        public UserRoleModelValidation Role { get; set; }
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

    public class UserRoleModelValidation
    {
        [Required(ErrorMessage = "Please select a role.")]
        [Display(Name = "Role")]
        public int SelectedRoleID { get; set; }
        public List<UserRole> Roles { get; set; }
    }

    public class UserRole
    {
        public long RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
    }
}