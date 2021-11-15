using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Demo_First.Controllers
{
    public class UserInfoController : Controller
    {
        // GET: UserInfo
        public ActionResult Index()
        {
            return View();
        }

        //用户注册视图
        public ActionResult UserRegist()
        {
            return View();
        }

        public ActionResult ProcessUserRegist(FormCollection formCollection)
        {
            string str = formCollection["txtName"];
            return Content("ok" + str);
        }

    }
}