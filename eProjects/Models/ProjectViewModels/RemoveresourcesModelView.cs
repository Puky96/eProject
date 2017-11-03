using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eProjects.Models.ProjectViewModels
{
    public class RemoveresourcesModelView
    {
        [Display(Name ="Startup Leader")]
        public string StartupLeader { get; set; }

        [Display(Name ="PC&IS Resource")]
        public string Pcis { get; set; }

        [Display(Name ="P&T Resource")]
        public string PT { get; set; }

        [Display(Name ="E&I Resource")]
        public string EI { get; set; }
    }
}
