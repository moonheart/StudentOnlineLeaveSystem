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
    public class OfficeController : Controller
    {
        private IOfficeManager _officeManager;

        public IOfficeManager OfficeManager
        {
            get { return _officeManager ?? HttpContext.GetOwinContext().GetUserManager<OfficeManager>(); }
            private set { _officeManager = value; }
        }

        private IPositionManager _positionManager;

        public IPositionManager PositionManager
        {
            get { return _positionManager ?? HttpContext.GetOwinContext().GetUserManager<PositionManager>(); }
            private set { _positionManager = value; }
        }
        private IDepartmentManager _departmentManager;

        public IDepartmentManager DepartmentManager
        {
            get { return _departmentManager ?? HttpContext.GetOwinContext().GetUserManager<DepartmentManager>(); }
            private set { _departmentManager = value; }
        }


        // GET: Office
        public ActionResult Index(int? p, string searchString, string currentFilter)
        {
            var list = OfficeManager.GetAllEntities();
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
            ViewBag.Positions = await PositionManager.FindNoOfficePositions().ToListAsync();
            ViewBag.Offices = await OfficeManager.GetAllEntities().ToListAsync();
            var de = await DepartmentManager.GetAllEntities().ToListAsync();
            ViewBag.SelectListDepart = new SelectList(de, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OfficeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await OfficeManager.IsEntityExistByLamdaAsync(e => e.Name == model.Name))
                {
                    ModelState.AddModelError("", "同名已存在");
                }
                else
                {
                    var dep = await DepartmentManager.FindEntityByIdAsync(model.DepartmentId);
                    if (dep == null)
                    {
                        ModelState.AddModelError("", "指定学院不存在");
                        return View(model);
                    }
                    var office = new Office()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Department = dep

                    };
                    await OfficeManager.AddEntityAsync(office);
                    office = await OfficeManager.FindOfficeByNameAsync(office.Name);
                    await PositionManager.SetPositionsOfficeAsync(model.IdsToAdd ?? new int[] { }, office);
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }


        public async Task<ActionResult> Edit(int? id)
        {
            var office = await OfficeManager.FindEntityByIdAsync(id ?? 0);
            if (office == null)
                return View("Error", new[] { "找不到该部门" });
            ViewBag.OutPositions = await PositionManager.FindNoOfficePositions().ToListAsync();
            ViewBag.InPositions = office.Positions.ToList();
            var model = new OfficeEditViewModel
            {
                DepartmentId = office.Department.Id,
                Description = office.Description,
                Id = office.Id,
                Name = office.Name
            };
            var de = await DepartmentManager.GetAllEntities().ToListAsync();
            ViewBag.SelectListDepart = new SelectList(de, "Id", "Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(OfficeEditViewModel model)
        {
            var de = await DepartmentManager.GetAllEntities().ToListAsync();
            ViewBag.SelectListDepart = new SelectList(de, "Id", "Name");

            if (ModelState.IsValid)
            {
                var office = await OfficeManager.FindEntityByIdAsync(model.Id);
                if (office == null)
                    return View("Error", new[] { "找不到该部门" });
                var depart = await DepartmentManager.FindEntityByIdAsync(model.DepartmentId);
                if (depart == null)
                    return View("Error", new[] { "找不到该学院" });
                office.Name = model.Name;
                office.Description = model.Description;
                office.Department = depart;
                await OfficeManager.UpdateEntityAsync(office);
                await PositionManager.ResetPositionsOfficeAsync(model.IdsToRemove ?? new int[] { });
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id)
        {
            if (ModelState.IsValid)
            {
                var office = await OfficeManager.FindEntityByIdAsync(id ?? 0);
                if (office == null)
                    return View("Error", new[] { "找不到目标" });
                await OfficeManager.DeleteEntityAsync(office);
            }
            return View("Index");
        }

    }
}