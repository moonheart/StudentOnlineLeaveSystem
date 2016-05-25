using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LeaveSystem.Web.BLL;
using LeaveSystem.Web.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using LeaveSystem.Web.Models;
using LeaveSystem.Web.ViewModels;


namespace LeaveSystem.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        #region Managers
        private ApplicationSignInManager _signInManager;
        private UserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(UserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public UserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        #endregion


        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            return RedirectToAction("Profiles");
        }

        public async Task<ActionResult> Profiles()
        {
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                return View("Error", new[] { "找不到该用户" });
            ViewBag.CurrentUser = user;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Profiles(User model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    ModelState.AddModelError("", "用户不存在");
                }
                else
                {
                    user.PhoneNumber = model.PhoneNumber;
                    if (user.StudentInfo != null)
                    {
                        user.StudentInfo.HomeAddress = model.StudentInfo.HomeAddress;
                        user.StudentInfo.PersonalTel = model.StudentInfo.PersonalTel;
                        user.StudentInfo.Dormitory = model.StudentInfo.Dormitory;
                    }
                    if (user.TeacherInfo != null)
                    {
                        user.TeacherInfo.OfficeNum = model.TeacherInfo.OfficeNum;
                    }
                    var res = await UserManager.UpdateAsync(user);
                    if (res.Succeeded)
                    {
                        ViewBag.Message = "修改成功";
                    }
                    else
                    {
                        ViewBag.Message = "修改失败";
                    }
                }
            }
            return View(model);
        }



        public async Task<ActionResult> HeadSet()
        {
            var userid = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userid);
            ViewBag.CurrentUser = user;
            return View(new HeadSetViewModel() { UserId = user.Id, UserHeadAddress = user.HeadImage });
        }

        [HttpPost]
        public async Task<ActionResult> HeadSet(HeadSetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    ViewBag.Message = "找不到用户";
                    return View(model);
                }
                if (model.HeadFile == null)
                {
                    ViewBag.Message = "请选择文件上传";
                    return View(model);
                }
                var p = Server.MapPath("/UploadHeadSet/");
                if (!Directory.Exists(p))
                    Directory.CreateDirectory(p);
                var t = DateTime.Now.Ticks;
                var namea = model.HeadFile.FileName.Split('\\');
                var name = namea[namea.Length - 1];
                var filename = "/UploadHeadSet/" + t.ToString() + "_" + name;

                model.HeadFile.SaveAs(p + t.ToString() + "_" + name);

                user.HeadImage = filename;
                await UserManager.UpdateAsync(user);
                ViewBag.Message = "保存成功";
                return RedirectToAction("HeadSet");
            }
            return View(model);
        }


        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region 帮助程序
        // 用于在添加外部登录名时提供 XSRF 保护
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}