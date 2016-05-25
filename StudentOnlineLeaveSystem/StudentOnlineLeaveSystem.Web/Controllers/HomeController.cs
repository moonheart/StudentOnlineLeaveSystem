using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using StudentOnlineLeaveSystem.Common;

namespace StudentOnlineLeaveSystem.Web.Controllers
{
    public class HomeController : Controller
    {
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
            string verificationCode = Security.CreateVerificationText(4);
            Bitmap img = Security.CreateVerificationImage(verificationCode, 100, 34);
            img.Save(Response.OutputStream, ImageFormat.Jpeg);
            TempData["VerificationCode"] = verificationCode.ToUpper();
            return null;
        }
    }
}