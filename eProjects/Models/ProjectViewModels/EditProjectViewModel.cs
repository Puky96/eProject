using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eProjects.Models.ProjectViewModels
{
    public class EditProjectViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name ="Startup Leader")]
        public string StartupLeader { get; set; }

        [Display(Name = "Project Leader")]
        public string ProjectLeader { get; set; }

        [Display(Name = "Start Date")]
        public string StartDate { get; set; }

        [Display(Name = "Predicted End Date")]
        public string PredictedEndDate { get; set; } 

        [Display(Name = "CM#")]
        public int? CM { get; set; }

        [Display(Name = "PC&IS Resource")]
        public string PcisResource { get; set; }

        [Display(Name = "Other PC&IS Resource")]
        public string AdditionalPcisResource { get; set; }

        [Display(Name = "PT Resource")]
        public string PTResource { get; set; }

        [Display(Name = "OtherPTResource")]
        public string AdditionalPTResource { get; set; }

        [Display(Name = "E&I Resource")]
        public string EIResource { get; set; }

        [Display(Name = "Other E&I Resource")]
        public string AdditionalEIResource { get; set; }

        [Display(Name = "ETC")]
        public int? ETC { get; set; }

        [Display(Name = "ORA")]
        public decimal? ORA { get; set; }

        [Display(Name = "Predicted End Date")]
        public DateTime? FesabilityEndDate { get; set; }

        [Display(Name ="Actual End Date")]
        public DateTime? FesabilityActualEnd { get; set; }

        [Display(Name = "Delay Causes")]
        public string FesabilityComments { get; set; }

        [Display(Name ="Deliverables")]
        public string FesabilityDeliverables { get; set; }

        [Display(Name ="Completed?")]
        public bool FesabilityComplete { get; set; }

        [Display(Name = "Predicted End Date")]
        public DateTime? ConceptualEndDate { get; set; }

        [Display(Name = "Actual End Date")]
        public DateTime? ConceptualActualEnd { get; set; }

        [Display(Name = "Delay Causes")]
        public string ConceptualComments { get; set; }

        [Display(Name = "Deliverables")]
        public string ConceptualDeliverables { get; set; }

        [Display(Name = "Completed?")]
        public bool ConceptualComplete { get; set; }

        [Display(Name = "Predicted End Date")]
        public DateTime? DefinitionEndDate { get; set; }

        [Display(Name = "Actual End Date")]
        public DateTime? DefinitionActualEnd { get; set; }

        [Display(Name = "Delay Causes")]
        public string DefinitionComments { get; set; }

        [Display(Name = "Deliverables")]
        public string DefinitionDeliverables { get; set; }

        [Display(Name = "Completed?")]
        public bool DefinitionComplete { get; set; }

        [Display(Name = "Predicted End Date")]
        public DateTime? DesignConstructEndDate { get; set; }

        [Display(Name = "Actual End Date")]
        public DateTime? DesignConstructActualEnd { get; set; }

        [Display(Name = "Delay Causes")]
        public string DesignConstructComments { get; set; }

        [Display(Name = "Deliverables")]
        public string DesignConstructDeliverables { get; set; }

        [Display(Name = "Completed?")]
        public bool DesignConstructnComplete { get; set; }

        [Display(Name = "Predicted End Date")]
        public DateTime? StartupEndDate { get; set; }

        [Display(Name ="Actual End Date")]
        public DateTime? StartupActualEnd { get; set; }

        [Display(Name = "Delay Causes")]
        public string StartupComments { get; set; }

        [Display(Name = "Actual fiscal year of completion")]
        public string FiscalYearOfCompletion { get; set; }

        [Display(Name = "Actual Spending on target with ETC")]
        public bool ActualSpending { get; set; }

        [Display(Name = "Succes Criteria Met for Project?")]
        public bool SuccesCriteriaMet { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Comments")]
        public string Comments { get; set; }

        //fesability deliverables

        [Display(Name = "Charter and/or Technical Base Plan (TBP)")]
        public bool FessabilityCharter { get; set; }

        [Display(Name = "Preliminary Equipment List")]
        public bool FesabilityPreliminary { get; set; }

        [Display(Name = "Feasibility Capital Estimate")]
        public bool FesabilityCapital { get; set; }

        [Display(Name = "Level 1 - Milestone Schedule")]
        public bool FesabilityLevel1 { get; set; }

        //Conceptual
        //select tehnology option
        [Display(Name = "Preliminary Layouts, P&IDs, and Single Line Drawing")]
        public bool ConceptualPreliminaryLayouts { get; set; }

        [Display(Name = "Overall Risk Assessment (ORA)")]
        public bool ConceptualORA { get; set; }

        [Display(Name = "User Requirements Document (URD)")]
        public bool ConceptualURD { get; set; }

        [Display(Name = "Equipment List ")]
        public bool ConceptualEquipmentList { get; set; }
        //develop project execution
        [Display(Name = "Preliminary Sourcing Plan")]
        public bool ConceptualPrimarySourcingPlan { get; set; }

        [Display(Name = "Preliminary Execution Plan")]
        public bool ConceptualPreliminaryExecution { get; set; }

        [Display(Name = "Preliminary Funding Package")]
        public bool ConceptualPreliminaryFundiingPackage { get; set; }

        //Definition
        [Display(Name = "Design Basis ")]
        public bool DefinitionDesignBasis { get; set; }

        [Display(Name = "Final Layouts & P&IDs, and Single Line Drawings")]
        public bool DefinitionFinalLayout { get; set; }

        [Display(Name = "Equipment Specifications")]
        public bool DesignEquipmentSpecification { get; set; }

        [Display(Name = "Request for Quotations (RFQs) ")]
        public bool DefinitionRequestForQuotation { get; set; }

        [Display(Name = "Full Start Capital Estimate ")]
        public bool DefinitionFullStart { get; set; }

        [Display(Name = "Level 2 Project Schedule")]
        public bool DefinitionLevel2 { get; set; }

        [Display(Name = "Project Execution Plan ")]
        public bool DefinitionProjectExecutionPlan { get; set; }

        [Display(Name = "Full Start Funding")]
        public bool DefinitionFulllStartFunding { get; set; }

        //design&construct
        [Display(Name = "Detailed Assembly Drawing & Bill of Materials (BOMs)")]
        public bool DesignDetailedAssembly { get; set; }

        [Display(Name = "IQ/OQ Protocol")]
        public bool DesignIQPQProtocol { get; set; }

        [Display(Name = "Vendor Acceptance Test Plans")]
        public bool DesignVendorAcceptance { get; set; }

        [Display(Name = "Engineering Information (EI) Packages")]
        public bool DesignEngineeringInformation { get; set; }

        [Display(Name = "Level 3 Project Schedule")]
        public bool DesignnLevel3 { get; set; }

        [Display(Name = "Construction Plan")]
        public bool DesignConstructionPlan { get; set; }

        //startup    
        [Display(Name = "Appropriation Close-out")]
        public bool StartupAppropriationCloseOut { get; set; }
    }
}
