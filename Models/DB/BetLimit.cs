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
    
    public partial class BetLimit
    {
        public int BetLimitID { get; set; }
        public Nullable<int> NormalLimit { get; set; }
        public Nullable<int> RumbleLimit { get; set; }
        public Nullable<int> DoubleLimit { get; set; }
    }
}
