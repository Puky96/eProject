using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eProjects.Models.ProjectViewModels
{
    public class AddProjectViewModel
    {
        [Display(Name = "Project Name *")]
        [Required]
        public string ProjectName { get; set; }

        [Display(Name = "Startup Leader")]
        public string StartupLeader { get; set; }

        [Display(Name = "Saving * ")]
        public int Savings { get; set; }

        [Display(Name = "Other Startup Leader")]
        public string AdditionalStartupLeader { get; set; }

        [Display(Name = "ChangeManagement number")]
        public int ? CM { get; set; }

        [Display(Name = "PC&IS Resource")]
        public string PcisResource { get; set; }

        [Display(Name = "Other PC&IS Resource")]
        public string AdditionalPcisResource { get; set; }

        [Display(Name = "PT Resource")]
        public string PTResource { get; set; }

        [Display(Name = "OtherPTResource")]
        public string AdditionalPTResource { get; set; }

        [Display(Name = "E&I Resource")]
        public string EIResource { get; set; }

        [Display(Name = "Other E&I Resource")]
        public string AdditionalEIResource { get; set; }

        [Display(Name = "Project Start Date *")]
        [Required]
        public DateTime StartDate { get; set; }

        [Display(Name = "Plant AE")]
        public bool PlantAE { get; set; }

        [Display(Name = "Team Charter")]
        public bool TeamCharter { get; set; }

        [Display(Name = "ETC")]
        public int ? ETC { get; set; }

        [Display(Name = "Priority *")]
        [Required]
        public int Priority { get; set; }

        [Display(Name = "Funding Type *")]
        [Required]
        public string FundingType { get; set; }

        [Display(Name = "Project Type *")]
        [Required]
        public string ProjectType { get; set; }

        [Display(Name = "Category*")]
        [Required]
        public string Category { get; set; }

        [Display(Name = "Leading Department *")]
        [Required]
        public string LeadingDepartment { get; set; }

        [Display(Name = "Impacted Department *")]
        [Required]
        public string ImpactedDepartment { get; set; }

        [Display(Name = "Predicted End Date *")]
        [Required]
        public DateTime PredictedEndDate { get; set; }

        [Required]
        [Display(Name = "Predicted Fesability End Date *")]
        public DateTime ? FesabilityEndDate { get; set; }

        [Required]
        [Display(Name = "Predicted Conceptual End Date *")]
        public DateTime ? ConceptualEndDate { get; set; }

        [Required]
        [Display(Name = "Predicted Definition End Date *")]
        public DateTime ? DefinitionEndDate { get; set; }

        [Required]
        [Display(Name = "Predicted Design&Construct End Date *")]
        public DateTime ? DesignConstructEndDate { get; set; }

        [Display(Name = "Status")]
        [Required]
        public string Status { get; set; }

        [Display(Name = "Other comments")]
        public string Comments { get; set; }

    }
}
