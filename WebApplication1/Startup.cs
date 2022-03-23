using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DataRepositories;
using WebApplication1.Infrastructure;
using WebApplication1.Models;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration,IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //添加数据库连接注入
            services.AddDbContextPool<AppDbContext>(optionsBuilder => optionsBuilder.UseMySql(Configuration.GetConnectionString("MockStudentDBConnection"),new MySqlServerVersion(new Version(8,0,26))));

            //config =>
            //{
            //    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            //    config.Filters.Add(new AuthorizeFilter(policy));
            //}

            services.AddControllersWithViews().AddXmlSerializerFormatters();
            //AddXmlSerializerFormatters()方法，在MVC服务中允许返回XML格式的数据类型(JSON格式默认自动允许？？？)
            //services.AddSingleton<IStudentRepository, MockStudentRepository>();    
            services.AddScoped<IStudentRepository, SQLStudentRepository>();     //添加数据库实体类依赖注入
            //services.AddSingleton<IWebHostEnvironment, WebHostBuilder>();
            //services.AddMvc(a => a.EnableEndpointRouting = false);

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            //添加Identity的依赖注入
            services.Configure<IdentityOptions>(options => {
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

            });
            //添加Identity的选项配置依赖

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
            else if(env.IsStaging()||env.IsProduction()||env.IsEnvironment("UAT"))
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
            app.UseStaticFiles();

         

            app.UseRouting();
            //app.UseMvcWithDefaultRoute();   //添加默认路由到应用程序的请求处理管道中(默认到Home控制器，Index()处理方法下)

            //若要使用自定义路由规则，请使用app.UseMvc()方法，按照规则添加(***还是使用的MVC默认路由规则***)

            app.UseAuthentication();    //添加鉴权中间件 找出解析请求携带的信息
            app.UseAuthorization();     //添加验证中间件,在添加路由中间件之前添加，目的是到达路由之前进行验证


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                      name: "default",
                      pattern: "{controller=Student}"
                    );
            });

            /***如果使用<UseEndpoints>来注册路由，UseRouting()注册中间件，一定要放在该方法之前，这里还是中间件的加载顺序问题!!!***/

            //app.UseMvc(routes => routes.MapRoute(
            //    name: "default",
            //    template: "{controller=Student}"
            //    ));

            /*****中间件是按照顺序来加载的******/

            //app.UseMvc();   //不传参，将使用<属性路由>来设定路由


            //app.UseEndpoints(endpoints =>
            //{
            //    //endpoints.MapControllerRoute(
            //    //    name: "default",
            //    //    pattern: "{controller=Home}/{action=Index}/{id?}");
            //    endpoints.MapControllerRoute(
            //       name: "default",
            //       pattern: "{controller=Student}/{action=Details}/{id?}");
            //});
        }
    }
}
