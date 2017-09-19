using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eProjects.Models.ManageViewModels
{
    public class Top3ViewModel
    {
        [Display(Name ="1st Project")]
        public string Option1 { get; set; }

        [Display(Name = "2nd Project")]
        public string Option2 { get; set; }

        [Display(Name = "3rd Project")]
        public string Option3 { get; set; }
    }
}
