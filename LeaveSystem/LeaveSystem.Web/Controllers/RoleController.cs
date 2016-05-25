using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LeaveSystem.Web.BLL;
using LeaveSystem.Web.Infrastructure;
using LeaveSystem.Web.Models;
using LeaveSystem.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace LeaveSystem.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoleController : Controller
    {
        #region 属性和构造函数
        private RoleManager _roleManager;
        private UserManager _userManager;

        public RoleController()
        {

        }

        public RoleController(RoleManager roleManager, UserManager userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public RoleManager RoleManager
        {
            get { return _roleManager ?? HttpContext.GetOwinContext().GetUserManager<RoleManager>(); }
            private set { _roleManager = value; }
        }

        public UserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>(); }
            private set { _userManager = value; }
        }
        #endregion

        // GET: Role
        public ActionResult Index()
        {
            var list = RoleManager.Roles;
            return View(list);
        }


        public ActionResult Create()
        {
            var model = new RoleCreateViewModel { Users = UserManager.Users.ToList() };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(RoleCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            IdentityResult result = await RoleManager.CreateAsync(new Role(model.Name) { Description = model.Description });
            if (result.Succeeded)
            {
                foreach (string userId in model.UserIdsToAdd ?? new string[] { })
                {
                    result = await UserManager.AddToRoleAsync(userId, model.Name);
                    if (!result.Succeeded)
                    {
                        AddErrorsFromResult(result);
                        return View(model);
                    }
                }
                return RedirectToAction("Index");
            }
            AddErrorsFromResult(result);
            return View(model);
        }

        public async Task<ActionResult> Edit(string id)
        {
            Role role = await RoleManager.FindByIdAsync(id);
            string[] memherIDs = role.Users.Select(x => x.UserId).ToArray();
            IEnumerable<User> members = UserManager.Users.Where(x => memherIDs.Any(y => y == x.Id));
            IEnumerable<User> nonMembers = UserManager.Users.Except(members);
            return View(new RoleEditViewModel()
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoleModificationViewModel model)
        {
            IdentityResult result;
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            foreach (string userId in model.IdsToAdd ?? new string[] { })
            {
                result = await UserManager.AddToRoleAsync(userId, model.RoleName);
                if (!result.Succeeded)
                {
                    return View("Error");
                }
            }
            foreach (var userId in model.IdsToDelete ?? new string[] { })
            {
                //演示用，正式部署时去掉
                var currentUser = await UserManager.FindByIdAsync(userId);
                if (currentUser.UserName == "000000" && model.RoleName == "Administrator")
                {
                    return View("Error");
                }

                result = await UserManager.RemoveFromRoleAsync(userId, model.RoleName);
                if (!result.Succeeded)
                {
                    return View("Error");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            Role role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                ModelState.AddModelError("", "无法找到该角色");
                return View("Error");
            }
            if (role.Name == "Administrator")
            {
                ModelState.AddModelError("", "不能删除管理员角色");
                return View("Error");
            }
            var result = await RoleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            AddErrorsFromResult(result);
            return View("Error");
        }



        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

    }
}