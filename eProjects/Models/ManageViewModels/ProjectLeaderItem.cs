using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eProjects.Models.ManageViewModels
{
    public class ProjectLeaderItem
    {
        public string Id { get; set; }

        [Display(Name ="Full Name")]
        public string Fullname { get; set; }

        [Display (Name = "URI Link")]
        public string UriName { get; set; }

        [Display(Name ="Username")]
        public string Username { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}
