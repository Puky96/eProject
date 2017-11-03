using System;
using System.Collections.Generic;

namespace eProjects.DBModels
{
    public partial class ProjectLate
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public bool FesabilityLate { get; set; }
        public bool ConceptualLate { get; set; }
        public bool DefinitionLate { get; set; }
        public bool DesignConstructLate { get; set; }
        public bool StartupLate { get; set; }
    }
}
