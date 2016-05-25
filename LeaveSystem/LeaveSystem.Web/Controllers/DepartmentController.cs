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
    public class DepartmentController : Controller
    {
        private IDepartmentManager _departmentManager;

        public IDepartmentManager DepartmentManager
        {
            get { return _departmentManager ?? HttpContext.GetOwinContext().GetUserManager<DepartmentManager>(); }
            set { _departmentManager = value; }
        }

        private IMajorManager _majorManager;

        public IMajorManager MajorManager
        {
            get { return _majorManager ?? HttpContext.GetOwinContext().GetUserManager<MajorManager>(); }
            set { _majorManager = value; }
        }



        #region Department

        public ActionResult Index(int? p, string searchString, string currentFilter)
        {
            var list = DepartmentManager.GetAllEntities();
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
            ViewBag.Departments = await DepartmentManager.GetAllEntities().ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DepartmentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Name))
                {
                    var depart = await DepartmentManager.FindDepartmentByNameAsync(model.Name);
                    if (depart == null)
                    {
                        depart = new Department() { Name = model.Name };
                        await DepartmentManager.AddEntityAsync(depart);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "已有相同名称");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "不能为空");
                }
            }
            return View(model);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            var de = await DepartmentManager.FindEntityByIdAsync(id ?? 0);
            if (de == null)
                return View("Error", new[] { "学院不存在" });
            ViewBag.InMajors = await MajorManager.GetMajorsForDepartment(de).ToListAsync();
            ViewBag.OutMajors = await MajorManager.FindNoDepartmentMajors().ToListAsync();
            var model = new DepartmentEditViewModel { Id = de.Id, Name = de.Name };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(DepartmentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var de = await DepartmentManager.FindEntityByIdAsync(model.Id);
                if (de != null)
                {
                    if (!string.IsNullOrEmpty(model.Name))
                        de.Name = model.Name;
                    await DepartmentManager.UpdateEntityAsync(de);
                    await MajorManager.SetMajorsDepartmentAsync(model.IdsToAdd ?? new int[] { }, de);
                    await MajorManager.ResetMajorsDepartmentAsync(model.IdsToRemove ?? new int[] { });
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", new[] { "学院不存在" });
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
                var major = await DepartmentManager.FindEntityByIdAsync(id ?? 0);
                if (major != null)
                {
                    await DepartmentManager.DeleteEntityAsync(major);
                }
            }
            return RedirectToAction("Index");
        }


        #endregion
    }
}