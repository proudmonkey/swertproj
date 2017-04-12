using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Swertres.Web.Models.ViewModels
{
    public class SourceModelValidation
    {
        [Required(ErrorMessage = "Please select a coordinator.")]
        [Display(Name ="Coordinator")]
        public long SelectedSourceID { get; set; }
        public List<SourceModel> Sources { get; set; }
    }

    public class SourceModel
    {
        public long SourceID { get; set; }
        public string SourceName { get; set; }
        public int? Share { get; set; }
    }

    public class SourceListModel
    {
        public IEnumerable<SourceModel> SourceList { get; set; }
    }

    public class CreateSourceModel
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "* required")]
        public string SourceName { get; set; }
        [Display(Name = "Share")]
        [Required(ErrorMessage = "* required")]
        public int? Share { get; set; }
    }

  
}