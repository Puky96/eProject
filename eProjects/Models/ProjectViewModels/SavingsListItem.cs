using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eProjects.Models.ProjectViewModels
{
    public class SavingsListItem
    {
        [Display (Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Project Leader")]
        public string ProjectLeader { get; set; }

        [Display(Name ="Due Date")]
        public string DueDate { get; set; }

        [Display(Name = "Savings")]
        public string Savings { get; set; }
    }
}
