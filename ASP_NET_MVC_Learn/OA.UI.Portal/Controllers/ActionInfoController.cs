using OA.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.UI.Portal.Controllers
{
    public class ActionInfoController : Controller
    {

        public IActionInfoService ActionInfoService { get; set; }

        // GET: ActionInfo
        public ActionResult Index()
        {
            ViewData.Model = ActionInfoService.GetEntities(u => true).AsEnumerable();
            return View();
        }
    }
}