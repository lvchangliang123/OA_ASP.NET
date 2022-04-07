using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Route("Admin")]
    //[Authorize(Roles ="Admin")]
    //只有管理员角色才能管理角色
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AdminController> logger;
        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger<AdminController> logger)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.logger = logger;
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

        #region 遍历角色列表
        [HttpGet]
        [Route("ListRoles")]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }
        #endregion

        #region 编辑角色
        [HttpGet]
        [Route("EditRole")]
        [Authorize(Policy ="EditRolePolicy")]
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
        #endregion

        #region 在角色信息中编辑用户
        [HttpGet]
        [Route("EditUsersInRole")]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);
            if (role==null) {
                ViewBag.ErrorMessage = $"角色Id={roleId}的信息不存在，请重试！";
                return View("NotFound");
            }
            var model = new List<UserRoleViewModel>();
            var users = userManager.Users.ToList();
            foreach (var user in users) {
                var userRoleViewModel = new UserRoleViewModel {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                var isInRole = await userManager.IsInRoleAsync(user, role.Name);
                if (isInRole) {
                    userRoleViewModel.IsSelected = true;
                }
                else {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }
            return View(model);
        }

        [HttpPost]
        [Route("EditUsersInRole")]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> models,string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null) {
                ViewBag.ErrorMessage = $"角色Id={roleId}的信息不存在，请重试！";
                return View("NotFound");
            }
            for (int i = 0; i < models.Count; i++) {
                var user = await userManager.FindByIdAsync(models[i].UserId);
                IdentityResult result = null;
                if (models[i].IsSelected&&!(await userManager.IsInRoleAsync(user, role.Name))) {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!models[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name)) {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else {
                    continue;
                }
                if (result.Succeeded) {
                    if (i<(models.Count-1)) {
                        continue;
                    }
                    else {
                        return RedirectToAction("EditRole", new { id = roleId });
                    }
                }
            }
            return RedirectToAction("EditRole", new { id = roleId });
        }
        #endregion

        #region 用户管理列举所有注册用户
        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users.ToList();
            return View(users);
        }
        #endregion

        #region 编辑用户
        [HttpGet]
        [Route("EditUser")]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user==null) {
                ViewBag.ErrorMessage = $"无法找到ID为{id}的用户";
                return View("NotFound");
            }
            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);
            var model = new EditUserViewModel {

                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                City = user.City,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles.ToList()
            };
            return View(model);
        }
        [HttpPost]
        [Route("EditUser")]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user==null) {
                ViewBag.ErrorMessage = $"无法找到ID为{model.Id}的用户";
                return View("NotFound");
            }
            else {
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.City = model.City;
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded) {
                    return RedirectToAction("ListUsers");
                }
                foreach (var error in result.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);

        }

        #endregion

        #region 删除用户

        [HttpPost]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user==null) {
                ViewBag.ErrorMessage = $"无法找到ID为{id}的用户";
                return View("NotFound");
            }
            else {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded) {
                    return RedirectToAction("ListUsers");
                }
                foreach (var error in result.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListUsers");
            }
        }

        #endregion

        #region 删除角色
        [HttpPost]
        [Route("DeleteRole")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByNameAsync(id);
            if (role==null) {
                ViewBag.ErrorMessage = $"无法找到ID为{id}的用户";
                return View("NotFound");
            }
            else {

                try {
                    var result = await roleManager.DeleteAsync(role);
                    if (result.Succeeded) {
                        return RedirectToAction("ListRoles");
                    }
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("ListRoles");
                }
                catch (Exception ex) {

                    if (ex.GetType()==typeof(DbUpdateException)) {
                        //说明无法删除角色，该角色中已经存在了用户信息
                        logger.LogError($"删除角色过程发生异常:{ex.Message}");
                        ViewBag.ErrorTitle = $"角色{role.Name}正在被部分用户使用中!";
                        ViewBag.ErrorMessage = $"无法删除{role.Name}角色，因为该角色中已经存在用户。若想删除此角色，请先删除角色中的所有用户，再删除此角色.";
                        return View("Error");
                    }
                    else {
                        return View("Error");
                    }
                }

            }
        }
        #endregion

        #region 管理用户的所有角色
        [HttpGet]
        [Route("ManageUserRoles")]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) {
                ViewBag.ErrorMessage = $"无法找到ID为{userId}的用户";
                return View("NotFound");
            }
            var model = new List<RolesInUserViewModel>();
            foreach (var role in await roleManager.Roles.ToListAsync()) {
                var rolesInUserViewModel = new RolesInUserViewModel {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await userManager.IsInRoleAsync(user,role.Name)) {
                    rolesInUserViewModel.IsSelected = true;
                }
                else {
                    rolesInUserViewModel.IsSelected = false;
                }
                model.Add(rolesInUserViewModel);
            }
            return View(model);
        }

        [HttpPost]
        [Route("ManageUserRoles")]
        public async Task<IActionResult> ManageUserRoles(List<RolesInUserViewModel> model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) {
                ViewBag.ErrorMessage = $"无法找到ID为{userId}的用户";
                return View("NotFound");
            }
            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded) {
                ModelState.AddModelError("", "无法删除用户中的现有角色");
                return View(model);
            }
            result = await userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));
            if (!result.Succeeded) {
                ModelState.AddModelError("", "无法向用户添加选定的角色");
                return View(model);
            }
            return RedirectToAction("EditUser", new { Id = userId });
        }


        #endregion

        #region 管理用户的所有声明

        [HttpGet]
        [Route("ManageUserClaims")]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) {
                ViewBag.ErrorMessage = $"无法找到ID为{userId}的用户";
                return View("NotFound");
            }
            var existingUserClaims = await userManager.GetClaimsAsync(user);
            var model = new UserClaimsViewModel {
                UserId = userId
            };
            foreach (var claim in ClaimsStore.AllClaims) {
                UserClaim userClaim = new UserClaim {
                    ClaimType = claim.Type
                };
                if (existingUserClaims.Any(c=>c.Type==claim.Type)) {
                    userClaim.IsSelected = true;
                }
                model.Claims.Add(userClaim);
            }
            return View(model);
        }

        [HttpPost]
        [Route("ManageUserClaims")]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null) {
                ViewBag.ErrorMessage = $"无法找到ID为{model.UserId}的用户";
                return View("NotFound");
            }
            //获取并删除用户的所有声明
            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);
            if (!result.Succeeded) {
                ModelState.AddModelError("", "无法删除当前用户的声明");
                return View(model);
            }
            //添加页面上选中的所有声明信息
            result = await userManager.AddClaimsAsync(user, model.Claims.Where(c => c.IsSelected).Select(c => new System.Security.Claims.Claim(c.ClaimType, c.ClaimType)));
            if (!result.Succeeded) {
                ModelState.AddModelError("", "无法向用户添加选定的声明");
                return View(model);
            }
            return RedirectToAction("EditUser", new { Id = model.UserId });

        }


        #endregion

        #region 拒绝访问路由
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion

    }
}
