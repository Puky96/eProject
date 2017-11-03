using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eProjects.Models.ManageViewModels
{
    public class AddResourceViewModel
    {
        [Display(Name ="Username")]
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Fullname { get; set; }

        [Required]
        public string ResourceType { get; set; }
    }
}
