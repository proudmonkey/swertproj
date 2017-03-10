using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Swertres.Web.Models.ViewModels
{
    public class BetSummaryModel
    {
        public int? Number { get; set; }
        public decimal TotalAmountNormal { get; set; }
        public decimal TotalAmountRumble { get; set; }
        public decimal? TotalAmountNormalExcess { get; set; }
        public decimal? TotalAmountRumbleExcess { get; set; }
    }
}