using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using StudentOnlineLeaveSystem.BLL;
using StudentOnlineLeaveSystem.IBLL;
using StudentOnlineLeaveSystem.Models;
using StudentOnlineLeaveSystem.Web.Models;

namespace StudentOnlineLeaveSystem.Web.Controllers
{
    public class LeaveController : Controller
    {
        private ILeaveService _leaveService;

        public LeaveController()
        {
            _leaveService = new LeaveService();
        }
        // GET: Student/Leave
        [Authorize]
        public ActionResult Index()
        {
            int totalRecord;
            var list = _leaveService.FindPageList(1, 10, out totalRecord, 0, User.Identity.GetUserId());

            List<LeaveListViewModel> list2 = new List<LeaveListViewModel>();
            foreach (var leaf in list.ToList())
            {
                list2.Add(new LeaveListViewModel
                {
                    Leave = leaf,
                    Status = _leaveService.GetLeaveStatus(leaf.LeaveId, leaf.UserId)
                });
            }
            return View(list2);
        }

        [Authorize]
        public ActionResult Notice()
        {
            return View();
        }

        /// <summary>
        /// 申请请假
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Apply()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Apply(LeaveApplyViewModel leavemoModel)
        {
            if (ModelState.IsValid)
            {
                //判断是否需要图片附件
                switch (leavemoModel.Category)
                {
                    //病假
                    case LeaveCategory.病假:
                        break;
                    //事假
                    case LeaveCategory.事假:
                        break;
                }

                var userId = User.Identity.GetUserId();

                Leave leave = new Leave
                {
                    Category = leavemoModel.Category,
                    StartDate = leavemoModel.StartDate,
                    EndDate = leavemoModel.EndDate,
                    ImageUrls = leavemoModel.ImageUrls,
                    OneDayTime = leavemoModel.OneDayTime,
                    OneDayStart = leavemoModel.OneDayStart,
                    OneDayEnd = leavemoModel.OneDayEnd,
                    Reason = leavemoModel.Reason,
                    ToWhere = leavemoModel.ToWhere,
                    TimeType = leavemoModel.TimeType,

                    UserId = userId,

                    LeaveStatus = 1,
                    AddTime = DateTime.Now,
                    ResumeTime = DateTime.Now,
                    IsResume = 0,

                    Checks = new[]
                    {
                        new Check { CheckStatus = 0, CheckTime = DateTime.Now, UserId = int.Parse(userId),CheckOrder = 0}, 
                        new Check { CheckStatus = 0, CheckTime = DateTime.Now, UserId = int.Parse(userId) ,CheckOrder = 1}
                    }
                };
                try
                {
                    leave = _leaveService.Add(leave);
                    if (leave.LeaveId > 0)
                    {
                        return RedirectToAction("Success", leave);
                    }
                    else
                    {
                        ModelState.AddModelError("", "请假失败");
                    }

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "请假失败," + ex.Message);
                }

            }
            return View(leavemoModel);
        }


        [Authorize]
        public ActionResult Success(Leave leave)
        {
            return View(leave);
        }

        [Authorize]
        public ActionResult Detail(int? id)
        {
            if (id == null || id < 1)
            {
                return RedirectToAction("Index");
            }
            var leave = _leaveService.Find((int)id, User.Identity.GetUserId());

            var model = new LeaveDetailViewModel
            {
                Leave = leave,
                Checks = leave.Checks
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public int GetLeaveTimes(string userid)
        {
            return _leaveService.GetLeaveTimes(userid);
        }
    }
}