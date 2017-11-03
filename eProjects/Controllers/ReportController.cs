using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using eProjects.DBModels;
using eProjects.Models.ReportViewModels;
using eProjects.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using eProjects.Models.ProjectViewModels;
using jsreport.AspNetCore;
using jsreport.Types;

namespace eProjects.Controllers
{
    [Authorize(Roles = "Administrator, ProjectLeader")]
    public class ReportController : Controller
    {
        private eProjectsCTX ctx = new eProjectsCTX();
        UserManager<ApplicationUser> _userManager;

        public ReportController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public double getJavaScriptDateFormat(DateTime date)
        {
            return date.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        public async Task<JsonResult> RetrieveData(string projectLeader)
        {
            var leaderName = (await _userManager.FindByIdAsync(projectLeader)).Fullname;
            var projects = ctx.Masterplan.Where(x => x.ProjectLeader == leaderName && x.Status == "IN PROGRESS").ToList();

            JsonResponseModel model = new JsonResponseModel
            {
                Cols = new List<Col>()
                    {
                        new Col
                        {
                            Id = "",
                            Label = "Project Name",
                            Pattern = "",
                            Type = "string",
                            Role=""
                        },
                        new Col
                        {
                            Id="",
                            Label ="Period",
                            Pattern="",
                            Type = "string",
                            Role=""
                        },
                        new Col
                        {
                            Type="string",
                            Role = "style",
                            Id="",
                            Pattern="",
                            Label=""
                        },
                        new Col
                        {
                            Id ="",
                            Label = "Start",
                            Pattern="",
                            Type = "date",
                            Role=""
                        },
                        new Col
                        {
                            Id="",
                            Label = "End",
                            Pattern="",
                            Type = "date",
                            Role=""
                        }
                    },
                Rows = new List<Row>()
            };

            foreach (var project in projects)
            {
                AddRows(model, project);
            }

            return Json(model);
        }

        public void AddRow(JsonResponseModel model, string ProjectName, string PeriodName, DateTime Start, DateTime End, string Color = null)
        {
            model.Rows.Add(new Row
            {
                C = new List<C>()
                    {
                        new C()
                        {
                            V = ProjectName
                        },
                        new C()
                        {
                            V = PeriodName
                        },
                        new C()
                        {
                            V = Color
                        },
                        new C()
                        {
                            V = getJavaScriptDateFormat(Start)
                        },
                        new C()
                        {
                            V = getJavaScriptDateFormat(End)
                        }
                    }
            });
        }

        public void AddRows(JsonResponseModel model, Masterplan project)
        {
            AddRow(model, project.ProjectName, "Fesability", project.StartDate, project.FesabilityEnd.Value, "blue");
            AddRow(model, project.ProjectName, "Conceptual", project.FesabilityEnd.Value, project.ConceptualEnd.Value, "yellow");
            AddRow(model, project.ProjectName, "Definition", project.ConceptualEnd.Value, project.DefinitionEnd.Value, "purple");
            AddRow(model, project.ProjectName, "Design&Construct", project.DefinitionEnd.Value, project.DesignConstructEnd.Value, "grey");
            AddRow(model, project.ProjectName, "Startup", project.DesignConstructEnd.Value, project.PredictedEndDate, "orange");

            if (project.FesabilityEnd != null && project.FesabilityActualEnd != null)
            {
                var check = project.FesabilityActualEnd.Value.CompareTo(project.FesabilityEnd.Value);

                if (check <= 0)
                    AddRow(model, project.ProjectName, "Fesability Actual", project.StartDate, project.FesabilityActualEnd.Value, "green");
                if (check > 0)
                    AddRow(model, project.ProjectName, "Fesability Actual", project.StartDate, project.FesabilityActualEnd.Value, "red");
            }

            if (project.FesabilityActualEnd != null && project.ConceptualActualEnd != null)
            {
                var check = project.ConceptualActualEnd.Value.CompareTo(project.ConceptualEnd.Value);

                if (check <= 0)
                    AddRow(model, project.ProjectName, "Conceptual Actual", project.FesabilityActualEnd.Value, project.ConceptualActualEnd.Value, "green");
                else
                    AddRow(model, project.ProjectName, "Conceptual Actual", project.FesabilityActualEnd.Value, project.ConceptualActualEnd.Value, "red");
            }

            if (project.ConceptualActualEnd != null && project.DefinitionActualEnd != null)
            {
                var check = project.DefinitionActualEnd.Value.CompareTo(project.DefinitionEnd.Value);

                if (check <= 0)
                    AddRow(model, project.ProjectName, "Definition Actual", project.ConceptualActualEnd.Value, project.DefinitionActualEnd.Value, "green");
                else
                    AddRow(model, project.ProjectName, "Definition Actual", project.ConceptualActualEnd.Value, project.DefinitionActualEnd.Value, "red");
            }

            if (project.DefinitionActualEnd != null && project.DesignConstructActualEnd != null)
            {
                var check = project.DefinitionActualEnd.Value.CompareTo(project.DefinitionEnd.Value);

                if (check <= 0)
                    AddRow(model, project.ProjectName, "Design&Construct Actual", project.DefinitionActualEnd.Value, project.DesignConstructActualEnd.Value, "green");
                else
                    AddRow(model, project.ProjectName, "Design&Construct Actual", project.DefinitionActualEnd.Value, project.DesignConstructActualEnd.Value, "red");
            }

            if (project.DesignConstructActualEnd != null)
            {
                var check = project.ActualEndDate.Value.CompareTo(project.PredictedEndDate);

                if (check <= 0)
                    AddRow(model, project.ProjectName, "Startup Actual", project.DesignConstructActualEnd.Value, project.ActualEndDate.Value, "green");
                else
                    AddRow(model, project.ProjectName, "Startup Actual", project.DesignConstructActualEnd.Value, project.ActualEndDate.Value, "red");
            }
        }

        public IActionResult ViewTimeline(string projectLeader)
        {
            ViewBag.ProjectLeader = projectLeader;
            var leaderName = _userManager.FindByIdAsync(projectLeader).Result.Fullname;
            ViewBag.LeaderName = leaderName;
            ViewBag.IsAdmin = _userManager.IsInRoleAsync(_userManager.FindByIdAsync(projectLeader).Result, "Administrator").Result;

            var projects = ctx.Masterplan.Where(x => x.ProjectLeader == leaderName && x.Status == "IN PROGRESS").ToList();
            if (projects.Count() == 0)
                ViewBag.Message = "There are no projects to show timeline for! All projects are completed or there is no project registered for " + leaderName + " yet";

            ViewBag.Projects = new SelectList(projects, "ProjectName", "ProjectName");
            var periods = new List<string> { "Feasibility", "Conceptual", "Definition", "Design&Construct", "Startup" };
            ViewBag.Periods = new SelectList(periods);

            return View(new ViewDelaysViewModel { Username = projectLeader });
        }

        public JsonResult RetrieveComments(string projectLeader, string period, string projectName)
        {
            var fullname = _userManager.FindByIdAsync(projectLeader).Result.Fullname;
            var project = ctx.Masterplan.FirstOrDefault(x => x.ProjectLeader == fullname && x.ProjectName == projectName);

            var message = "a";
            if (period == "Feasibility")
                message = project.FesabilityComments ?? "No comment attached to this delay";
            if (period == "Conceptual")
                message = project.ConceptualComments ?? "No comment attached to this delay";
            if (period == "Definition")
                message = project.DefinitionComments ?? "No comment attached to this delay";
            if (period == "Design&Construct")
                message = project.DesignConstructComments ?? "No comment attached to this delay";
            if (period == "Startup")
                message = project.StartupComments ?? "No comment attached to this delay";

            return Json(message);
        }

        public async Task<IActionResult> ViewMyTimeline()
        {
            var userId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;

            return RedirectToAction("ViewTimeline", new { projectLeader = userId });
        }

        public JsonResult RetrieveResourceTimeline(string resourceName, string resourceType)
        {
            List<Masterplan> projects = new List<Masterplan>();
            if (resourceType == "PCIS")
                projects = ctx.Masterplan.Where(x => x.PcisResource == resourceName).ToList();
            if (resourceType == "PT")
                projects = ctx.Masterplan.Where(x => x.Ptresource == resourceName).ToList();
            if (resourceType == "EI")
                projects = ctx.Masterplan.Where(x => x.Eiresource == resourceName).ToList();

            JsonResponseModel model = new JsonResponseModel
            {
                Cols = new List<Col>()
                    {
                        new Col
                        {
                            Id = "",
                            Label = "Project Name",
                            Pattern = "",
                            Type = "string",
                            Role=""
                        },
                        new Col
                        {
                            Id="",
                            Label ="Period",
                            Pattern="",
                            Type = "string",
                            Role=""
                        },
                        new Col
                        {
                            Type="string",
                            Role = "style",
                            Id="",
                            Pattern="",
                            Label=""
                        },
                        new Col
                        {
                            Id ="",
                            Label = "Start",
                            Pattern="",
                            Type = "date",
                            Role=""
                        },
                        new Col
                        {
                            Id="",
                            Label = "End",
                            Pattern="",
                            Type = "date",
                            Role=""
                        }
                    },
                Rows = new List<Row>()
            };

            foreach(var project in projects)
            {
                AddRows(model, project);
            }

            return Json(model);
        }

        public IActionResult GenerateReport()
        {
            var usr = _userManager.FindByNameAsync(User.Identity.Name).Result;
            ViewBag.IsAdmin = _userManager.IsInRoleAsync(usr, "Administrator").Result;

            return View();
        }

        public IActionResult ResourceTimeline()
        {
            return View();
        }
        
        public JsonResult RetrieveResources(string resourceType)
        {
            if (resourceType == "PCIS")
            {
                var resources = ctx.Pcisresource.ToList();
                var json = new List<JsonStandardResponse>();
                foreach (var resource in resources)
                    json.Add(new JsonStandardResponse
                    {
                        Id = resource.Id,
                        Value = resource.Name
                    });
                return Json(json);
            }

            if (resourceType == "PT")
            {
                var resources = ctx.Ptresource.ToList();
                var json = new List<JsonStandardResponse>();
                foreach (var resource in resources)
                    json.Add(new JsonStandardResponse
                    {
                        Id = resource.Id,
                        Value = resource.Name
                    });
                return Json(json);
            }

            if (resourceType == "EI")
            {
                var resources = ctx.Eiresource.ToList();
                var json = new List<JsonStandardResponse>();
                foreach (var resource in resources)
                    json.Add(new JsonStandardResponse
                    {
                        Id = resource.Id,
                        Value = resource.Name
                    });
                return Json(json);
            }

            return null;
        }

        public IActionResult OnePager()
        {
            ViewBag.ProjectLeaders = new SelectList(ctx.Masterplan.Select(x => x.ProjectLeader).Distinct().ToList());

            return View();
        }

        public JsonResult RetrieveProjects(string projectLeader)
        {
            var projects = ctx.Masterplan.Where(x => x.ProjectLeader == projectLeader).ToList();

            List<JsonStandardResponse> json = new List<JsonStandardResponse>();
            foreach (var project in projects)
                json.Add(new JsonStandardResponse
                {
                    Id = project.Id,
                    Value = project.ProjectName
                });

            return Json(json);
        }

        public JsonResult RetrieveMasterPlanData(string projectName = "eProjects - dashboard", string projectLeader = "Costea Sorin")
        {
            var project = ctx.Masterplan.FirstOrDefault(x => x.ProjectName == projectName && x.ProjectLeader == projectLeader);

            MasterplanDetailsModel model = new MasterplanDetailsModel
            {
                ActualEndDate = project.ActualEndDate != null ? project.ActualEndDate.Value.ToString("dd.MM.yyyy") : "none",
                ActualEndFiscalYear = project.ActualEndFiscalYear ?? "none",
                ActualSpendingTargetEtc = project.ActualSpendingTargetEtc,
                Category = project.Category,
                Cm = project.Cm ?? 0,
                ConceptualActualEnd = project.ConceptualActualEnd != null ? project.ConceptualActualEnd.Value.ToString("dd.MM.yyy") : "none",
                ConceptualComments = project.ConceptualComments,
                ConceptualCompleted = project.ConceptualCompleted,
                ConceptualEnd = project.ConceptualEnd.Value.ToString("dd.MM.yyyy"),
                ConceptualEquipmentList = project.ConceptualEquipmentList != null ? project.ConceptualEquipmentList.Value : false,
                ConceptualOra = project.ConceptualOra != null ? project.ConceptualOra.Value : false,
                ConceptualPreliminaryExecutionPlan = project.ConceptualPreliminaryExecutionPlan != null ? project.ConceptualPreliminaryExecutionPlan.Value : false,
                ConceptualPreliminaryFundingPackage = project.ConceptualPreliminaryFundingPackage != null ? project.ConceptualPreliminaryFundingPackage.Value : false,
                ConceptualPreliminaryLayouts = project.ConceptualPreliminaryLayouts != null ? project.ConceptualPreliminaryLayouts.Value : false,
                ConceptualPreliminarySourcingPlan = project.ConceptualPreliminarySourcingPlan != null ? project.ConceptualPreliminarySourcingPlan.Value : false,
                ConceptualUrd = project.ConceptualUrd != null ? project.ConceptualUrd.Value : false,
                DefinitionActualEnd = project.DefinitionActualEnd != null ? project.DefinitionActualEnd.Value.ToString("dd.MM.yyyy") : "none",
                DefinitionComments = project.DefinitionComments,
                DefinitionDesignBasis = project.DefinitionDesignBasis != null ? project.DefinitionDesignBasis.Value : false,
                DefinitionDone = project.DefinitionDone,
                DefinitionEnd = project.DefinitionEnd.Value.ToString("dd.MM.yyyy"),
                DefinitionEquipmentSpecification = project.DefinitionEquipmentSpecification != null ? project.DefinitionEquipmentSpecification.Value : false,
                DefinitionFinalLayout = project.DefinitionFinalLayout != null ? project.DefinitionFinalLayout.Value : false,
                DefinitionFullStartCapitalEstimate = project.DefinitionFullStartCapitalEstimate != null ? project.DefinitionFullStartCapitalEstimate.Value : false,
                DefinitionFullStartFunding = project.DefinitionFullStartFunding != null ? project.DefinitionFullStartFunding.Value : false,
                DefinitionLevel2 = project.DefinitionLevel2 != null ? project.DefinitionLevel2.Value : false,
                DefinitionProjectExecutionPlan = project.DefinitionProjectExecutionPlan != null ? project.DefinitionProjectExecutionPlan.Value : false,
                DefinitionRequestForQuotations = project.DefinitionRequestForQuotations != null ? project.DefinitionRequestForQuotations.Value : false,
                DesignConstructActualEnd = project.DesignConstructActualEnd != null ? project.DesignConstructActualEnd.Value.ToString("dd.MM.yyyy") : "none",
                DesignConstructcCompleted = project.DesignConstructcCompleted,
                DesignConstructComments = project.DesignConstructComments,
                DesignConstructEnd = project.DesignConstructEnd.Value.ToString("dd.MM.yyyy"),
                DesignConstructionPlan = project.DesignConstructionPlan != null ? project.DesignConstructionPlan.Value : false,
                DesignDetailedAnssembly = project.DesignDetailedAnssembly != null ? project.DesignDetailedAnssembly.Value : false,
                DesignEngineeringInformation = project.DesignEngineeringInformation != null ? project.DesignEngineeringInformation.Value : false,
                DesignIqpqprotocol = project.DesignIqpqprotocol != null ? project.DesignIqpqprotocol.Value : false,
                DesignLevel3 = project.DesignLevel3 != null ? project.DesignLevel3.Value : false,
                DesignVendorAcceptance = project.DesignVendorAcceptance != null ? project.DesignVendorAcceptance.Value : false,
                Eiresource = project.Eiresource,
                Etc = project.Etc ?? 0,
                FesabilityActualEnd = project.FesabilityActualEnd != null ? project.FesabilityActualEnd.Value.ToString("dd.MM.yyyy") : "none",
                FesabilityCapitalEstimate = project.FesabilityCapitalEstimate != null ? project.FesabilityCapitalEstimate.Value : false,
                FesabilityCharter = project.FesabilityCharter != null ? project.FesabilityCharter.Value : false,
                FesabilityComments = project.FesabilityComments,
                FesabilityCompletetd = project.FesabilityCompletetd,
                FesabilityEnd = project.FesabilityEnd.Value.ToString("dd.MM.yyyy"),
                FesabilityLevel1 = project.FesabilityLevel1 != null ? project.FesabilityLevel1.Value : false,
                FesabilityPreliminaryEquipmentList = project.FesabilityPreliminaryEquipmentList != null ? project.FesabilityPreliminaryEquipmentList.Value : false,
                FiscalYearStart = project.FiscalYearStart,
                FundingType = project.FundingType,
                Id = project.Id,
                ImpactedDepartment = project.ImpactedDepartment,
                LeadingDepartment = project.LeadingDepartment,
                MetSuccesCriteria = project.MetSuccesCriteria,
                Ora = project.Ora ?? 0,
                OtherComments = project.OtherComments,
                PcisResource = project.PcisResource ?? "none",
                PlantAe = project.PlantAe,
                PredictedEndDate = project.PredictedEndDate.ToString("dd.MM.yyyy"),
                PredictedEndFiscalYear = project.PredictedEndFiscalYear,
                Priority = project.Priority,
                ProjectLeader = project.ProjectLeader,
                ProjectName = project.ProjectName,
                ProjectType = project.ProjectType,
                Ptresource = project.Ptresource ?? "none",
                StartDate = project.StartDate.ToString("dd.MM.yyyy"),
                StartupAppropriationCloseOut = project.StartupAppropriationCloseOut != null ? project.StartupAppropriationCloseOut.Value : false,
                StartupComments = project.StartupComments,
                StartupLeader = project.StartupLeader,
                Status = project.Status,
                TeamCharter = project.TeamCharter
            };

            return Json(model);
        }

        private List<SavingsListItem> RetrieveSavings()
        {
            var projects = ctx.Masterplan.ToList();
            var list = new List<SavingsListItem>();
            foreach (var project in projects)
            {
                list.Add(new SavingsListItem
                {
                    ProjectName = project.ProjectName,
                    ProjectLeader = project.ProjectLeader,
                    DueDate = project.PredictedEndDate.ToString("dd.MM.yyyy"),
                    Savings = project.Saving.ToString()
                });
            }

            return list;
        }

        private List<LateProjectListItem> RetrieveLateProjects()
        {
            var projects = ctx.Masterplan.ToList();
            var list = new List<LateProjectListItem>();
            foreach(var project in projects)
            {
                var late = ctx.ProjectLate.FirstOrDefault(x => x.ProjectId == project.Id);
                if (CheckIfIsLate(late))
                {
                    list.Add(new LateProjectListItem
                    {
                        ConceptualComments = project.ConceptualComments,
                        ConceptualLate = late.ConceptualLate,
                        DefinitionComments = project.DefinitionComments,
                        DefinitionLate = late.DefinitionLate,
                        DesignConstructComments = project.DesignConstructComments,
                        DesignConstructLate = late.DesignConstructLate,
                        FeasibilityComments = project.FesabilityComments,
                        FeasibilityLate = late.FesabilityLate,
                        ProjectName = project.ProjectName,
                        StartupComments = project.StartupComments,
                        StartupLate = late.StartupLate
                    });
                }
            }

            return list;
        }

        private List<OngoingProjectsListItem> RetrieveOngoingProjects()
        {
            var projects = ctx.Masterplan.Where(x => x.Status == "IN PROGRESS").ToList();
            var list = new List<OngoingProjectsListItem>();

            foreach(var project in projects)
            {
                var late = ctx.ProjectLate.FirstOrDefault(x => x.ProjectId == project.Id);
                list.Add(new OngoingProjectsListItem
                {
                    ProjectName = project.ProjectName,
                    DueDate = project.PredictedEndDate.ToString("dd.MM.yyyy"),
                    StartDate = project.StartDate.ToString("dd.MM.yyyy"),
                    IsLate = CheckIfIsLate(late)
                });
            }

            return list;
        }

        private List<Top3ListItem> RetrieveTop3Priorities()
        {
            var tops = ctx.Top3.ToList();
            var list = new List<Top3ListItem>();

            foreach (var top in tops)
                list.Add(new Top3ListItem
                {
                    ProjectLeader = top.ProjectLeader,
                    Priority1 = top.Option1,
                    Priority2 = top.Option2,
                    Priority3 = top.Option3
                });

            return list;
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult Savings()
        {
            var model = RetrieveSavings();

            HttpContext.JsReportFeature().Recipe(Recipe.PhantomPdf);
            return View(model);
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult OngoingProjects()
        {
            var model = RetrieveOngoingProjects();
            HttpContext.JsReportFeature().Recipe(Recipe.PhantomPdf);

            return View(model);
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult LateProjects()
        {
            var model = RetrieveLateProjects();
            HttpContext.JsReportFeature().Recipe(Recipe.PhantomPdf);

            return View(model);
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult Top3()
        {
            var model = RetrieveTop3Priorities();
            HttpContext.JsReportFeature().Recipe(Recipe.PhantomPdf);

            return View(model);
        }

        private bool CheckIfIsLate(ProjectLate latency)
        {
            return latency.FesabilityLate || latency.ConceptualLate || latency.DefinitionLate || latency.DesignConstructLate || latency.StartupLate;
        }
    }
}
