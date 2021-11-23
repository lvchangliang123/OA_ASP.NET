using OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.UI.Portal.Controllers
{
    public class BaseController:Controller
    {
        public UserInfo LoginUser { get; set; }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            //这里由于注册了全局过滤器，所以直接对登录用户进行赋值,理论上是需要校验用户登陆成功，再去赋值！
            LoginUser = filterContext.HttpContext.Session["loginUser"] as UserInfo;  //Session中存储的对象为object类型


        }

    }

}