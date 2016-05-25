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
    public class ClassController : Controller
    {
        #region fields
        private IClassManager _classManager;

        public IClassManager ClassManager
        {
            get { return _classManager ?? HttpContext.GetOwinContext().GetUserManager<ClassManager>(); }
            set { _classManager = value; }
        }
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
        private IGradeManager _gradeManager;

        public IGradeManager GradeManager
        {
            get { return _gradeManager ?? HttpContext.GetOwinContext().GetUserManager<GradeManager>(); }
            set { _gradeManager = value; }
        }
        private UserManager _userManager;

        public UserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>(); }
            private set { _userManager = value; }
        }

        #endregion

        #region Class

        public ActionResult Index(int? p, string searchString, string currentFilter)
        {

            var list = ClassManager.GetAllEntities()
                .Include(e => e.ClassTeacher);

            if (searchString != null)
            {
                p = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            var list2 =
                UserManager.Users.Where(e => e.StudentInfo != null)
                    .Include(e => e.StudentInfo)
                    .Where(e => e.StudentInfo.BelongClass != null)
                    .GroupBy(e => e.StudentInfo.BelongClass.Id)
                    .ToDictionary(e => e.Key, e => e.Count());
            ViewBag.Group = list2;
            list = list.Where(e => string.IsNullOrEmpty(searchString) || e.ClassDefination.Contains(searchString));
            list = list.OrderBy(e => e.ClassDefination);
            var pagesize = 5;
            var page = p ?? 1;
            return View(list.ToPagedList(page, pagesize));
        }


        public async Task<ActionResult> Create()
        {
            var departments = await DepartmentManager.GetAllEntities().ToListAsync();
            SelectList departSelectList = new SelectList(departments, "Id", "Name");
            ViewBag.departSelectList = departSelectList;

            var majors = await MajorManager.GetMajorsForDepartment(departments.ElementAtOrDefault(0)).ToListAsync();
            SelectList majorSelectList = new SelectList(majors, "Id", "Name");
            ViewBag.majorSelectList = majorSelectList;

            var grades = await GradeManager.GetAllEntities().ToListAsync();
            SelectList gradeSelectList = new SelectList(grades, "Id", "GradeNum");
            ViewBag.gradeSelectList = gradeSelectList;

            var teachers = (await UserManager.GetNoClassTeachersAsync()).ToList();
            ViewBag.teacherSelectList = new SelectList(teachers, "Id", "Name");


            ViewBag.Classes = await ClassManager.GetAllEntities().ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ClassCreateViewModel model)
        {

            if (ModelState.IsValid)
            {
                var departments = await DepartmentManager.GetAllEntities().ToListAsync();
                SelectList departSelectList = new SelectList(departments, "Id", "Name");
                ViewBag.departSelectList = departSelectList;

                var majors = await MajorManager.GetMajorsForDepartment(departments.ElementAtOrDefault(0)).ToListAsync();
                SelectList majorSelectList = new SelectList(majors, "Id", "Name");
                ViewBag.majorSelectList = majorSelectList;

                var grades = await GradeManager.GetAllEntities().ToListAsync();
                SelectList gradeSelectList = new SelectList(grades, "Id", "GradeNum");
                ViewBag.gradeSelectList = gradeSelectList;

                var teachers = (await UserManager.GetNoClassTeachersAsync()).ToList();
                ViewBag.teacherSelectList = new SelectList(teachers, "Id", "Name");

                ViewBag.Classes = await ClassManager.GetAllEntities().ToListAsync();

                var grade = await GradeManager.FindEntityByIdAsync(model.GradeId);
                if (grade == null)
                {
                    ModelState.AddModelError("", "找不到grade");
                    return View(model);
                }
                var major = await MajorManager.FindEntityByIdAsync(model.MajorId);
                if (major == null)
                {
                    ModelState.AddModelError("", "找不到major");
                    return View(model);
                }
                var teacher = await UserManager.FindByIdAsync(model.ClassTeacherId);
                if (teacher == null)
                {
                    ModelState.AddModelError("", "找不到teacher");
                    return View(model);
                }
                var class1 = new Class
                {
                    ClassDefination = model.Defination,
                    Grade = grade,
                    Major = major,
                    ClassTeacher = teacher.TeacherInfo
                };
                if (await ClassManager.IsEntityExistByLamdaAsync(c =>
                    c.Grade.Id == class1.Grade.Id && c.Major.Id == class1.Major.Id &&
                    c.ClassDefination == class1.ClassDefination))
                {
                    ModelState.AddModelError("", "已存在Class");
                    return View(model);
                }
                var class2 = await ClassManager.AddEntityAsync(class1);

                teacher.TeacherInfo.ManageClass = class2;
                await UserManager.UpdateAsync(teacher);


                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> GetMajorOfDepartment(int? departId)
        {
            var depart = await DepartmentManager.FindEntityByIdAsync(departId ?? 0);
            if (depart == null)
            {
                return Json(new { error = 1, message = "找不到该Department" });
            }
            return Json(new { error = 0, message = "获取成功", data = depart.Majors.Select(m => new { m.Id, m.Name }) });
        }

        public async Task<ActionResult> Edit(int? id)
        {
            var class1 = await ClassManager.FindEntityByIdAsync(id ?? 0);
            if (class1 == null)
                return HttpNotFound("class不存在");
            var departments = await DepartmentManager.GetAllEntities().ToListAsync();
            SelectList departSelectList = new SelectList(departments.ToList(), "Id", "Name");
            ViewBag.departSelectList = departSelectList;

            var majors = await MajorManager.GetMajorsForDepartment(departments.ElementAtOrDefault(0)).ToListAsync();
            SelectList majorSelectList = new SelectList(majors.ToList(), "Id", "Name");
            ViewBag.majorSelectList = majorSelectList;

            var grades = await GradeManager.GetAllEntities().ToListAsync();
            SelectList gradeSelectList = new SelectList(grades.ToList(), "Id", "GradeNum");
            ViewBag.gradeSelectList = gradeSelectList;

            var teachers = (await UserManager.GetNoClassTeachersAsync()).ToList();
            if (class1.ClassTeacher != null)
                teachers.Add(class1.ClassTeacher.Teacher);
            ViewBag.teacherSelectList = new SelectList(teachers, "Id", "Name");

            ViewBag.Classes = await ClassManager.GetAllEntities().ToListAsync();

            var model = new ClassEditViewModel
            {
                Defination = class1.ClassDefination,
                DepartmentId = class1.Major.Department.Id,
                GradeId = class1.Grade.Id,
                MajorId = class1.Major.Id,
                Students = await UserManager.GetStudentsOfClassAsync(class1).ToListAsync(),
                ClassTeacherId = class1.ClassTeacher == null ? null : class1.ClassTeacher.Teacher.Id
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ClassEditViewModel model)
        {
            var departments = await DepartmentManager.GetAllEntities().ToListAsync();
            SelectList departSelectList = new SelectList(departments.ToList(), "Id", "Name");
            ViewBag.departSelectList = departSelectList;

            var majors = await MajorManager.GetMajorsForDepartment(departments.ElementAtOrDefault(0)).ToListAsync();
            SelectList majorSelectList = new SelectList(majors.ToList(), "Id", "Name");
            ViewBag.majorSelectList = majorSelectList;

            var grades = await GradeManager.GetAllEntities().ToListAsync();
            SelectList gradeSelectList = new SelectList(grades.ToList(), "Id", "GradeNum");
            ViewBag.gradeSelectList = gradeSelectList;

            var teachers = (await UserManager.GetNoClassTeachersAsync()).ToList();
            ViewBag.teacherSelectList = new SelectList(teachers, "Id", "Name");

            ViewBag.Classes = await ClassManager.GetAllEntities().ToListAsync();

            if (ModelState.IsValid)
            {
                var class1 = await ClassManager.FindEntityByIdAsync(model.Id);
                if (class1 == null)
                {
                    ModelState.AddModelError("", "找不到class1");
                    return View(model);
                }
                var grade = await GradeManager.FindEntityByIdAsync(model.GradeId);
                if (grade == null)
                {
                    ModelState.AddModelError("", "找不到grade");
                    return View(model);
                }
                var major = await MajorManager.FindEntityByIdAsync(model.MajorId);
                if (major == null)
                {
                    ModelState.AddModelError("", "找不到major");
                    return View(model);
                }
                var teacher = await UserManager.FindByIdAsync(model.ClassTeacherId);
                if (teacher == null)
                {
                    ModelState.AddModelError("", "找不到teacher");
                    return View(model);
                }
                class1.Major = major;
                class1.Grade = grade;
                class1.ClassDefination = model.Defination;
                class1.ClassTeacher = teacher.TeacherInfo;

                await ClassManager.UpdateEntityAsync(class1);

                teacher.TeacherInfo.ManageClass = class1;
                await UserManager.UpdateAsync(teacher);

                await UserManager.RemoveStudentsFromClassAsync(model.IdsToRemove ?? new string[] { });

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
                var model = await ClassManager.FindEntityByIdAsync(id ?? 0);
                if (model != null)
                {
                    await UserManager.RemoveAllStudentsFromClassAsync(model);
                    await ClassManager.DeleteEntityAsync(model);
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> AddStudent(int? id, int? p)
        {
            var class1 = await ClassManager.FindEntityByIdAsync(id ?? 0);
            if (class1 == null)
            {
                ModelState.AddModelError("id", "The Class you are looking for is not Exist");
                return View("Error", new[] { "The Class you are looking for is not Exist" });
                //return HttpNotFound("The Class you are looking for is not Exist");
            }
            var students = (await UserManager.GetNoClassStudentsAsync()).ToPagedList(p ?? 1, 10);
            ViewBag.OutStudents = students;
            ViewBag.Class = class1;
            var list = await UserManager.GetStudentsOfClassAsync(class1).Select(e => e.Name).ToArrayAsync();

            return View(new AddStudentsViewModel { StudentsName = list });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddStudent(AddStudentsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var class1 = await ClassManager.FindEntityByIdAsync(model.ClassId);
                if (class1 == null)
                {
                    return View("Error", new[] { "班级找不到" });
                }
                await UserManager.AddStudentsToClassAsync(class1, model.IdsToAdd ?? new string[] { });
                model.IdsToAdd = new string[] { };
                return RedirectToAction("AddStudent");

            }
            return View(model);

        }

        #endregion
    }
}