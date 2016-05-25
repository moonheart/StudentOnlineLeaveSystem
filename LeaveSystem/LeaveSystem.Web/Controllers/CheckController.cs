using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LeaveSystem.Web.BLL;
using LeaveSystem.Web.IBLL;
using LeaveSystem.Web.Models;
using LeaveSystem.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace LeaveSystem.Web.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class CheckController : Controller
    {
        #region fields
        private UserManager _userManager;

        public UserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>(); }
            set { _userManager = value; }
        }

        private ICheckManager _checkManager;

        public ICheckManager CheckManager
        {
            get { return _checkManager ?? HttpContext.GetOwinContext().GetUserManager<CheckManager>(); }
            set { _checkManager = value; }
        }
        #endregion

        // GET: Check
        public async Task<ActionResult> Index()
        {
            var userid = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userid);
            if (user == null)
                return View("Error", new[] { "找不到用户" });
            ViewBag.Checks = await CheckManager.GetToCheckAsync(user);
            return View();
        }

        public async Task<ActionResult> History()
        {
            var userid = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userid);
            if (user == null)
                return View("Error", new[] { "找不到用户" });
            ViewBag.Checks = await CheckManager.GetTeachersCheckAsync(user);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Approve(CheckApprovalViewModel model)
        {
            if (ModelState.IsValid)
            {
                // todo 添加拒绝处理
                var check = await CheckManager.FindEntityByIdAsync(model.CheckId);
                if (check == null)
                    return View("Error", new[] { "该项不存在" });
                check.CheckStatus = (model.Option == 0 ? CheckStatus.已通过 : CheckStatus.未通过);
                check.CheckOpinion = model.CheckOpinion;
                check.CheckTime = DateTime.Now;
                check.CheckTeacher = check.CheckTeacher;
                if (model.Option == 0)
                {
                    if (check.CheckOrder == 0 && check.Leave.Checks.Count == 2)
                    {
                        check.Leave.LeaveStatus= LeaveStatus.请假审核中;
                    }
                    else
                    {
                        check.Leave.LeaveStatus = LeaveStatus.已通过;
                    }
                }
                else
                {
                    check.Leave.LeaveStatus = LeaveStatus.已拒绝;
                }
                check.Leave = check.Leave;
                await CheckManager.UpdateEntityAsync(check);
                return RedirectToAction("Index");
            }
            return View("Error");
        }

        public async Task<ActionResult> Detail(int? id)
        {
            var check = await CheckManager.FindEntityByIdAsync(id ?? 0);
            if (check == null)
                return View("Error", new[] { "找不到对象" });
            var userId = User.Identity.GetUserId();
            if (check.CheckTeacher.Teacher.Id != userId)
                return View("Error", new[] { "找不到对象" });
            ViewBag.Leave = check.Leave;
            return
                View(new CheckApprovalViewModel
                {
                    CheckId = check.Id,
                    CheckOpinion = check.CheckOpinion,
                    Option = (check.CheckStatus == CheckStatus.已通过 ? 0 : 1),
                    Status = check.CheckStatus
                });
        }

    }
}