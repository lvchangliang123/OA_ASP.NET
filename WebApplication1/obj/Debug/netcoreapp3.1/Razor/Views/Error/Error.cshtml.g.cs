#pragma checksum "E:\ASP.NET\WebApplication1\Views\Error\Error.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "003f1132bdb42f27c1b2e9952cc9e7e763d22613"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Error_Error), @"mvc.1.0.view", @"/Views/Error/Error.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "E:\ASP.NET\WebApplication1\Views\_ViewImports.cshtml"
using WebApplication1;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\ASP.NET\WebApplication1\Views\_ViewImports.cshtml"
using WebApplication1.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\ASP.NET\WebApplication1\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"003f1132bdb42f27c1b2e9952cc9e7e763d22613", @"/Views/Error/Error.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"aa7cd25983d39fd720d8f3c7e467bca2029cc033", @"/Views/_ViewImports.cshtml")]
    public class Views_Error_Error : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "E:\ASP.NET\WebApplication1\Views\Error\Error.cshtml"
 if (ViewBag.ErrorTitle == null)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h3>程序请求时发生了一个内部错误，错误信息：</h3>\r\n    <div class=\"alert alert-danger\">\r\n        <h5>异常路径:</h5>\r\n        <hr />\r\n        <p>");
#nullable restore
#line 7 "E:\ASP.NET\WebApplication1\Views\Error\Error.cshtml"
      Write(ViewBag.ExceptionPath);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n    </div>\r\n");
            WriteLiteral("    <div class=\"alert alert-danger\">\r\n        <h5>异常信息:</h5>\r\n        <hr />\r\n        <p>");
#nullable restore
#line 13 "E:\ASP.NET\WebApplication1\Views\Error\Error.cshtml"
      Write(ViewBag.ExceptionMessage);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n    </div>\r\n");
            WriteLiteral("    <div class=\"alert alert-danger\">\r\n        <h5>异常堆栈跟踪:</h5>\r\n        <hr />\r\n        <p>");
#nullable restore
#line 19 "E:\ASP.NET\WebApplication1\Views\Error\Error.cshtml"
      Write(ViewBag.StackTrace);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n    </div>\r\n");
#nullable restore
#line 21 "E:\ASP.NET\WebApplication1\Views\Error\Error.cshtml"
}
else
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <!--当捕获到了删除异常时，直接显示指定异常信息-->\r\n    <h1 class=\"text-danger\">");
#nullable restore
#line 25 "E:\ASP.NET\WebApplication1\Views\Error\Error.cshtml"
                       Write(ViewBag.ErrorTitle);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n    <h6 class=\"text-danger\">");
#nullable restore
#line 26 "E:\ASP.NET\WebApplication1\Views\Error\Error.cshtml"
                       Write(ViewBag.ErrorMessage);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h6>\r\n");
#nullable restore
#line 27 "E:\ASP.NET\WebApplication1\Views\Error\Error.cshtml"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
