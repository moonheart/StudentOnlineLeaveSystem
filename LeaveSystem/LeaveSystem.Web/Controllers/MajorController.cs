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
    public class MajorController : Controller
    {
        private IMajorManager _majorManager;

        public IMajorManager MajorManager
        {
            get { return _majorManager ?? HttpContext.GetOwinContext().GetUserManager<MajorManager>(); }
            set { _majorManager = value; }
        }
        private IDepartmentManager _departmentManager;

        public IDepartmentManager DepartmentManager
        {
            get { return _departmentManager ?? HttpContext.GetOwinContext().GetUserManager<DepartmentManager>(); }
            set { _departmentManager = value; }
        }


        #region Major

        public ActionResult Index(int? p, string searchString, string currentFilter)
        {
            var list = MajorManager.GetAllEntities();
            if (searchString != null)
            {
                p = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            list = list.Where(e => string.IsNullOrEmpty(searchString) || e.Name.Contains(searchString));
            list = list.OrderBy(e => e.Name);
            var pagesize = 5;
            var page = p ?? 1;
            return View(list.ToPagedList(page, pagesize));
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.Majors = await MajorManager.GetAllEntities().ToListAsync();
            var departments = await DepartmentManager.GetAllEntities().ToListAsync();
            SelectList selectList = new SelectList(departments, "Id", "Name");
            ViewBag.selectList = selectList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MajorCreateViewModel major)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(major.MajorName))
                {
                    ModelState.AddModelError("", "MajorName不能为空");
                }
                else if (major.DepartmentId == 0)
                {
                    ModelState.AddModelError("", "DepartmentId不能为空");
                }
                else
                {
                    var model = await MajorManager.FindMajorByNameAsync(major.MajorName);
                    if (model == null)
                    {
                        var instance = new Major() { Name = major.MajorName };
                        await MajorManager.AddEntityAsync(instance);
                        model = await MajorManager.FindMajorByNameAsync(major.MajorName);
                        var depart = await DepartmentManager.FindEntityByIdAsync(major.DepartmentId);
                        if (depart == null)
                        {
                            ModelState.AddModelError("", "找不到对应学院");
                        }
                        await MajorManager.SetMajorsDepartmentAsync(new[] { model.Id }, depart);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "已有相同名称");
                    }

                }
            }
            return View(major);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id)
        {
            if (ModelState.IsValid)
            {
                var major = await MajorManager.FindEntityByIdAsync(id ?? 0);
                if (major != null)
                {
                    await MajorManager.DeleteEntityAsync(major);
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int? id)
        {
            var major = await MajorManager.FindEntityByIdAsync(id ?? 0);
            if (major == null)
                return HttpNotFound();
            var departments = await DepartmentManager.GetAllEntities().ToListAsync();
            SelectList selectList = new SelectList(departments, "Id", "Name");
            ViewBag.selectList = selectList;
            var model = new MajorEditViewModel { Id = major.Id, DepartmentId = major.Department.Id, MajorName = major.Name };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MajorEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var major = await MajorManager.FindEntityByIdAsync(model.Id);
                if (major == null)
                {
                    ModelState.AddModelError("", "未找到此项major");
                    return View(model);
                }
                var depart = await DepartmentManager.FindEntityByIdAsync(model.DepartmentId);
                if (depart == null)
                {
                    ModelState.AddModelError("", "未找到此项depart");
                    return View(model);
                }
                major.Name = model.MajorName;
                major.Department = depart;
                await MajorManager.UpdateEntityAsync(major);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        #endregion
    }
}