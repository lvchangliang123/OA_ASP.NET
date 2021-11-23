using OA.IBLL;
using OA.UI.Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.UI.Portal.Controllers
{
    [LoginCheckFilter(IsCheck =false)]
    //这里用了属性来拒绝Login页面执行过滤器中的用户是否存在的验证
    public class UserLoginController : Controller
    {
        // GET: UserLogin   这地方的注释告诉我们，默认的index()方法是GET请求下的响应

        public IUserInfoService UserInfoService { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        //生成验证码
        public ActionResult ShowVCode()
        {
            string oldcode = Session["ShowVCode"] as string;
            Common.ValidateCode validateCode = new Common.ValidateCode();
            string strCode = validateCode.CreateRandomCode(5);
            Session["VCode"] = strCode;
            return File(validateCode.CreateValidateGraphic(strCode), "image/Jpeg");
        }

        //处理登录的表单
        public ActionResult ProcessLogin()
        {
            //步骤：1. 处理验证码  2.验证用户信息  3.跳转到首页
            //1.
            string strCode = Request["vCode"];      //从前台页面取值  控件的name（vCode）
            string sessionCode = Session["VCode"] as string;        //从session域中取值
            Session["VCode"] = null;
            if (string.IsNullOrEmpty(sessionCode))
            {
                return Content("验证码错误");
            }
            if (strCode!=sessionCode)
            {
                return Content("验证码错误");
            }
            //2.
            string name = Request["loginName"];
            string pwd = Request["loginPwd"];

            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            var userinfo_Login = UserInfoService.GetEntities(u => u.UName == name && u.Pwd == pwd && u.DelFlag == delNormal).FirstOrDefault();
            if (userinfo_Login == null)
            {
                return Content("The UserName or Passward Incorrect,Please Login again!");
            }
            //存储登录过的用户信息,用户其他地方的校验
            Session["loginUser"] = userinfo_Login;

            //3.
            //如果验证正确，跳转到首页
            //return Content("Login Success");
            return Redirect("/Home/Index");

        }

    }
}