using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region 创建角色
        [HttpGet]
        [Route("CreateRole")]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel roleViewModel)
        {
            if (ModelState.IsValid) {
                var identityRole = new IdentityRole {
                    Name = roleViewModel.RoleName
                };
                var result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded) {
                    return RedirectToAction("ListRoles", "Admin");
                }
                foreach (var error in result.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(roleViewModel);
        }

        #endregion

        [HttpGet]
        [Route("ListRoles")]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        [Route("EditRole")]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null) {
                ViewBag.ErrorMessage = $"角色Id={id}不存在，请重试";
                return View("NotFound");
            }
            var model = new EditRoleViewModel {
                Id = role.Id,
                RoleName = role.Name
            };
            var users = userManager.Users.ToList();
            foreach (var user in users) {
                if (await userManager.IsInRoleAsync(user, role.Name)) {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

        [HttpPost]
        [Route("EditRole")]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role==null) {
                ViewBag.ErrorMessage = $"角色Id={model.Id}不存在，请重试";
                return View("NotFound");
            }
            else {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded) {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }


    }
}
