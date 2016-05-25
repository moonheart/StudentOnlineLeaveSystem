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
using Microsoft.AspNet.Identity.Owin;
using PagedList;

namespace LeaveSystem.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class GradeController : Controller
    {
        private IGradeManager _gradeManager;

        public IGradeManager GradeManager
        {
            get { return _gradeManager ?? HttpContext.GetOwinContext().GetUserManager<GradeManager>(); }
            set { _gradeManager = value; }
        }
        #region Grade

        public ActionResult Index(int? p, string searchString, string currentFilter)
        {
            var list = GradeManager.GetAllEntities();
            if (searchString != null)
            {
                p = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            list = list.Where(e => string.IsNullOrEmpty(searchString) || e.GradeNum.Contains(searchString));
            list = list.OrderBy(e => e.GradeNum);
            var pagesize = 5;
            var page = p ?? 1;
            return View(list.ToPagedList(page, pagesize));

        }

        public async Task<ActionResult> Create()
        {
            ViewBag.Grades = await GradeManager.GetAllEntities().ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GradeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var grade = await GradeManager.FindGradeByGradeNumberAsync(model.GradeNumber);
                if (grade == null)
                {
                    var instance = new Grade() { GradeNum = model.GradeNumber };
                    await GradeManager.AddEntityAsync(instance);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "已有相同名称");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id)
        {
            if (ModelState.IsValid)
            {
                var model = await GradeManager.FindEntityByIdAsync(id ?? 0);
                if (model != null)
                {
                    await GradeManager.DeleteEntityAsync(model);
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int? id)
        {
            var grade = await GradeManager.FindEntityByIdAsync(id ?? 0);
            if (grade == null)
                return HttpNotFound();
            var grades = await GradeManager.GetAllEntities().ToListAsync();
            ViewBag.Grades = grades;
            return View(new GradeEditViewModel { GradeNumber = grade.GradeNum, Id = grade.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(GradeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var grade = await GradeManager.FindEntityByIdAsync(model.Id);
                if (grade == null)
                {
                    ModelState.AddModelError("", "未找到此项grade");
                    return View(model);
                }
                grade.GradeNum = model.GradeNumber;
                await GradeManager.UpdateEntityAsync(grade);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        #endregion
    }
}