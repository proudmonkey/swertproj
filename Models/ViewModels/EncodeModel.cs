using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Swertres.Web.Models.ViewModels
{
    public class EncodeModel
    {
        public long BetID { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public int? Number { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        [Display(Name ="Is Rumble?")]
        public bool IsRumble { get; set; }
        [Display(Name = "Is Double?")]
        public bool IsDouble { get; set; }
    }

    public class EncodeDetailModel
    {
        public EncodeModel Bet { get; set; }
        public DrawModel Draw { get; set; }
        public SourceModelValidation Coordinators { get; set; }
    }

    public class BetModel
    {
        public long BetID { get; set; }
        public int? Number { get; set; }
        public decimal? Amount { get; set; }
        public bool IsRumble { get; set; }
        public bool IsDouble { get; set; }
        public string SourceName { get; set; }
    }

    public class BetListModel
    {
        public List<BetModel> BetList { get; set; }
    }
}