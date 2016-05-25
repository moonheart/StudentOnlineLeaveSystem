using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LeaveSystem.Web.BLL;
using LeaveSystem.Web.IBLL;
using LeaveSystem.Web.Models;
using Microsoft.AspNet.Identity.Owin;

namespace LeaveSystem.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LeaveConfigController : Controller
    {
        private UserManager _userManager;

        public UserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>(); }
            set { _userManager = value; }
        }

        private ILeaveConfigManager _leaveConfigManager;

        public ILeaveConfigManager LeaveConfigManager
        {
            get { return _leaveConfigManager ?? HttpContext.GetOwinContext().GetUserManager<LeaveConfigManager>(); }
            set { _leaveConfigManager = value; }
        }

        // GET: LeaveConfig
        public async Task<ActionResult> Index()
        {
            var config = await LeaveConfigManager.GetConfigAsync();
            return View(config);
        }

        public async Task<ActionResult> Edit()
        {
            var config = await LeaveConfigManager.GetConfigAsync();
            var teachers = await UserManager.GetInPositionTeachersAsync();
            ViewBag.Teachers = new SelectList(teachers, "Id", "Name");
            return View(config);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LeaveConfig config)
        {
            var teachers = await UserManager.GetInPositionTeachersAsync();
            ViewBag.Teachers = new SelectList(teachers, "Id", "Name");
            if (ModelState.IsValid)
            {
                await LeaveConfigManager.SetConfigAsync(config);
                return RedirectToAction("Index");
            }
            return View(config);
        }

    }
}