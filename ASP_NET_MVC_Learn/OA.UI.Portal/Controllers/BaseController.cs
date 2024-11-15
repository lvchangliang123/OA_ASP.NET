﻿using OA.Model;
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
            //LoginUser = filterContext.HttpContext.Session["loginUser"] as UserInfo;  //Session中存储的对象为object类型

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
            LoginUser = userInfo;   //缓存取出，并赋值

            //增加滑动窗口机制     重置缓存时间
            Common.Cache.CacheHelper.SetCache(userGuid, userInfo, DateTime.Now.AddMinutes(30));

        }

    }

}