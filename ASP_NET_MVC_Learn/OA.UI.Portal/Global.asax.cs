using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OA.UI.Portal
{
    public class MvcApplication : Spring.Web.Mvc.SpringMvcApplication //System.Web.HttpApplication    使用Spring.Net需要更改基类
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //应用程序一开始，就配置log，初始化
            log4net.Config.XmlConfigurator.Configure();

        }
    }
}
