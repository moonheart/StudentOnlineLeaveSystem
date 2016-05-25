using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    [Authorize]
    public class ResumeController : Controller
    {
        #region Fields

        private IResumeManager _resumeManager;

        public IResumeManager ResumeManager
        {
            get { return _resumeManager ?? HttpContext.GetOwinContext().GetUserManager<ResumeManager>(); }
            set { _resumeManager = value; }
        }

        private LeaveManager _leaveManager;

        public LeaveManager LeaveManager
        {
            get { return _leaveManager ?? HttpContext.GetOwinContext().GetUserManager<LeaveManager>(); }
            private set { _leaveManager = value; }
        }

        #endregion

        // GET: Resume
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            ViewBag.Resumes = await ResumeManager.GetAllEntities()
                  .Where(e => e.RecieveTeacher.TeacherId == userId)
                  .Where(e => e.ResumeApplyType == ResumeApplyType.未处理)
                  .ToListAsync();
            return View();
        }

        public async Task<ActionResult> History()
        {
            var userId = User.Identity.GetUserId();
            ViewBag.Resumes = await ResumeManager.GetAllEntities()
                  .Where(e => e.RecieveTeacher.TeacherId == userId)
                  .ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Process(ResumeProcessViewModel model)
        {
            if (ModelState.IsValid)
            {
                var leave = await LeaveManager.FindEntityByIdAsync(model.LeaveId);
                if (leave == null)
                    return View("Error", new[] { "找不到对象" });
                leave.ResumeApply.ResumeApplyType = model.ResumeApplyType;
                leave.ResumeApply.RecieveTeacher = leave.ResumeApply.RecieveTeacher;
                leave.ResumeApply.ProcessTime = DateTime.Now;
                leave.ResumeTime = DateTime.Now;
                leave.IsResume = ResumeStatus.已销假;
                await LeaveManager.UpdateEntityAsync(leave);
            }
            return RedirectToAction("Index");
        }
    }
}