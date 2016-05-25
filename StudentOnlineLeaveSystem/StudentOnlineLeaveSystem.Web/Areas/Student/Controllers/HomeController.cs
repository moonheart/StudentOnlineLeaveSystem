using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentOnlineLeaveSystem.BLL;
using StudentOnlineLeaveSystem.IBLL;

namespace StudentOnlineLeaveSystem.Web.Areas.Student.Controllers
{
    public class HomeController : Controller
    {
        private ILeaveService _leaveService;

        public HomeController()
        {
            _leaveService = new LeaveService();
        }

        // GET: Student/Home
        public ActionResult Index()
        {
            return View();
        }

    }
}