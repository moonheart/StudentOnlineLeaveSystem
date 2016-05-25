using LeaveSystem.Web.BLL;
using LeaveSystem.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LeaveSystem.Web.IBLL;
using LeaveSystem.Web.Models;
using LeaveSystem.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace LeaveSystem.Web.Controllers
{
    [Authorize(Roles = "Student")]
    public class LeaveController : Controller
    {
        #region 属性和构造函数
        private UserManager _userManager;
        private ILeaveConfigManager _leaveConfigManager;

        public ILeaveConfigManager LeaveConfigManager
        {
            get { return _leaveConfigManager ?? HttpContext.GetOwinContext().GetUserManager<LeaveConfigManager>(); }
            set { _leaveConfigManager = value; }
        }

        public UserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>(); }
            private set { _userManager = value; }
        }
        private LeaveManager _leaveManager;

        public LeaveManager LeaveManager
        {
            get { return _leaveManager ?? HttpContext.GetOwinContext().GetUserManager<LeaveManager>(); }
            private set { _leaveManager = value; }
        }

        private IClassManager _classManager;

        public IClassManager ClassManager
        {
            get { return _classManager ?? HttpContext.GetOwinContext().GetUserManager<ClassManager>(); }
            set { _classManager = value; }
        }


        #endregion

        // GET: Leave
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                return HttpNotFound();
            //var leaves = await LeaveManager.GetLeavesAsync(user);
            //var leaves = user.Leaves.ToList();
            var leaves = await LeaveManager.GetUnResumeLeavesOfUserAsync(user).ToListAsync();
            var list = new List<LeaveListViewModel>();
            foreach (var leave in leaves)
            {
                //list.Add(new LeaveListViewModel() { Leave = leave, Status = await LeaveManager.GetLeaveStatus(leave.Id) });
                list.Add(new LeaveListViewModel() { Leave = leave});
            }
            return View(list);
        }

        public async Task<ActionResult> History()
        {
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                return HttpNotFound();
            //var leaves = await LeaveManager.GetLeavesAsync(user);
            //var leaves = user.Leaves.ToList();
            var leaves = await LeaveManager.GetLeavesOfUserAsync(user).ToListAsync();
            var list = new List<LeaveListViewModel>();
            foreach (var leave in leaves)
            {
                list.Add(new LeaveListViewModel() { Leave = leave });
            }
            return View(list);
        }


        public ActionResult Apply()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Apply(LeaveApplyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userid = User.Identity.GetUserId();
                var user = await UserManager.FindByIdAsync(userid);
                if (user.StudentInfo.BelongClass == null ||
                    user.StudentInfo.Dormitory == null ||
                    user.StudentInfo.HomeAddress == null ||
                    user.StudentInfo.PersonalTel == null)
                {
                    ModelState.AddModelError("", "请完善个人信息");
                    return View(model);
                }

                if (!await LeaveManager.IsAllLeaveResumedAsync(userid))
                {
                    ModelState.AddModelError("", "还未销假");
                }
                else
                {
                    var config = await LeaveConfigManager.GetConfigAsync();

                    if (await LeaveManager.IsAllLeaveConditionMeeted(userid, config))
                    {
                        if (model.Category == LeaveCategory.病假)
                        {
                            if (model.TimeType == TimeType.一天以上)
                            {
                                if ((model.EndDate - model.StartDate).Days + 1 >= config.LeastSickLeaveDay)
                                {
                                    if (model.UploadFile == null)
                                    {
                                        ModelState.AddModelError("", "请上传图片附件");
                                        return View(model);
                                    }
                                    else
                                    {
                                        //"E:\\MyPrivateProject\\StudentManageSystem\\LeaveSystem\\LeaveSystem.Web\\"
                                        var p = Server.MapPath("/UploadImageAttach/");
                                        if (!Directory.Exists(p))
                                            Directory.CreateDirectory(p);
                                        var t = DateTime.Now.Ticks;
                                        var namea = model.UploadFile.FileName.Split('\\');
                                        var name = namea[namea.Length - 1];
                                        var filename = "/UploadImageAttach/" + t.ToString() + "_" +
                                                       name;

                                        model.UploadFile.SaveAs(p + t.ToString() + "_" + name);
                                        model.ImageUrls = filename;
                                    }
                                }
                            }
                        }

                        Leave leave = new Leave
                        {
                            Category = model.Category,
                            StartDate = model.StartDate,
                            EndDate = model.EndDate,
                            ImageUrls = model.ImageUrls,
                            OneDayTime = model.OneDayTime,
                            OneDayStart = model.OneDayStart,
                            OneDayEnd = model.OneDayEnd,
                            Reason = model.Reason,
                            ToWhere = model.ToWhere,
                            TimeType = model.TimeType,
                            Student = user.StudentInfo,
                            LeaveStatus = LeaveStatus.已提交,
                            AddTime = DateTime.Now,
                            ResumeTime = DateTime.Now,
                            IsResume = 0,
                        };
                        leave.Checks = new List<Check>();
                        var classHeadTeacher = user.StudentInfo.BelongClass.ClassTeacher;
                        leave.Checks.Add(new Check()
                        {
                            CheckTeacher = classHeadTeacher,
                            CheckOrder = 0,
                            CheckStatus = CheckStatus.未查看,
                            CheckTime = DateTime.Now,
                            AddTime = DateTime.Now
                        });
                        if (model.TimeType == TimeType.一天以上)
                        {
                            if ((model.EndDate - model.StartDate).Days > config.LeastDayToSign)
                            {
                                var clerk = config.ClerkTeacher;
                                leave.Checks.Add(new Check()
                                {
                                    CheckTeacher = clerk.Teacher.TeacherInfo,
                                    CheckOrder = 1,
                                    CheckStatus = CheckStatus.未查看,
                                    CheckTime = DateTime.Now,
                                    AddTime = DateTime.Now
                                });
                            }
                        }
                        await LeaveManager.AddEntityAsync(leave);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "请假添加不符合");
                    }
                }
            }
            return View(model);
        }

        public async Task<ActionResult> AddLeaveImages()
        {
            if (Session["TempLeave"] == null)
                return View("Error", new[] { "请按照流程" });
            return View();
        }

        //public async Task<ActionResult> UploadImage()
        //{

        //}


        [HttpPost]
        public async Task<ActionResult> AddLeaveImages(AddLeaveImagesViewModel omodel)
        {
            if (ModelState.IsValid)
            {
                if (Session["TempLeave"] == null)
                    return View("Error", new[] { "请按照流程" });
                var model = Session["TempLeave"] as LeaveApplyViewModel;
                if (model == null)
                    return View("Error", new[] { "请按照流程" });
                model.ImageUrls = omodel.ImagesUrl;
                var userid = User.Identity.GetUserId();
                var user = await UserManager.FindByIdAsync(userid);
                var config = await LeaveConfigManager.GetConfigAsync();

                Leave leave = new Leave
                {
                    Category = model.Category,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    ImageUrls = model.ImageUrls,
                    OneDayTime = model.OneDayTime,
                    OneDayStart = model.OneDayStart,
                    OneDayEnd = model.OneDayEnd,
                    Reason = model.Reason,
                    ToWhere = model.ToWhere,
                    TimeType = model.TimeType,
                    //UserId = userid,
                    LeaveStatus = LeaveStatus.已提交,
                    AddTime = DateTime.Now,
                    ResumeTime = DateTime.Now,
                    IsResume = 0,
                };
                leave.Checks = new Check[] { };
                var classHeadTeacher = user.StudentInfo.BelongClass.ClassTeacher;
                leave.Checks.Add(new Check()
                {
                    CheckTeacher = classHeadTeacher,
                    CheckOrder = 0,
                    CheckStatus = CheckStatus.未查看,
                    CheckTime = DateTime.Now,
                    CheckOpinion = ""
                });
                if (model.TimeType == TimeType.一天以上)
                {
                    if ((model.EndDate - model.StartDate).Days > config.LeastDayToSign)
                    {
                        var clerk = config.ClerkTeacher;
                        leave.Checks.Add(new Check()
                        {
                            CheckTeacher = clerk.Teacher.TeacherInfo,
                            CheckOrder = 1,
                            CheckStatus = CheckStatus.未查看,
                            CheckTime = DateTime.Now,
                            CheckOpinion = ""
                        });
                    }
                }
                await LeaveManager.AddEntityAsync(leave);
                return RedirectToAction("Index");

            }
            return View(omodel);
        }

        public async Task<ActionResult> Detail(int? id)
        {
            var leave = await LeaveManager.FindEntityByIdAsync(id ?? 0);

            if (leave == null)
                return View("Error", new[] { "找不到该信息" });
            var model = new LeaveDetailViewModel
            {
                Leave = leave,
                Checks = leave.Checks
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Resume(int? id)
        {
            if (ModelState.IsValid)
            {
                var userid = User.Identity.GetUserId();
                var user = await UserManager.FindByIdAsync(userid);
                if (user == null)
                {
                    return View("Error", new[] { "找不到用户" });
                }
                var leave = await LeaveManager.FindEntityByIdAsync(id ?? 0);
                if (leave == null)
                {
                    return View("Error", new[] { "找不到该项" });
                }
                LeaveManager.ResumeApply(leave.Id);
                //var teacher = leave.Checks.First(e => e.CheckOrder == 0).CheckTeacher;
                //teacher.TeacherInfo.ResumeApplies
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Cancel(int? id)
        {
            if (ModelState.IsValid)
            {
                var userid = User.Identity.GetUserId();
                var user = await UserManager.FindByIdAsync(userid);
                if (user == null)
                {
                    return View("Error", new[] { "找不到用户" });
                }
                var leave = await LeaveManager.FindEntityByIdAsync(id ?? 0);
                if (leave == null)
                {
                    return View("Error", new[] { "找不到该项" });
                }
                leave.LeaveStatus = LeaveStatus.已取消;
                await LeaveManager.UpdateEntityAsync(leave);
                //var teacher = leave.Checks.First(e => e.CheckOrder == 0).CheckTeacher;
                //teacher.TeacherInfo.ResumeApplies
            }
            return RedirectToAction("Index");
        }

    }
}