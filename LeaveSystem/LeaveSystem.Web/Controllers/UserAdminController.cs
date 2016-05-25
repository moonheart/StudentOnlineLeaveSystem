using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using LeaveSystem.Web.BLL;
using LeaveSystem.Web.Infrastructure;
using LeaveSystem.Web.Models;
using LeaveSystem.Web.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;

namespace LeaveSystem.Web.Controllers
{
    [CustomAuthorizeAttribute(Roles = "Administrator")]
    public class UserAdminController : Controller
    {
        #region 属性与构造方法
        private UserManager _userManager;
        private RoleManager _roleManager;

        public UserAdminController()
        {
        }

        public UserAdminController(UserManager userManager, RoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public UserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public RoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().GetUserManager<RoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        #endregion


        // GET: UserAdmin
        [CustomAuthorize(Permisson = Permissions.查看用户)]
        public async Task<ActionResult> Index(int? p, string searchString, string currentFilter, string selectedRole)
        {
            var roles = await RoleManager.Roles.ToListAsync();
            ViewBag.Roles = roles;
            ViewBag.SelectedRoleId = selectedRole;
            ViewBag.SelectedRole = new SelectList(roles, "Id", "Name", selectedRole);
            if (searchString != null)
            {
                p = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var list = UserManager.Users.Where(
                    u =>
                        string.IsNullOrEmpty(selectedRole) ||
                        u.Roles.Select(r => r.RoleId).Any(ri => ri == selectedRole));

            if (!string.IsNullOrEmpty(searchString))
            {
                list = list.Where(s => s.Name.Contains(searchString));
            }
            list = list.OrderBy(s => s.Name);
            var pagesize = 5;
            var page = p ?? 1;
            //var list = UserManager.GetAllUsers().ToList().ToPagedList(page, pagesize);
            //ViewBag.PagedList = list;
            return View(list.ToPagedList(page, pagesize));
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.Roles = await RoleManager.GetNonAdminRoles().ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserCreateViewModel model)
        {
            ViewBag.Roles = await RoleManager.GetNonAdminRoles().ToListAsync();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new User { UserName = model.Number, Name = model.Name };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                user = await UserManager.FindByNameAsync(user.UserName);
                await UserManager.AddToRolesAsync(user.Id, model.RolesToAdd ?? new string[] { });
                return RedirectToAction("Index");
            }
            AddErrors(result);
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                if (user.UserName == "000000")
                {
                    return View("Error", new[] { "请勿删除管理员！" });
                }

                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                return View("Error", result.Errors);
            }
            return View("Error", new[] { "User Not Found" });
        }

        public async Task<ActionResult> Edit(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
                return View("Error", new[] { "User not exist" });
            var nowIds = user.Roles.Select(r => r.RoleId).ToArray();
            var inRoles = await RoleManager.Roles.Where(e => nowIds.Contains(e.Id)).ToListAsync();
            var outRoles = (await RoleManager.GetNonAdminRoles().ToListAsync()).Except(inRoles).ToList();
            ViewBag.OutRoles = outRoles;
            ViewBag.InRoles = inRoles;
            UserEditViewModel model = new UserEditViewModel
            {
                Id = user.Id,
                Number = user.UserName,
                Name = user.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.Id);

                if (user != null)
                {
                    if (user.Name == "000000")
                    {
                        return View("Error", new[] { "请勿修改管理员！" });
                    }

                    IdentityResult validPass = null;
                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        //验证密码是否满足要求
                        validPass = await UserManager.PasswordValidator.ValidateAsync(model.Password);
                        if (validPass.Succeeded)
                        {
                            user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.Password);
                        }
                        else
                        {
                            AddErrors(validPass);
                        }
                    }


                    if (validPass == null || validPass.Succeeded)
                    {
                        user.Name = model.Name;
                        IdentityResult result = await UserManager.UpdateAsync(user);

                        if (model.NamesToAdd != null)
                        {
                            var ids = model.NamesToAdd.Where(i => RoleManager.FindByNameAsync(i) != null).ToArray();
                            await UserManager.AddToRolesAsync(user.Id, ids);
                        }
                        if (model.NamesToRemove != null)
                        {
                            var ids = model.NamesToRemove.Where(i => RoleManager.FindByNameAsync(i) != null).ToArray();
                            await UserManager.RemoveFromRolesAsync(user.Id, ids);
                        }
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        AddErrors(result);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "无法找到改用户");
                }
            }
            return View(model);
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

    }
}