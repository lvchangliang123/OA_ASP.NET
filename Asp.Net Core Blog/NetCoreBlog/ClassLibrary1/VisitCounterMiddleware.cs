using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MiddlewareLib
{
    /// <summary>
    /// 用于记录博客被访问过的次数的中间件
    /// </summary>
    public class VisitCounterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApplicationDbContext _dbContext;

        public VisitCounterMiddleware(RequestDelegate next, ApplicationDbContext dbContext)
        {
            _next = next;
            _dbContext = dbContext;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 获取博客 ID
            if (int.TryParse(context.Request.Query["BlogId"], out int blogId))
            {
                // 更新博客的访问次数
                var blog = await _dbContext.Blogs.FindAsync(blogId);
                if (blog != null)
                {
                    blog.VisitCount++;
                    await _dbContext.SaveChangesAsync();
                }
            }

            // 调用下一个中间件
            await _next(context);
        }
    }
}
