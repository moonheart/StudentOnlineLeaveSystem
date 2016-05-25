using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentOnlineLeaveSystem.BLL;
using StudentOnlineLeaveSystem.IBLL;
using StudentOnlineLeaveSystem.Models;

namespace StudentOnlineLeaveSystem.Web.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private IUserService _userService;

        public UserController()
        {
            _userService = new UserService();
        }

        // GET: Admin/User
        [Authorize]
        public ActionResult Index()
        {
            int totalRecord;
            var list = _userService.FindPageList(1, 10, out totalRecord, 0);
            return View(list);
        }

        // GET: Admin/User/Details/5
        public ActionResult Details(string id)
        {
            var user = _userService.Find(id);
            return View(user);
        }

        // GET: Admin/User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/User/Create
        [HttpPost]
        public ActionResult Create(AppUser user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _userService.Add(user);
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View(user);
            }
            return View(user);
        }

        // GET: Admin/User/Edit/5
        public ActionResult Edit(string id)
        {
            var user = _userService.Find(id);
            return View(user);
        }

        // POST: Admin/User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
