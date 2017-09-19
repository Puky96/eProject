using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eProjects.Models.ManageViewModels
{
    public class AddProjectLeaderViewModel
    {
        [Display(Name ="Username*")]
        [Required]
        public string Username { get; set; }

        [Display(Name ="Fullname*")]
        [Required]
        public string Fullname { get; set; }

        [Display(Name = "Administrator*")]
        [Required]
        public bool IsAdmin { get; set; }
    }
}
