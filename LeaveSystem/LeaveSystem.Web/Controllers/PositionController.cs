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
    public class PositionController : Controller
    {

        #region fields
        private IPositionManager _positionManager;

        public IPositionManager PositionManager
        {
            get { return _positionManager ?? HttpContext.GetOwinContext().GetUserManager<PositionManager>(); }
            set { _positionManager = value; }
        }
        private IDepartmentManager _departmentManager;

        public IDepartmentManager DepartmentManager
        {
            get { return _departmentManager ?? HttpContext.GetOwinContext().GetUserManager<DepartmentManager>(); }
            set { _departmentManager = value; }
        }

        private IOfficeManager _officeManager;

        public IOfficeManager OfficeManager
        {
            get { return _officeManager ?? HttpContext.GetOwinContext().GetUserManager<OfficeManager>(); }
            set { _officeManager = value; }
        }


        private UserManager _userManager;

        public UserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>(); }
            set { _userManager = value; }
        }
        #endregion

        #region Major

        public ActionResult Index(int? p, string searchString, string currentFilter)
        {
            var list = PositionManager.GetAllEntities();
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
            ViewBag.Positions = await PositionManager.GetAllEntities().ToListAsync();

            var teachers = await UserManager.GetNoPositionTeachersAsync();
            ViewBag.selectListTeacher = new SelectList(teachers, "Id", "Name");

            var departs = await DepartmentManager.GetAllEntities().ToListAsync();
            ViewBag.selectListDepart = new SelectList(departs, "Id", "Name");

            var offices = await OfficeManager.GetOfficesForDepartment(departs.ElementAtOrDefault(0)).ToListAsync();
            ViewBag.selectListOffice = new SelectList(offices, "Id", "Name");

            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetOfficesOfDepartment(int? departId)
        {
            var depart = await DepartmentManager.FindEntityByIdAsync(departId ?? 0);
            if (depart == null)
            {
                return Json(new { error = 1, message = "找不到该Department" });
            }
            return Json(new { error = 0, message = "获取成功", data = depart.Offices.Select(m => new { m.Id, m.Name }) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PositionCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var teachers = await UserManager.GetNoPositionTeachersAsync();
                ViewBag.selectListTeacher = new SelectList(teachers, "Id", "Name");

                var departs = await DepartmentManager.GetAllEntities().ToListAsync();
                ViewBag.selectListDepart = new SelectList(departs, "Id", "Name");

                var offices = await OfficeManager.GetOfficesForDepartment(departs.ElementAtOrDefault(0)).ToListAsync();
                ViewBag.selectListOffice = new SelectList(offices, "Id", "Name");

                var user = await UserManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    ModelState.AddModelError("", "用户不存在");
                    return View(model);
                }
                var office = await OfficeManager.FindEntityByIdAsync(model.OfficeId);
                if (office == null)
                {
                    ModelState.AddModelError("", "部门不存在");
                    return View(model);
                }
                var position = new Position()
                 {
                     Name = model.Name,
                     Description = model.Description,
                     HeadUser = user.TeacherInfo,
                     Office = office
                 };
                position = await PositionManager.AddEntityAsync(position);
                user.TeacherInfo.Position = position;
                await UserManager.UpdateAsync(user);
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
                var position = await PositionManager.FindEntityByIdAsync(id ?? 0);
                if (position != null)
                {
                    var user = await UserManager.FindByIdAsync(position.HeadUser.Teacher.Id);
                    user.TeacherInfo.Position = null;
                    await UserManager.UpdateAsync(user);
                    await PositionManager.DeleteEntityAsync(position);
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int? id)
        {
            var position = await PositionManager.FindEntityByIdAsync(id ?? 0);
            if (position == null)
                return HttpNotFound();

            ViewBag.Positions = await PositionManager.GetAllEntities().ToListAsync();

            var departs = await DepartmentManager.GetAllEntities().ToListAsync();
            ViewBag.selectListDepart = new SelectList(departs, "Id", "Name");

            var offices = await OfficeManager.GetOfficesForDepartment(departs.ElementAtOrDefault(0)).ToListAsync();
            ViewBag.selectListOffice = new SelectList(offices, "Id", "Name");

            var teachers = await UserManager.GetNoPositionTeachersAsync();
            ViewBag.selectListTeacher = new SelectList(teachers, "Id", "Name");

            var model = new PositionEditViewModel
            {
                Id = position.Id,
                Description = position.Description,
                Name = position.Name,
                UserId = position.HeadUser.Teacher.Id,
                OfficeId = position.Office.Id
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PositionEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var departs = await DepartmentManager.GetAllEntities().ToListAsync();
                ViewBag.selectListDepart = new SelectList(departs, "Id", "Name");

                var offices = await OfficeManager.GetOfficesForDepartment(departs.ElementAtOrDefault(0)).ToListAsync();
                ViewBag.selectListOffice = new SelectList(offices, "Id", "Name");

                var teachers = await UserManager.GetNoPositionTeachersAsync();
                ViewBag.selectListTeacher = new SelectList(teachers, "Id", "Name");



                var position = await PositionManager.FindEntityByIdAsync(model.Id);
                if (position != null)
                {
                    var user = await UserManager.FindByIdAsync(model.UserId);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "用户不存在");
                        return View(model);
                    }
                    var office = await OfficeManager.FindEntityByIdAsync(model.OfficeId);
                    if (office == null)
                    {
                        ModelState.AddModelError("", "部门不存在");
                        return View(model);
                    }
                    position.Name = model.Name;
                    position.Description = model.Description;
                    position.HeadUser = user.TeacherInfo;
                    position.Office = office;
                    await PositionManager.UpdateEntityAsync(position);

                    user.TeacherInfo.Position = position;
                    await UserManager.UpdateAsync(user);

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "目标不存在");
                }

            }
            return View(model);
        }

        #endregion
    }
}