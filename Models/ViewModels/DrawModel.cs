using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Swertres.Web.Models.ViewModels
{

    public class DrawListModel
    {
        public IEnumerable<DrawModel> DrawList { get; set; }
    }

    public class DrawModel
    {
        public long DrawID { get; set; }
        [Display(Name = "Control Number")]
        public string ControlNumber { get; set; }
        [Display(Name = "Draw Time")]
        public string SelectedDrawTime { get; set; }
        [Display(Name = "Has Ended?")]
        public bool IsEnded { get; set; }
    }

    public class CreateDrawModel
    {
        [Display(Name = "Control Number")]
        public string ControlNumber { get; set; }
        [Display(Name = "Draw Time")]
        public DefaultDrawTime DrawTime { get; set; }
    }

    public class AddDrawModel
    {
        public string ControlNumber { get; set; }
        public DateTime? StartDateTime { get; set; }
    }

    public class DrawTime
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

    public class DefaultDrawTime
    {
        [Display(Name ="Draw Time")]
        public string SelectedDrawTime { get; set; }
        public IEnumerable<DrawTime> DrawTime { get; set; }
    }
}