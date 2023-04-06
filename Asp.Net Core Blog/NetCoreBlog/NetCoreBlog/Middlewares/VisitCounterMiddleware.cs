using BlogModels.Dtos;
using DataBaseFramework.Infrastructure;

namespace NetCoreBlog.Middlewares
{
    /// <summary>
    /// 用来记录访问次数的中间件
    /// </summary>
    public class VisitCounterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRepository<BlogInfoDto, int> _blogRepository;

        public VisitCounterMiddleware(RequestDelegate next, IRepository<BlogInfoDto, int> blogRepository)
        {
            _next = next;
            _blogRepository = blogRepository;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //获取博客ID
            if (int.TryParse(context.Request.Query["blogId"], out int blogId))
            {
                // 更新博客的访问次数
                var blog = await _blogRepository.SingleAsync(b=>b.Id == blogId);
                if (blog != null)
                {
                    blog.ViewCount++;
                    await _blogRepository.UpdateAsync(blog);
                }
            }
            //调用下一个中间件
            await _next(context);
        }
    }
}
