using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eProjects.Models.ProjectViewModels
{
    public class LateProjectListItem
    {
        [Display(Name ="Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Feasibility Late")]
        public bool FeasibilityLate { get; set; }

        [Display(Name = "Feasibility Comments")]
        public string FeasibilityComments { get; set; }

        [Display(Name ="Conceptual Late")]
        public bool ConceptualLate { get; set; }

        [Display(Name = "Conceptual Comments")]
        public string ConceptualComments { get; set; }

        [Display(Name = "Definition Late")]
        public bool DefinitionLate { get; set; }

        [Display(Name = "Definition Comments")]
        public string DefinitionComments { get; set; }

        [Display(Name ="Design&Construct Late")]
        public bool DesignConstructLate { get; set; }

        [Display(Name = "Design&Construct Comments")]
        public string DesignConstructComments { get; set; }

        [Display(Name ="StartupLate")]
        public bool StartupLate { get; set; }

        [Display(Name = "Startup Comments")]
        public string StartupComments { get; set; }
    }
}
