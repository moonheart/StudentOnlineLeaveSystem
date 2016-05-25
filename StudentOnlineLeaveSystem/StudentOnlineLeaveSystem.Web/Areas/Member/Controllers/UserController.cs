using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Host.SystemWeb;
using StudentOnlineLeaveSystem.BLL;
using StudentOnlineLeaveSystem.Common;
using StudentOnlineLeaveSystem.IBLL;
using StudentOnlineLeaveSystem.Models;
using StudentOnlineLeaveSystem.Web.Areas.Member.Models;

namespace StudentOnlineLeaveSystem.Web.Areas.Member.Controllers
{
    public class UserController : Controller
    {
        private IUserService _userService;
        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        public UserController()
        {
            _userService = new UserService();
        }

        // GET: Member/User
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult VerificationCode()
        {
            string verificationCode = Security.CreateVerificationText(6);
            Bitmap img = Security.CreateVerificationImage(verificationCode, 160, 30);
            img.Save(Response.OutputStream, ImageFormat.Jpeg);
            TempData["VerificationCode"] = verificationCode.ToUpper();
            return null;
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
        public ActionResult Register(RegisterViewModel register)
        {
            if (TempData["VerificationCode"] == null
                || register.VerificationCode.ToUpper() != TempData["VerificationCode"].ToString())
            {
                ModelState.AddModelError("VerificationCode", "验证码不正确");
                return View(register);
            }

            if (ModelState.IsValid)
            {
                if (_userService.Exist(register.UserName))
                {
                    ModelState.AddModelError("Username", "用户名已存在");
                }
                else
                {
                    ApplicationUser user = new ApplicationUser()
                    {
                        UserName = register.UserName,
                        //默认用户组代码写这里
                        DisplayName = register.DisplayName,
                        Password = Security.Sha256(register.Password),
                        //邮箱验证与邮箱唯一性问题
                        Email = register.Email,
                        //用户状态问题
                        Status = 0,
                    };
                    user = _userService.Add(user);
                    if (!string.IsNullOrEmpty(user.Id))
                    {
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

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult Login(string returnUrl)
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.Find(loginViewModel.UserName);
                if (user == null)
                    ModelState.AddModelError("UserName", "用户名不存在");
                else if (user.Password == Common.Security.Sha256(loginViewModel.Password))
                {
                    _userService.Update(user);
                    var identity = _userService.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = loginViewModel.RememberMe }, identity);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Password", "密码错误");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return Redirect(Url.Content("~"));
        }
    }
}