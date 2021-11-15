using OA.BLL;
using OA.IBLL;
using OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.UI.Portal.Controllers
{
    public class UserInfoController : Controller
    {
        // GET: UserInfo

        //public UserInfoService userInfoService = new UserInfoService();
        //业务逻辑类由Spring创建，作为属性
        public IUserInfoService UserInfoService { get; set; }
        public ActionResult Index()
        {
            ViewData.Model = UserInfoService.GetEntities(u => true);

            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection formCollection)
        {
            string name = formCollection["txtName"];
            string id= formCollection["txtID"]; //数据库中ID是自增的，手动设置也没用

            if (ModelState.IsValid)
            {
                UserInfoService.Add(new UserInfo { UName=name,Id=Int32.Parse(id)});
            }
            return RedirectToAction("Index");
        }

    }
}