using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using StudentOnlineLeaveSystem.Models;

namespace StudentOnlineLeaveSystem.Web.Controllers
{
    public class ChecksController : Controller
    {
        // GET: Checks
        [Authorize]
        public ActionResult Index()
        {
            var user = Session["User"] as AppUser;
            if (user != null)
            {
                var list = user.Checks.OrderByDescending(c => c.CheckId);
                return View(list);
            }
            return RedirectToAction("Index");
        }

        // GET: Checks/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Checks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Checks/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Checks/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Checks/Edit/5
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

        // GET: Checks/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Checks/Delete/5
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
