using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Swertres.Web.Models.ViewModels
{
    public class BetSummaryModel
    {
        public string Number { get; set; }
        public decimal? Total { get; set; }
        public decimal? Excess { get; set; }
        public bool? IsDouble { get; set; }
    }
}