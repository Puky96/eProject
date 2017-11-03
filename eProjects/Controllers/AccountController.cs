using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using eProjects.Models;
using eProjects.Models.AccountViewModels;
using eProjects.Services;
using Novell.Directory.Ldap;
using eProjects.DBModels;

namespace eProjects.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private string defaultAssignedPassword = "3WFosqON°#LlU&5Cy&o(>c<WV_$SH,9A";
        private readonly eProjectsCTX ctx;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _logger = logger;
            ctx = new eProjectsCTX();
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    var user = await _userManager.FindByEmailAsync(model.UserName + "@pg.com");
                    CheckIfLate(user);
                    return RedirectToAction("ShowLandingPage");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult LDAP()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LDAP(LDAPViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if(user != null)
                {
                    var cn = new LdapConnection();
                    cn.Connect("pg.com", 389);
                    try
                    {
                        var usr = "EU\\" + model.UserName;
                        var psw = model.Password;
                        cn.Bind(usr, psw);

                        var result = await _signInManager.PasswordSignInAsync(model.UserName, defaultAssignedPassword, false, lockoutOnFailure: false);
                        var user1 = await _userManager.FindByEmailAsync(model.UserName + "@pg.com");
                        CheckIfLate(user1);

                        return RedirectToAction("ShowLandingPage");
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError(string.Empty, "Incercare de autentificare invalida!");
                        return View(model);
                    }
                }else
                {
                    ModelState.AddModelError(string.Empty, "Incercare de autentificare invalida!");

                    return View(model);
                }
            }else
            {
                return View(model);
            }
        }

        public async Task<IActionResult> ShowLandingPage()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Administrator"))
                return RedirectToAction("ViewProjectLeaders", "Manage");
            if(roles.Contains("ProjectLeader"))
                return RedirectToAction("ViewProjects", "Project");

            return RedirectToAction("Index", "Home");
        }

        private void CheckIfLate(ApplicationUser user)
        { 
            var projects = ctx.Masterplan.Where(x => x.ProjectLeader == user.Fullname && x.Status == "IN PROGRESS").ToList();
            var date2Compare = DateTime.Now;

            foreach(var project in projects)
            {
                var late = ctx.ProjectLate.FirstOrDefault(x => x.ProjectId == project.Id);
                if (late == null)
                {
                    ctx.ProjectLate.Add(new ProjectLate
                    {
                        ProjectId = project.Id,
                        ConceptualLate = false,
                        StartupLate = false,
                        FesabilityLate = false,
                        DesignConstructLate = false,
                        DefinitionLate = false
                    });
                    ctx.SaveChanges();
                }

                late = ctx.ProjectLate.FirstOrDefault(x => x.ProjectId == project.Id);

                if (date2Compare.CompareTo(project.FesabilityEnd) > 0 && project.FesabilityCompletetd == false)
                    late.FesabilityLate = true;

                if (date2Compare.CompareTo(project.ConceptualEnd) > 0 && project.ConceptualCompleted == false)
                    late.ConceptualLate = true;

                if (date2Compare.CompareTo(project.DesignConstructEnd) > 0 && project.DesignConstructcCompleted == false)
                    late.DesignConstructLate = true;

                if (date2Compare.CompareTo(project.DefinitionEnd) > 0 && project.DefinitionDone == false)
                    late.DefinitionLate = true;

                if (date2Compare.CompareTo(project.PredictedEndDate) > 0 && project.Status != "COMPLETE")
                    late.FesabilityLate = true;

                ctx.Update(late);
                ctx.SaveChanges();

            }
        }

        //public async Task<IActionResult> AddUserToRoles()
        //{
        //    IdentityRole role = new IdentityRole { Name = "Administrator" };
        //    IdentityRole role1 = new IdentityRole { Name = "ProjectLeader" };
        //    await _roleManager.CreateAsync(role);
        //    await _roleManager.CreateAsync(role1);
        //    var user = await _userManager.FindByNameAsync("URLT-Admin");
        //    await _userManager.AddToRoleAsync(user, "Administrator");


        //    return RedirectToAction("Index", "Home");
        //}

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
