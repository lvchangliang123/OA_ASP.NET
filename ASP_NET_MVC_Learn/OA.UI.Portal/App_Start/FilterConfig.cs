using OA.UI.Portal.Models;
using System.Web;
using System.Web.Mvc;

namespace OA.UI.Portal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());

            //注册自定义的异常过滤器
            filters.Add(new MyExceptionFilterAttribute());
            filters.Add(new LoginCheckFilterAttribute() { IsCheck=true});

            //对于Asp.Net MVC  有三种过滤器：
            // 1.ActionFilter   2.ResultFilter   3.ErrorFilter HandleErrorAttribute 就是默认的异常处理类
        }
    }
}
