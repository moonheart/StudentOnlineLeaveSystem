using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LeaveSystem.Web.BLL;
using LeaveSystem.Web.Common;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using LeaveSystem.Web.Models;
using LeaveSystem.Web.ViewModels;

namespace LeaveSystem.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        #region 属性与构造方法
        private UserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(UserManager userManager)
        {
            UserManager = userManager;
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


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        #endregion

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (TempData["VerificationCode"] == null || model.VerificationCode != TempData["VerificationCode"].ToString())
            {
                ModelState.AddModelError("VerificationCode", "验证码错误");
                return View(model);
            }

            var user = await UserManager.FindAsync(model.UserName, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "用户名或密码错误");
                return View(model);
            }

            var claimsIdentity =
                await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignOut();
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = model.RememberMe }, claimsIdentity);
            await UserManager.WriteLoginLogAsync(user, new LoginLog(Request.UserHostAddress, DateTime.Now));
            return RedirectToLocal(returnUrl);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (TempData["VerificationCode"] == null ||
                    model.VerificationCode != TempData["VerificationCode"].ToString())
                {
                    ModelState.AddModelError("VerificationCode", "验证码错误");
                    return View(model);
                }

                var user = new User { UserName = model.UserName, Name = model.Name };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    user = await UserManager.FindAsync(model.UserName, model.Password);
                    var claimsIdentity =
                        await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = false }, claimsIdentity);

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }


        //验证码
        [AllowAnonymous]
        public ActionResult VerificationCode()
        {
            string verifCode;
            var img = Security.GetVerifImage(4, 100, 34, out verifCode);
            TempData["VerificationCode"] = verifCode.ToUpper();
            return File(img, "image/jpeg");
        }



        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
            }

            base.Dispose(disposing);
        }



        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

    }
}