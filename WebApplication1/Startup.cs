using Microsoft.AspNetCore.Authentication.OpenIdConnect;
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
using Microsoft.Identity.Web;
using MrHuo.OAuth;
using MrHuo.OAuth.Github;
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

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();    //提供电子邮件确认令牌和密码重置和双因子身份登录等
            //添加Identity的依赖注入
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = true;    //验证电子邮箱

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);  //添加锁定账户时间和登录失败次数
            });
            //添加Identity的选项配置依赖



            services.AddAuthorization(options =>
            {
                //策略结合声明的授权
                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role"));
                options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("ADMIN"));
                //策略结合多个角色进行授权
                options.AddPolicy("SuperAdminPolicy", policy => policy.RequireRole("ADMIN", "User", "SuperManager"));
                options.AddPolicy("AllowedCountryPolicy", policy => policy.RequireClaim("Country", "China", "USA", "UK"));
                //利用委托书写多策略的注册声明
                options.AddPolicy("EditRolePolicy", policy => policy.RequireAssertion(context => AuthorizeAccess(context)));
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Admin/AccessDenied");
                options.Cookie.Name = "MockSchoolCookieName";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
            });

            //添加第三方登录验证依赖(失败......)

            //services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            //   .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));

            services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ClientId"];
                microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
            });

            //services.AddAuthentication().AddGitHub(options =>
            //{
            //    options.ClientId = Configuration["Authentication:Github:ClientId"];
            //    options.ClientSecret = Configuration["Authentication:Github:ClientSecret"];
            //});

            //services.AddSingleton(new GithubOAuth(OAuthConfig.LoadFrom(Configuration, "Authentication:Github")));
            services.AddSingleton<Extensions.DataProtectionPurposeStrings>();   //ID加密
        }

        private bool AuthorizeAccess(AuthorizationHandlerContext context)
        {
            return context.User.IsInRole("ADMIN") && context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") || context.User.IsInRole("Super Admin");
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

         

            app.UseRouting();
            //app.UseMvcWithDefaultRoute();   //添加默认路由到应用程序的请求处理管道中(默认到Home控制器，Index()处理方法下)

            //若要使用自定义路由规则，请使用app.UseMvc()方法，按照规则添加(***还是使用的MVC默认路由规则***)

            app.UseAuthentication();    //添加授权中间件 找出解析请求携带的信息
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
