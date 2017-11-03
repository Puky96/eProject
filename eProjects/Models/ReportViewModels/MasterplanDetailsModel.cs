using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eProjects.Models.ReportViewModels
{
    public class MasterplanDetailsModel
    {
        public int Id { get; set; }

        [Display(Name ="Project Name")]
        public string ProjectName { get; set; }

        [Display(Name ="Project Leader")]
        public string ProjectLeader { get; set; }

        [Display(Name = "startup Leader")]
        public string StartupLeader { get; set; }

        [Display(Name = "Change Managemment Number")]
        public int Cm { get; set; }

        [Display(Name ="PC&IS Resource")]
        public string PcisResource { get; set; }

        [Display(Name = "P&T Resource")]
        public string Ptresource { get; set; }

        [Display(Name = "E&I Resource")]
        public string Eiresource { get; set; }

        [Display(Name = "Start Date")]
        public string StartDate { get; set; }

        [Display(Name = "Starting Fiscal Year")]
        public string FiscalYearStart { get; set; }

        [Display(Name = "Plant AE")]
        public bool PlantAe { get; set; }

        [Display(Name = "Team Charter")]
        public bool TeamCharter { get; set; }

        [Display(Name = "ETC ($)")]
        public int Etc { get; set; }

        [Display(Name = "Priority")]
        public int Priority { get; set; }

        [Display(Name = "Funding Type")]
        public string FundingType { get; set; }

        [Display(Name = "Project Type")]
        public string ProjectType { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }
        public string LeadingDepartment { get; set; }

        [Display(Name = "Leading Department")]
        public string ImpactedDepartment { get; set; }

        [Display(Name = "ORA")]
        public decimal Ora { get; set; }

        [Display(Name = "Predicted End Date")]
        public string PredictedEndDate { get; set; }

        [Display(Name = "Predicted Ending Fiscal Year ")]
        public string PredictedEndFiscalYear { get; set; }

        [Display(Name = "Actual End Date")]
        public string ActualEndDate { get; set; }

        [Display(Name = "Actual End Fiscal Year")]
        public string ActualEndFiscalYear { get; set; }

        [Display(Name = "Feasibility End")]
        public string FesabilityEnd { get; set; }

        [Display(Name = "Feasibility Actual End")]
        public string FesabilityActualEnd { get; set; }

        [Display(Name = "Feasibility Comments")]
        public string FesabilityComments { get; set; }

        [Display(Name = "Feasibility Completion")]
        public bool FesabilityCompletetd { get; set; }

        [Display(Name = "Conceptual End")]
        public string ConceptualEnd { get; set; }

        [Display(Name = "Conceptual Actual End")]
        public string ConceptualActualEnd { get; set; }

        [Display(Name = "Conceptual Comments")]
        public string ConceptualComments { get; set; }

        [Display(Name = "Conceptual Completion")]
        public bool ConceptualCompleted { get; set; }

        [Display(Name = "Definition End")]
        public string DefinitionEnd { get; set; }

        [Display(Name = "Definition Actual End")]
        public string DefinitionActualEnd { get; set; }

        [Display(Name = "Definition Comments")]
        public string DefinitionComments { get; set; }

        [Display(Name = "Definition Completion")]
        public bool DefinitionDone { get; set; }

        [Display(Name = "Design&Construct End")]
        public string DesignConstructEnd { get; set; }

        [Display(Name = "Design&Construct Actual End")]
        public string DesignConstructActualEnd { get; set; }

        [Display(Name = "Design&Construct Comments")]
        public string DesignConstructComments { get; set; }

        [Display(Name = "Design&Construct Completion")]
        public bool DesignConstructcCompleted { get; set; }

        [Display(Name = "Startup Comments")]
        public string StartupComments { get; set; }

        [Display(Name = "Actual Spending Target ETC")]
        public bool ActualSpendingTargetEtc { get; set; }

        [Display(Name = "Succes Criteria Met")]
        public bool MetSuccesCriteria { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Other Comments")]
        public string OtherComments { get; set; }

        [Display(Name = "Charter/ Technical Base Plan")]
        public bool FesabilityCharter { get; set; }

        [Display(Name = "Preliminary Equipment List")]
        public bool FesabilityPreliminaryEquipmentList { get; set; }

        [Display(Name ="Capital Estimate")]
        public bool FesabilityCapitalEstimate { get; set; }

        [Display(Name = "Level 1 - Milestone Schedule")]
        public bool FesabilityLevel1 { get; set; }

        [Display(Name = "Preliminary Layouts, P&IDs, Single Line Drawing")]
        public bool ConceptualPreliminaryLayouts { get; set; }

        [Display(Name = "Overall Risk Asesement (ORA)")]
        public bool ConceptualOra { get; set; }

        [Display(Name = "User Required Document (URD)")]
        public bool ConceptualUrd { get; set; }

        [Display(Name = "Equipment List")]
        public bool ConceptualEquipmentList { get; set; }

        [Display(Name = "Preliminary Sourcing Plan")]
        public bool ConceptualPreliminarySourcingPlan { get; set; }

        [Display(Name = "Preliminary Execution Plan")]
        public bool ConceptualPreliminaryExecutionPlan { get; set; }

        [Display(Name = "Preliminary Funding Package")]
        public bool ConceptualPreliminaryFundingPackage { get; set; }

        [Display(Name = "Design Basis")]
        public bool DefinitionDesignBasis { get; set; }

        [Display(Name = "Final Layouts, P&ID, Single Line Drawings")]
        public bool DefinitionFinalLayout { get; set; }

        [Display(Name = "Equipment Specification")]
        public bool DefinitionEquipmentSpecification { get; set; }

        [Display(Name = "Request for Quotations (RFQs)")]    
        public bool DefinitionRequestForQuotations { get; set; }

        [Display(Name = "Full Start Capital Estimate")]
        public bool DefinitionFullStartCapitalEstimate { get; set; }

        [Display(Name = "Level 2 - Project Schedule")]
        public bool DefinitionLevel2 { get; set; }

        [Display(Name = "Project Execution Plan")]
        public bool DefinitionProjectExecutionPlan { get; set; }

        [Display(Name = "Full Start Funding")]
        public bool DefinitionFullStartFunding { get; set; }

        [Display(Name = "Detailed Assembly Drawing & Bill of Materials (BOMs)")]
        public bool DesignDetailedAnssembly { get; set; }

        [Display(Name ="IQ/PQ Protocol")]
        public bool DesignIqpqprotocol { get; set; }

        [Display(Name ="Vendor Acceptance Test Plans")]
        public bool DesignVendorAcceptance { get; set; }

        [Display(Name = "Engineering Information (EI) Packages")]
        public bool DesignEngineeringInformation { get; set; }

        [Display(Name = "Level 3 - Project Schedule")]
        public bool DesignLevel3 { get; set; }

        [Display(Name = "Construction Plan")]
        public bool DesignConstructionPlan { get; set; }

        [Display(Name = "Appropriation Close-Out")]
        public bool StartupAppropriationCloseOut { get; set; }
    }
}
