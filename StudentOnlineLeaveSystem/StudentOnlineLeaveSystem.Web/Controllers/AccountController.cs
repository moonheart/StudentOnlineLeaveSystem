using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Host.SystemWeb;
using StudentOnlineLeaveSystem.BLL;
using StudentOnlineLeaveSystem.Common;
using StudentOnlineLeaveSystem.IBLL;
using StudentOnlineLeaveSystem.Models;
using StudentOnlineLeaveSystem.Web.Models;

namespace StudentOnlineLeaveSystem.Web.Controllers
{
    public class AccountController : Controller
    {
        private IUserService _userService;

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
        public AccountController()
        {
            _userService = new UserService();
        }
        // GET: User
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var user = _userService.Find(userId);
            return View(user);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginViewModel login, string returnUrl)
        {
            if (login.VerificationCode.ToUpper() != TempData["VerificationCode"].ToString())
            {
                ModelState.AddModelError("VerificationCode", "验证码错误");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var user = _userService.Find(login.UserName);
                    if (user == null)
                    {
                        ModelState.AddModelError("UserName", "用户名不存在");
                    }
                    else if (user.Password == Security.Sha256(login.Password))
                    {
                        _userService.Update(user);

                        var identity = _userService.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = login.RememberMe }, identity);

                        Session["User"] = user;

                        if (string.IsNullOrEmpty(returnUrl))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "密码错误");
                    }
                }
            }
            return View();
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegisterViewModel register)
        {
            if (TempData["VerificationCode"] == null ||
                TempData["VerificationCode"].ToString() != register.VerificationCode.ToUpper())
            {
                ModelState.AddModelError("VerificationCode", "验证码错误");
                return View(register);
            }

            if (ModelState.IsValid)
            {
                if (_userService.Exist(register.UserName))
                {
                    ModelState.AddModelError("Username", "用户名已存在");

                }
                else if (register.Password != register.Password2)
                {
                    ModelState.AddModelError("Password", "两次密码不一致");
                }
                else
                {
                    AppUser user = new AppUser
                    {
                        UserName = register.UserName,
                        //默认用户组代码写这里
                        Password = Security.Sha256(register.Password),
                        //邮箱验证与邮箱唯一性问题
                        Email = register.Email,
                        //用户状态问题
                        Status = 0,
                        DisplayName = register.UserName,
                    };
                    user = _userService.Add(user);
                    if (!string.IsNullOrEmpty(user.Id))
                    {
                        Session["User"] = user;
                        var identity = _userService.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignIn(identity);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "注册失败");
                    }
                }
            }
            return View(register);
        }

        [Authorize]
        public ActionResult Statistics()
        {
            var userId = User.Identity.GetUserId();
            var user = _userService.Find(userId);
            return View(user);
        }

        [Authorize]
        public ActionResult Preferences()
        {
            var userId = User.Identity.GetUserId();
            var user = _userService.Find(userId);
            ViewBag.User = user;
            PreferencesViewModel preferences = new PreferencesViewModel();
            return View(preferences);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Preferences(PreferencesViewModel preferences)
        {
            return View(preferences);
        }


        [Authorize]
        public ActionResult Settings()
        {
            var userId = User.Identity.GetUserId();
            var user = _userService.Find(userId);
            return View(user);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Settings(AppUser user)
        {
            return View(user);
        }
    }
}