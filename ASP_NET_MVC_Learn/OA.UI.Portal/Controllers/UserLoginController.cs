using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.UI.Portal.Controllers
{
    public class UserLoginController : Controller
    {
        // GET: UserLogin   这地方的注释告诉我们，默认的index()方法是GET请求下的响应
        public ActionResult Index()
        {
            return View();
        }

        

        public ActionResult ShowVCode()
        {
            string oldcode = Session["ShowVCode"] as string;
            Common.ValidateCode validateCode = new Common.ValidateCode();
            string code = validateCode.CreateRandomCode(5);
            Session["ShowVCode"] = code;
            if (Session["ShowVCode"].ToString().ToLower() != code.ToLower())
            {
                Response.Write("<script>alert('验证码错误')</script>");
            }
            else
            {

            }
            return File(validateCode.CreateValidateGraphic(code), "image/Jpeg");
        }
    }
}