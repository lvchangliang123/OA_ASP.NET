using OA.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.UI.Portal.Controllers
{
    public class OrderInfoController : Controller
    {
        // GET: OrderInfo
        public IOrderInfoService OrderInfoService { get; set; }
        public ActionResult Index()
        {
            ViewData.Model = OrderInfoService.GetEntities(u => true);
            return View();
        }
    }
}