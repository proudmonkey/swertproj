//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Swertres.Web.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> PasswordResetDate { get; set; }
        public Nullable<long> PasswordResetUserID { get; set; }
        public Nullable<bool> IsLocked { get; set; }
        public Nullable<System.DateTime> LockedDate { get; set; }
    }
}
