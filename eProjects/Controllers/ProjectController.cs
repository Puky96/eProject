using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using eProjects.DBModels;
using Microsoft.AspNetCore.Identity;
using eProjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using eProjects.Models.ProjectViewModels;

namespace eProjects.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private eProjectsCTX ctx = new eProjectsCTX();
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult AddProject(string message)
        {
            ViewBag.IsAdmin = _userManager.IsInRoleAsync(_userManager.FindByNameAsync(User.Identity.Name).Result, "Administrator").Result;
            ViewBag.Message = message;
            if (User.Identity.Name == "URLT-Admin")
                return RedirectToAction("ViewProjectLeaders", "Manage");

            var Category = ctx.Category.Select(x => x.Name).ToList();
            ViewBag.Categories = new SelectList(Category);

            var ProjectType = ctx.ProjectType.Select(x => x.Name).ToList();
            ViewBag.ProjectTypes = new SelectList(ProjectType);

            var StartupLeaders = ctx.Leaders.Select(x => x.Name).ToList();
            ViewBag.StartupLeaders = new SelectList(StartupLeaders);

            var PcisResource = ctx.Pcisresource.Select(x => x.Name).ToList();
            ViewBag.PcisResource = new SelectList(PcisResource);

            var PTResource = ctx.Ptresource.Select(x => x.Name).ToList();
            ViewBag.PTResource = new SelectList(PTResource);

            var EIResource = ctx.Eiresource.Select(x => x.Name).ToList();
            ViewBag.EIResource = new SelectList(EIResource);

            var Fundingtype = ctx.FundingType.Select(x => x.Name).ToList();
            ViewBag.FundingType = new SelectList(Fundingtype);

            var Priority = new List<int> { 1, 2, 3, 4 };
            ViewBag.Priority = new SelectList(Priority);

            var LeadingDepartment = ctx.LeadingDepartment.Select(x => x.Name).ToList();
            ViewBag.LeadingDepartment = new SelectList(LeadingDepartment);

            var ImpactedDepartment = ctx.ImpactedDepartment.Select(x => x.Name).ToList();
            ViewBag.ImpactedDepartment = new SelectList(ImpactedDepartment);

            var status = ctx.Status.Select(x => x.Name).ToList();
            ViewBag.Status = new SelectList(status, "IN PROGRESS");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProject(AddProjectViewModel model, string errorMessage = null)
        {
            ViewBag.ErrorMessage = errorMessage;
            if (ModelState.IsValid)
            {
                if (CompareDate(model.StartDate, model.FesabilityEndDate.Value))
                    return await AddProject(model, "Feasibility: End Date is before Start Date");
                if (CompareDate(model.FesabilityEndDate.Value, model.ConceptualEndDate.Value))
                    return await AddProject(model, "Conceptual: End Date is before Start Date");
                if (CompareDate(model.ConceptualEndDate.Value, model.DefinitionEndDate.Value))
                    return await AddProject(model, "Definition: End Date is before Start Date");
                if (CompareDate(model.DefinitionEndDate.Value, model.DesignConstructEndDate.Value))
                    return await AddProject(model, "Design&Construct: End Date is before Start Date");
                if (CompareDate(model.DesignConstructEndDate.Value, model.PredictedEndDate))
                    return await AddProject(model, "Startup: End Date is before Start Date");

                if (model.StartupLeader == null && model.AdditionalStartupLeader == null)
                    return AddProject("Select Startup leader or add a new one!");

                if (model.AdditionalPcisResource != null)
                {
                    ctx.Pcisresource.Add(new Pcisresource { Name = model.AdditionalPcisResource });
                    ctx.SaveChanges();
                }
                    
                if (model.AdditionalPTResource != null)
                {
                    ctx.Ptresource.Add(new Ptresource { Name = model.AdditionalPTResource });
                    ctx.SaveChanges();
                }
                if (model.AdditionalEIResource != null)
                {
                    ctx.Eiresource.Add(new Eiresource { Name = model.AdditionalEIResource });
                    ctx.SaveChanges();
                }
                if (model.AdditionalStartupLeader != null)
                {
                    ctx.Leaders.Add(new Leaders { Name = model.AdditionalStartupLeader });
                    ctx.SaveChanges();
                }

                Masterplan project2Add = new Masterplan
                {
                    ActualEndDate = null,
                    ActualEndFiscalYear = null,
                    ActualSpendingTargetEtc = false,
                    Category = model.Category,
                    Cm = model.CM,
                    ConceptualActualEnd = null,
                    ConceptualComments = null,
                    ConceptualCompleted = false,
                    ConceptualDeliverables = null,
                    ConceptualEnd = model.ConceptualEndDate,
                    DefinitionActualEnd = null,
                    DefinitionComments = null,
                    DefinitionDeliverables = null,
                    DefinitionDone = false,
                    DefinitionEnd = model.DefinitionEndDate,
                    DesignConstructActualEnd = null,
                    DesignConstructcCompleted = false,
                    DesignConstructComments = null,
                    DesignConstructDeliverables = null,
                    DesignConstructEnd = model.DesignConstructEndDate,
                    Eiresource = model.AdditionalEIResource ?? model.EIResource,
                    Etc = model.ETC,
                    Saving = model.Savings,
                    FesabilityActualEnd = null,
                    FesabilityComments = null,
                    FesabilityCompletetd = false,
                    FesabilityDeliverables = null,
                    FesabilityEnd = model.FesabilityEndDate,
                    FiscalYearStart = Convert2FiscalYear(model.StartDate),
                    FundingType = model.FundingType,
                    ImpactedDepartment = model.ImpactedDepartment,
                    LeadingDepartment = model.LeadingDepartment,
                    MetSuccesCriteria = false,
                    Ora = null,
                    OtherComments = model.Comments,
                    PcisResource = model.AdditionalPcisResource ?? model.PcisResource,
                    PlantAe = model.PlantAE,
                    PredictedEndDate = model.PredictedEndDate,
                    PredictedEndFiscalYear = Convert2FiscalYear(model.PredictedEndDate),
                    Priority = model.Priority,
                    ProjectLeader = (await _userManager.FindByNameAsync(User.Identity.Name)).Fullname,
                    ProjectName = model.ProjectName,
                    ProjectType = model.ProjectType,
                    Ptresource = model.AdditionalPTResource ?? model.PTResource,
                    StartDate = model.StartDate,
                    StartupComments = null,
                    StartupLeader = model.AdditionalStartupLeader ?? model.StartupLeader,
                    Status = model.Status,
                    TeamCharter = model.TeamCharter
                };
                ctx.Masterplan.Add(project2Add);
                ctx.SaveChanges();

                return RedirectToAction("ViewProjects","Project", new { message = "Project succesfully added!" });
            }

            return View(model);
        }

        public string Convert2FiscalYear (DateTime date)
        {
            var year = date.Year;
            var changeDate = new DateTime(year, 7, 1);
            if (date.CompareTo(changeDate) < 0)
                return ((year - 1) % 100).ToString() + "/" + (year % 100).ToString();
            else
                return (year % 100).ToString() + "/" + ((year + 1) % 100).ToString();
        }

        public async Task<IActionResult> ViewProjects(string message = null)
        {
            ViewBag.Message = message;

            var user = (await _userManager.FindByNameAsync(User.Identity.Name));
            var roles = await _userManager.GetRolesAsync(user);
            var projects = new List<Masterplan>();
            ViewBag.IsAdmin = _userManager.IsInRoleAsync(user, "Administrator").Result;

            if (roles.Contains("Administrator"))
                projects = ctx.Masterplan.ToList();
            else
            {
                var fullName = (await _userManager.FindByNameAsync(User.Identity.Name)).Fullname;
                projects = ctx.Masterplan.Where(x => x.ProjectLeader == fullName).ToList();
            }

            return View(projects);
        }

        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> ViewMyProjects()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var projects = ctx.Masterplan.Where(x => x.ProjectLeader == user.Fullname).ToList();

            return View(projects);
        }

        [HttpGet]
        [Authorize]
        public ActionResult EditProject(int projectId)
        {
            var project = ctx.Masterplan.FirstOrDefault(x => x.Id == projectId);

            var pl = ctx.AspNetUsers.Where(x=> x.Fullname != "URLT-Admin").Select(x => x.Fullname).ToList();
            ViewBag.ProjectLeaders = new SelectList(pl, project.ProjectLeader);

            var leaders = ctx.Leaders.Select(x => x.Name).ToList();
            ViewBag.Leaders = new SelectList(leaders, project.StartupLeader);

            ViewBag.IsAdmin = _userManager.IsInRoleAsync(_userManager.FindByNameAsync(User.Identity.Name).Result, "Administrator").Result;

            var PcisResource = ctx.Pcisresource.Select(x => x.Name).ToList();
            ViewBag.PcisResource = new SelectList(PcisResource, project.PcisResource);

            var PTResource = ctx.Ptresource.Select(x => x.Name).ToList();
            ViewBag.PTResource = new SelectList(PTResource, project.Ptresource);

            var EIResource = ctx.Eiresource.Select(x => x.Name).ToList();
            ViewBag.EIResource = new SelectList(EIResource, project.Eiresource);

            var status = ctx.Status.Select(x => x.Name).ToList();
            ViewBag.Status = new SelectList(status, project.Status);

            ViewBag.StartDate = project.StartDate.ToString("MM/dd/yyyy");
            ViewBag.EndDate = project.PredictedEndDate.ToString("MM/dd/yyyy");

            var model = new EditProjectViewModel
            {
                ActualSpending = project.ActualSpendingTargetEtc,
                CM = project.Cm,
                ETC = project.Etc,
                StartupLeader = project.StartupLeader,
                FesabilityEndDate = project.FesabilityEnd,
                FesabilityDeliverables = project.FesabilityDeliverables,
                FesabilityActualEnd = project.FesabilityActualEnd,
                FesabilityComments = project.FesabilityComments,
                FesabilityComplete = project.FesabilityCompletetd,
                Comments = project.OtherComments,
                ConceptualEndDate = project.ConceptualEnd,
                ConceptualActualEnd = project.ConceptualActualEnd,
                ConceptualComments = project.ConceptualComments,
                ConceptualComplete = project.ConceptualCompleted,
                ConceptualDeliverables = project.ConceptualDeliverables,
                DefinitionEndDate = project.DefinitionEnd,
                DefinitionActualEnd = project.DefinitionActualEnd,
                DefinitionComments = project.DefinitionComments,
                DefinitionDeliverables = project.DefinitionDeliverables,
                DefinitionComplete = project.DefinitionDone,
                DesignConstructEndDate = project.DesignConstructEnd,
                DesignConstructActualEnd = project.DesignConstructActualEnd,
                DesignConstructComments = project.DesignConstructComments,
                DesignConstructDeliverables = project.DesignConstructDeliverables,
                DesignConstructnComplete = project.DesignConstructcCompleted,
                Id = project.Id,
                SuccesCriteriaMet = project.MetSuccesCriteria,
                Status = project.Status,
                StartupEndDate = project.PredictedEndDate,
                StartupActualEnd = project.ActualEndDate,
                StartupComments = project.StartupComments,
                StartDate = project.StartDate.ToString("dd.MM.yyyy"),
                PTResource = project.Ptresource,
                ProjectName = project.ProjectName,
                ProjectLeader = project.ProjectLeader,
                PredictedEndDate = project.PredictedEndDate.ToString("dd.MM.yyyy"),
                PcisResource = project.PcisResource,
                ORA = project.Ora,
                FiscalYearOfCompletion = project.ActualEndFiscalYear,
                EIResource = project.Eiresource,
                FesabilityCapital = project.FesabilityCapitalEstimate != null ? project.FesabilityCapitalEstimate.Value : false,
                FesabilityLevel1 = project.FesabilityLevel1 != null ? project.FesabilityLevel1.Value : false,
                FesabilityPreliminary = project.FesabilityPreliminaryEquipmentList != null ? project.FesabilityPreliminaryEquipmentList.Value : false,
                FessabilityCharter = project.FesabilityCharter != null ? project.FesabilityCharter.Value : false,
                ConceptualPreliminaryLayouts = project.ConceptualPreliminaryLayouts != null ? project.ConceptualPreliminaryLayouts.Value : false,
                ConceptualORA = project.ConceptualOra != null ? project.ConceptualOra.Value : false,
                ConceptualURD = project.ConceptualUrd != null ? project.ConceptualUrd.Value : false,
                ConceptualEquipmentList = project.ConceptualEquipmentList != null ? project.ConceptualEquipmentList.Value : false,
                ConceptualPrimarySourcingPlan = project.ConceptualPreliminarySourcingPlan != null ? project.ConceptualPreliminarySourcingPlan.Value : false,
                ConceptualPreliminaryExecution = project.ConceptualPreliminaryExecutionPlan != null ? project.ConceptualPreliminaryExecutionPlan.Value : false,
                ConceptualPreliminaryFundiingPackage = project.ConceptualPreliminaryFundingPackage != null ? project.ConceptualPreliminaryFundingPackage.Value : false,
                DefinitionDesignBasis = project.DefinitionDesignBasis != null ? project.DefinitionDesignBasis.Value : false,
                DefinitionFinalLayout = project.DefinitionFinalLayout != null ? project.DefinitionFinalLayout.Value : false,
                DesignEquipmentSpecification = project.DefinitionEquipmentSpecification != null ? project.DefinitionEquipmentSpecification.Value : false,
                DefinitionRequestForQuotation = project.DefinitionRequestForQuotations != null ? project.DefinitionRequestForQuotations.Value : false,
                DefinitionFullStart = project.DefinitionFullStartCapitalEstimate != null ? project.DefinitionFullStartCapitalEstimate.Value : false,
                DesignDetailedAssembly = project.DesignDetailedAnssembly != null ? project.DesignDetailedAnssembly.Value : false,
                DesignIQPQProtocol = project.DesignIqpqprotocol != null ? project.DesignIqpqprotocol.Value : false,
                DesignVendorAcceptance = project.DesignVendorAcceptance != null ? project.DesignVendorAcceptance.Value : false,
                DesignEngineeringInformation = project.DesignEngineeringInformation != null ? project.DesignEngineeringInformation.Value : false,
                DesignnLevel3 = project.DesignLevel3 != null ? project.DesignLevel3.Value : false,
                DesignConstructionPlan = project.DesignConstructionPlan != null ? project.DesignConstructionPlan.Value : false,
                StartupAppropriationCloseOut = project.StartupAppropriationCloseOut != null ? project.StartupAppropriationCloseOut.Value : false
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditProject(EditProjectViewModel model)
        {
            if(model.AdditionalPcisResource != null)
            {
                ctx.Pcisresource.Add(new Pcisresource { Name = model.AdditionalPcisResource });
                ctx.SaveChanges();
            }
            if(model.AdditionalEIResource != null)
            {
                ctx.Eiresource.Add(new Eiresource { Name = model.AdditionalEIResource });
                ctx.SaveChanges();
            }
            if(model.AdditionalPTResource != null)
            {
                ctx.Ptresource.Add(new Ptresource { Name = model.AdditionalPTResource });
                ctx.SaveChanges();
            }
            if(model.AdditionalStartupLeader != null)
            {
                ctx.Leaders.Add(new Leaders { Name = model.AdditionalStartupLeader });
                ctx.SaveChanges();
            }

            var project = ctx.Masterplan.FirstOrDefault(x => x.Id == model.Id);
            project.Cm = model.CM ?? project.Cm;
            project.PcisResource = model.AdditionalPcisResource ?? model.PcisResource;
            project.Ptresource = model.AdditionalPTResource ?? model.PTResource;
            project.Eiresource = model.AdditionalEIResource ?? model.EIResource;
            project.StartupLeader = model.AdditionalStartupLeader ?? model.StartupLeader;
            project.ProjectLeader = model.ProjectLeader;
            project.Etc = model.ETC;
            project.Ora = model.ORA;
            //fesability
            project.FesabilityEnd = model.FesabilityEndDate != null ? model.FesabilityEndDate : project.FesabilityEnd;
            project.FesabilityActualEnd = model.FesabilityActualEnd != null ? model.FesabilityActualEnd : project.FesabilityActualEnd;
            project.FesabilityComments = model.FesabilityComments;
            project.FesabilityDeliverables = model.FesabilityDeliverables;
            project.FesabilityCompletetd = model.FesabilityComplete;
            //conceptual
            project.ConceptualEnd = model.ConceptualEndDate != null ? model.ConceptualEndDate : project.ConceptualEnd;
            project.ConceptualActualEnd = model.ConceptualActualEnd != null ? model.ConceptualActualEnd : project.ConceptualActualEnd;
            project.ConceptualComments = model.ConceptualComments;
            project.ConceptualDeliverables = model.ConceptualDeliverables;
            project.ConceptualCompleted = model.ConceptualComplete;
            //definition
            project.DefinitionEnd = model.DefinitionEndDate != null ? model.DefinitionEndDate : project.DefinitionEnd;
            project.DefinitionActualEnd = model.DefinitionActualEnd != null ? model.DefinitionActualEnd : project.DefinitionActualEnd;
            project.DefinitionComments = model.DefinitionComments;
            project.DefinitionDeliverables = model.DefinitionDeliverables;
            project.DefinitionDone = model.DefinitionComplete;
            //design&construct
            project.DesignConstructEnd = model.DesignConstructEndDate != null ? model.DesignConstructEndDate : project.DesignConstructEnd;
            project.DesignConstructActualEnd = model.DesignConstructActualEnd != null ? model.DesignConstructActualEnd : project.DesignConstructActualEnd;
            project.DesignConstructComments = model.DesignConstructComments;
            project.DesignConstructDeliverables = model.DesignConstructDeliverables;
            project.DesignConstructcCompleted = model.DesignConstructnComplete;
            //startup
            project.ActualEndDate = model.StartupActualEnd != null ? model.StartupActualEnd : project.ActualEndDate;
            project.StartupComments = model.StartupComments;
            //other data
            project.ActualEndFiscalYear = model.StartupEndDate != null ? Convert2FiscalYear(model.StartupEndDate.Value) : null;
            project.ActualSpendingTargetEtc = model.ActualSpending;
            project.MetSuccesCriteria = model.SuccesCriteriaMet;
            project.Status = model.Status;
            project.OtherComments = model.Comments;
            //deliverables
            //feasibility
            project.FesabilityCharter = model.FessabilityCharter;
            project.FesabilityPreliminaryEquipmentList = model.FesabilityPreliminary;
            project.FesabilityCapitalEstimate = model.FesabilityCapital;
            project.FesabilityLevel1 = model.FesabilityLevel1;
            //conceptual
            project.ConceptualPreliminaryLayouts = model.ConceptualPreliminaryLayouts;
            project.ConceptualOra = model.ConceptualORA;
            project.ConceptualUrd = model.ConceptualURD;
            project.ConceptualEquipmentList = model.ConceptualEquipmentList;
            project.ConceptualPreliminarySourcingPlan = model.ConceptualPrimarySourcingPlan;
            project.ConceptualPreliminaryExecutionPlan = model.ConceptualPreliminaryExecution;
            project.ConceptualPreliminaryFundingPackage = model.ConceptualPreliminaryFundiingPackage;
            //definition
            project.DefinitionDesignBasis = model.DefinitionDesignBasis;
            project.DefinitionFinalLayout = model.DefinitionFinalLayout;
            project.DefinitionEquipmentSpecification = model.DesignEquipmentSpecification;
            project.DefinitionRequestForQuotations = model.DefinitionRequestForQuotation;
            project.DefinitionFullStartCapitalEstimate = model.DefinitionFullStart;
            project.DefinitionLevel2 = model.DefinitionLevel2;
            project.DefinitionProjectExecutionPlan = model.DefinitionProjectExecutionPlan;
            project.DefinitionFullStartFunding = model.DefinitionFulllStartFunding;
            //design
            project.DesignDetailedAnssembly = model.DesignDetailedAssembly;
            project.DesignIqpqprotocol = model.DesignIQPQProtocol;
            project.DesignVendorAcceptance = model.DesignVendorAcceptance;
            project.DesignEngineeringInformation = model.DesignEngineeringInformation;
            project.DesignLevel3 = model.DesignnLevel3;
            project.DesignConstructionPlan = model.DesignConstructionPlan;
            //startup
            project.StartupAppropriationCloseOut = model.StartupAppropriationCloseOut;


            ctx.Masterplan.Update(project);
            ctx.SaveChanges();


            return RedirectToAction("ViewProjects");
        }

        [Authorize(Roles ="Administrator")]
        public IActionResult DeleteProject(int projectId)
        {
            var project = ctx.Masterplan.FirstOrDefault(x => x.Id == projectId);
            ctx.Masterplan.Remove(project);
            ctx.SaveChanges();

            return RedirectToAction("ViewProjects");
        }

        

        private bool CompareDate(DateTime startDate, DateTime endDate)
        {
            if (startDate.CompareTo(endDate) > 0)
                return true;
            else
                return false;
        }



    }
}
