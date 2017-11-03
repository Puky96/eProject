using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eProjects.Models.ProjectViewModels
{
    public class OngoingProjectsListItem
    {
        [Display(Name ="Project Name")]
        public string ProjectName { get; set; }

        [Display(Name ="Start Date")]
        public string StartDate { get; set; }

        [Display(Name ="Due Date")]
        public string DueDate { get; set; }

        [Display(Name = "Is Late")]
        public bool IsLate { get; set; }
    }
}
