using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaveSystem.Web.BLL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace LeaveSystem.Web.Controllers
{
    [Authorize]
    public class LessonController : Controller
    {
        private LessonManager _lessonManager;

        public LessonManager LessonManager
        {
            get { return _lessonManager ?? HttpContext.GetOwinContext().GetUserManager<LessonManager>(); }
            set { _lessonManager = value; }
        }

        // GET: Lesson
        public ActionResult Index()
        {
            if (User.IsInRole("Student"))
                return RedirectToAction("StudentLesson");
            if (User.IsInRole("Teacher"))
                return RedirectToAction("TeacherLesson");
            return RedirectToAction("Index", "Home");
        }

        public ActionResult StudentLesson()
        {
            var userId = User.Identity.GetUserId();
            var lessons = LessonManager.GetStudentLessons(userId).ToList();
            ViewBag.Lessons = lessons;
            return View();
        }

        public ActionResult LessonAsign()
        {
            return View();
        }

        public ActionResult AdminLesson()
        {

            return View();
        }

        public ActionResult TeacherLesson()
        {
            throw new NotImplementedException();
        }
    }
}