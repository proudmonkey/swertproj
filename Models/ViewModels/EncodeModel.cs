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
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Invalid")]
        public string Number { get; set; }
        [Display(Name = "Target")]
        public decimal? AmountTarget { get; set; }
        [Display(Name = "Rumble")]
        public decimal? AmountRumble { get; set; }
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
        public string Number { get; set; }
        public decimal? AmountTarget { get; set; }
        public decimal? AmountRumble { get; set; }
        public bool IsRumble { get; set; }
        public bool IsDouble { get; set; }
        public string SourceName { get; set; }
        public string EncoderName { get; set; }
    }

    public class BetListModel
    {
        public List<BetModel> BetList { get; set; }
        public decimal? Total { get; set; }
        public decimal? Net { get; set; }
        public decimal? Share { get; set; }
    }
}