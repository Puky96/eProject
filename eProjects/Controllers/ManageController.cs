﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using eProjects.Models;
using eProjects.Models.ManageViewModels;
using eProjects.Services;
using eProjects.DBModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eProjects.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;
        private readonly eProjectsCTX ctx= new eProjectsCTX();
        private string defaultAssignedPassword = "3WFosqON°#LlU&5Cy&o(>c<WV_$SH,9A";

        private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        public ManageController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          RoleManager<IdentityRole> roleManager,
          IEmailSender emailSender,
          ILogger<ManageController> logger,
          UrlEncoder urlEncoder)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _urlEncoder = urlEncoder;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> ViewProjectLeaders(string message)
        {
            ViewBag.Message = message;

            var projectLeaders = await _userManager.GetUsersInRoleAsync("ProjectLeader");

            var ProjectLeaders = new List<ProjectLeaderItem>();
            foreach (var leader in projectLeaders)
                ProjectLeaders.Add(new ProjectLeaderItem {
                    Id = leader.Id,
                    UriName = leader.Fullname.Replace(" ","%20"),
                    Fullname = leader.Fullname,
                    Username = leader.UserName,
                    Role = "Project Leader"
                });

            var admins = await _userManager.GetUsersInRoleAsync("Administrator");
            admins.Remove(admins.FirstOrDefault(x => x.UserName == "URLT-Admin"));
            foreach (var admin in admins)
                ProjectLeaders.Add(new ProjectLeaderItem
                {
                    Id = admin.Id,
                    UriName = admin.Fullname.Replace(" ", "%20"),
                    Fullname = admin.Fullname,
                    Username = admin.UserName,
                    Role = "Administrator"
                });

            var resources = await _userManager.GetUsersInRoleAsync("Resource");
            foreach (var resource in resources)
                ProjectLeaders.Add(new ProjectLeaderItem
                {
                    Id = resource.Id,
                    UriName = resource.Fullname.Replace(" ", "%20"),
                    Fullname = resource.Fullname,
                    Username = resource.UserName,
                    Role = "Resource"
                });

            var sLeaders = await _userManager.GetUsersInRoleAsync("StartupLeader");
            foreach (var sLeader in sLeaders)
                ProjectLeaders.Add(new ProjectLeaderItem
                {
                    Id = sLeader.Id,
                    UriName = sLeader.Fullname.Replace(" ", "%20"),
                    Fullname = sLeader.Fullname,
                    Username = sLeader.UserName,
                    Role = "Startup Leader"
                });

            return View(ProjectLeaders.OrderBy(x => x.Username));
        }

        [Authorize(Roles ="Administrator")]
        public IActionResult AddProjectLeader(string message = null)
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddProjectLeader(AddProjectLeaderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if(user == null)
                {
                    try
                    {
                        var projectLeader = new ApplicationUser { UserName = model.Username, Email = model.Username + "@pg.com", Fullname = model.Fullname };
                        await _userManager.CreateAsync(projectLeader, defaultAssignedPassword);
                        var addedUser = await _userManager.FindByNameAsync(model.Username);
                        if(model.IsAdmin == false)
                            await _userManager.AddToRoleAsync(addedUser, "ProjectLeader");
                        else
                            await _userManager.AddToRoleAsync(addedUser, "Administrator");
                        return RedirectToAction("ViewProjectLeaders");
                    }
                    catch
                    {
                        return AddProjectLeader("Something went wrong. Please try again later!");
                    }
                }
                else
                {
                    return AddProjectLeader("User already in the system!");
                }
            }
            else
                return View(model);

        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteProjectLeader (string projectLeader)
        {
            var user = await _userManager.FindByIdAsync(projectLeader);
            await _userManager.DeleteAsync(user);

            return RedirectToAction("ViewProjectLeaders");
        }

        public IActionResult Top3()
        {
            ViewBag.IsAdmin = _userManager.IsInRoleAsync(_userManager.FindByNameAsync(User.Identity.Name).Result, "Administrator").Result;

            var userName = _userManager.FindByNameAsync(User.Identity.Name).Result.Fullname;
            var options = ctx.Top3.FirstOrDefault(x => x.ProjectLeader == userName);

            var projects = ctx.Masterplan.Where(x => x.ProjectLeader == userName).Select(x => x.ProjectName).ToList();
            if(options != null)
            {
                ViewBag.Options1 = new SelectList(projects, options.Option1);
                ViewBag.Options2 = new SelectList(projects, options.Option2);
                ViewBag.Options3 = new SelectList(projects, options.Option3);
            }
            else
            {
                ViewBag.Options1 = new SelectList(projects);
                ViewBag.Options2 = new SelectList(projects);
                ViewBag.Options3 = new SelectList(projects);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Top3(Top3ViewModel model)
        {
            var fullName = _userManager.FindByNameAsync(User.Identity.Name).Result.Fullname;
            var exists = ctx.Top3.FirstOrDefault(x => x.ProjectLeader == fullName);

            if (exists != null)
            {
                exists.Option1 = model.Option1;
                exists.Option2 = model.Option2;
                exists.Option3 = model.Option3;
                ctx.Top3.Update(exists);
                ctx.SaveChanges();
            }
            else
            {
                var newEntry = new DBModels.Top3
                {
                    ProjectLeader = fullName,
                    Option1 = model.Option1,
                    Option2 = model.Option2,
                    Option3 = model.Option3
                };
                ctx.Top3.Add(newEntry);
                ctx.SaveChanges();
            }

            return RedirectToAction("ViewProjects", "Project");
        }

        public JsonResult RetrievePriorities (string projectLeader)
        {
            var test = projectLeader;
            var options = ctx.Top3.FirstOrDefault(x => x.ProjectLeader == projectLeader);
            if(options != null)
            {
                var json2Send = new PriorityJson
                {
                    Type = "info",
                    Title = projectLeader + " Priorities",
                    Text = "Priority 1: " + (options.Option1 ?? "not defined") + "\nPriority 2: " + (options.Option2 ?? "not defined") + "\nPriority 3: " + (options.Option3 ?? "not defined")
                };

                return Json(json2Send);
            }
            else
            {
                var json2Send = new PriorityJson
                {
                    Type = "info",
                    Title = projectLeader + " Priorities",
                    Text = "Has no projects registered as priorities"
                };

                return Json(json2Send);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult ManageResources()
        {
            var startup = ctx.Leaders.ToList();
            ViewBag.SatrtupLeaders = new SelectList(startup, "Id", "Name");

            var pcis = ctx.Pcisresource.ToList();
            ViewBag.Pcis = new SelectList(pcis, "Id", "Name");

            var pt = ctx.Ptresource.ToList();
            ViewBag.Pt = new SelectList(pt, "Id", "Name");

            var ei = ctx.Eiresource.ToList();
            ViewBag.Ei = new SelectList(ei, "Id", "Name");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult ManageResources(string StartupLeader, string Pcis, string PT, string EI)
        {
            if (StartupLeader != null)
            {
                var sl = ctx.Leaders.FirstOrDefault(x => x.Id == Convert.ToInt32(StartupLeader));
                ctx.Leaders.Remove(sl);
                ctx.SaveChanges();
            }

            if (Pcis != null)
            {
                var pcis = ctx.Pcisresource.FirstOrDefault(x => x.Id == Convert.ToInt32(Pcis));
                ctx.Pcisresource.Remove(pcis);
                ctx.SaveChanges();
            }

            if (PT != null)
            {
                var pt = ctx.Ptresource.FirstOrDefault(x => x.Id == Convert.ToInt32(PT));
                ctx.Ptresource.Remove(pt);
                ctx.SaveChanges();
            }

            if (EI != null)
            {
                var ei = ctx.Eiresource.FirstOrDefault(x => x.Id == Convert.ToInt32(EI));
                ctx.Eiresource.Remove(ei);
                ctx.SaveChanges();
            }


            return RedirectToAction("ViewProjectLeaders", "Manage", new { message = "Resources succesfullly deleted!" });
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddResource()
        {
            var resourceList = new List<string> { "Startup Leader", "PC&IS", "P&T", "E&I" };
            ViewBag.Options = new SelectList(resourceList);

            return View();
        }

        [HttpPost]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> AddResource(AddResourceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.FindByNameAsync(model.UserName);
                if(result == null)
                {
                    var user = new ApplicationUser { UserName = model.UserName, Fullname = model.Fullname, Email = model.UserName + "@pg.com" };
                    await _userManager.CreateAsync(user, defaultAssignedPassword);
                }
                else
                {
                    model.Fullname = result.Fullname;
                }

                var usr = await _userManager.FindByNameAsync(model.UserName);
                
                if(model.ResourceType == "Startup Leader")
                {
                    ctx.Leaders.Add(new Leaders
                    {
                        Name = model.Fullname
                    });
                    ctx.SaveChanges();
                    await _userManager.AddToRoleAsync(usr, "StartupLeader");
                }

                if(model.ResourceType == "PC&IS")
                {
                    ctx.Pcisresource.Add(new Pcisresource
                    {
                        Name = model.Fullname
                    });
                    ctx.SaveChanges();
                    await _userManager.AddToRoleAsync(usr, "Resource");
                }

                if(model.ResourceType == "P&T")
                {
                    ctx.Ptresource.Add(new Ptresource
                    {
                        Name = model.Fullname
                    });
                    ctx.SaveChanges();
                    await _userManager.AddToRoleAsync(usr, "Resource");
                }

                if(model.ResourceType == "E&I")
                {
                    ctx.Eiresource.Add(new Eiresource
                    {
                        Name = model.Fullname
                    });
                    ctx.SaveChanges();
                    await _userManager.AddToRoleAsync(usr, "Resource");
                }

                return RedirectToAction("ViewProjectLeaders", "Manage", new { message = "Resource succesfully added!" });
            }

            return View(model);
        }






        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenicatorUriFormat,
                _urlEncoder.Encode("eProjects"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }

        #endregion
    }
}
