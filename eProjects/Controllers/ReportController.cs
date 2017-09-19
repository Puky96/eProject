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
                ViewBag.Message = "There are no projects to show timeline for! All completed or no project registered for " + leaderName + " yet";

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

        public IActionResult Test()
        {
            return View();
        }
    }
}
