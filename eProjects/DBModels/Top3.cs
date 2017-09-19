using System;
using System.Collections.Generic;

namespace eProjects.DBModels
{
    public partial class Top3
    {
        public int Id { get; set; }
        public string ProjectLeader { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
    }
}
