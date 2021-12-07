using OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.UI.Portal.Models
{
    //利用全局过滤器来验证：用户是否登录
    public class LoginCheckFilterAttribute:ActionFilterAttribute
    {
        public bool IsCheck { get; set; }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            //校验用户是否登录
            if (IsCheck)
            {

                //改为缓存机制下的校验
                if (filterContext.HttpContext.Request.Cookies["userLoginId"] == null)
                {
                    filterContext.HttpContext.Response.Redirect("/UserLogin/Index");
                    return;
                }
                string userGuid = filterContext.HttpContext.Request.Cookies["userLoginId"].Value;
                UserInfo userInfo = Common.Cache.CacheHelper.GetCache(userGuid) as UserInfo;
                if (userInfo == null)
                {
                    //等待用户，缓存超时
                    filterContext.HttpContext.Response.Redirect("/UserLogin/Index");
                    return;
                }

                //if (filterContext.HttpContext.Session["loginUser"] == null)
                //{
                //    filterContext.HttpContext.Response.Redirect("/UserLogin/Index");
                //}
            }
  
        }
    }
}