using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eProjects.Models.ProjectViewModels
{
    public class Top3ListItem
    {
        [Display(Name ="Project Leader")]
        public string ProjectLeader { get; set; }

        [Display(Name ="Priority 1")]
        public string Priority1 { get; set; }

        [Display(Name ="Priority 2")]
        public string Priority2 { get; set; }

        [Display(Name ="Priority 3")]
        public string Priority3 { get; set; }
    }
}
