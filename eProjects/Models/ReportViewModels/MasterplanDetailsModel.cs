using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProjects.Models.ReportViewModels
{
    public class MasterplanDetailsModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectLeader { get; set; }
        public string StartupLeader { get; set; }
        public int Cm { get; set; }
        public string PcisResource { get; set; }
        public string Ptresource { get; set; }
        public string Eiresource { get; set; }
        public string StartDate { get; set; }
        public string FiscalYearStart { get; set; }
        public bool PlantAe { get; set; }
        public bool TeamCharter { get; set; }
        public int Etc { get; set; }
        public int Priority { get; set; }
        public string FundingType { get; set; }
        public string ProjectType { get; set; }
        public string Category { get; set; }
        public string LeadingDepartment { get; set; }
        public string ImpactedDepartment { get; set; }
        public decimal Ora { get; set; }
        public string PredictedEndDate { get; set; }
        public string PredictedEndFiscalYear { get; set; }
        public string ActualEndDate { get; set; }
        public string ActualEndFiscalYear { get; set; }
        public string FesabilityEnd { get; set; }
        public string FesabilityActualEnd { get; set; }
        public string FesabilityComments { get; set; }
        public bool FesabilityCompletetd { get; set; }
        public string ConceptualEnd { get; set; }
        public string ConceptualActualEnd { get; set; }
        public string ConceptualComments { get; set; }
        public bool ConceptualCompleted { get; set; }
        public string DefinitionEnd { get; set; }
        public string DefinitionActualEnd { get; set; }
        public string DefinitionComments { get; set; }
        public bool DefinitionDone { get; set; }
        public string DesignConstructEnd { get; set; }
        public string DesignConstructActualEnd { get; set; }
        public string DesignConstructComments { get; set; }
        public bool DesignConstructcCompleted { get; set; }
        public string StartupComments { get; set; }
        public bool ActualSpendingTargetEtc { get; set; }
        public bool MetSuccesCriteria { get; set; }
        public string Status { get; set; }
        public string OtherComments { get; set; }
        public bool FesabilityCharter { get; set; }
        public bool FesabilityPreliminaryEquipmentList { get; set; }
        public bool FesabilityCapitalEstimate { get; set; }
        public bool FesabilityLevel1 { get; set; }
        public bool ConceptualPreliminaryLayouts { get; set; }
        public bool ConceptualOra { get; set; }
        public bool ConceptualUrd { get; set; }
        public bool ConceptualEquipmentList { get; set; }
        public bool ConceptualPreliminarySourcingPlan { get; set; }
        public bool ConceptualPreliminaryExecutionPlan { get; set; }
        public bool ConceptualPreliminaryFundingPackage { get; set; }
        public bool DefinitionDesignBasis { get; set; }
        public bool DefinitionFinalLayout { get; set; }
        public bool DefinitionEquipmentSpecification { get; set; }
        public bool DefinitionRequestForQuotations { get; set; }
        public bool DefinitionFullStartCapitalEstimate { get; set; }
        public bool DefinitionLevel2 { get; set; }
        public bool DefinitionProjectExecutionPlan { get; set; }
        public bool DefinitionFullStartFunding { get; set; }
        public bool DesignDetailedAnssembly { get; set; }
        public bool DesignIqpqprotocol { get; set; }
        public bool DesignVendorAcceptance { get; set; }
        public bool DesignEngineeringInformation { get; set; }
        public bool DesignLevel3 { get; set; }
        public bool DesignConstructionPlan { get; set; }
        public bool StartupAppropriationCloseOut { get; set; }
    }
}
