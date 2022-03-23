using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Controllers
{
    //[AllowAnonymous]
    public class ErrorController : Controller
    {
        private ILogger<ErrorController> logger;
        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode) {
                case 404:
                    ViewBag.ErrorMessage = "抱歉，您访问的页面不存在";
                    //记录日志
                    logger.LogWarning($"发生了一个404错误，路径:{statusCodeResult.OriginalPath},以及查找字符串:{statusCodeResult.OriginalQueryString}");
                    break;
                default:
                    break;
            }
            return View("NotFound");
        }

        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewBag.ExceptionPath = exceptionHandlerPathFeature.Path;
            ViewBag.ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
            ViewBag.StackTrace = exceptionHandlerPathFeature.Error.StackTrace;
            //记录日志
            logger.LogError($"路径{exceptionHandlerPathFeature.Path}" + $"产生了一个错误{exceptionHandlerPathFeature.Error}");
            return View("Error");
        }
    }
}
