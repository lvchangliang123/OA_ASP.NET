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
            //������ݿ�����ע��
            services.AddDbContextPool<AppDbContext>(optionsBuilder => optionsBuilder.UseMySql(Configuration.GetConnectionString("MockStudentDBConnection"),new MySqlServerVersion(new Version(8,0,26))));

            //config =>
            //{
            //    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            //    config.Filters.Add(new AuthorizeFilter(policy));
            //}

            services.AddControllersWithViews().AddXmlSerializerFormatters();
            //AddXmlSerializerFormatters()��������MVC������������XML��ʽ����������(JSON��ʽĬ���Զ���������)
            //services.AddSingleton<IStudentRepository, MockStudentRepository>();    
            services.AddScoped<IStudentRepository, SQLStudentRepository>();     //������ݿ�ʵ��������ע��
            //services.AddSingleton<IWebHostEnvironment, WebHostBuilder>();
            //services.AddMvc(a => a.EnableEndpointRouting = false);

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();    //�ṩ�����ʼ�ȷ�����ƺ��������ú�˫������ݵ�¼��
            //���Identity������ע��
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = true;    //��֤��������

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);  //��������˻�ʱ��͵�¼ʧ�ܴ���
            });
            //���Identity��ѡ����������



            services.AddAuthorization(options =>
            {
                //���Խ����������Ȩ
                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role"));
                options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("ADMIN"));
                //���Խ�϶����ɫ������Ȩ
                options.AddPolicy("SuperAdminPolicy", policy => policy.RequireRole("ADMIN", "User", "SuperManager"));
                options.AddPolicy("AllowedCountryPolicy", policy => policy.RequireClaim("Country", "China", "USA", "UK"));
                //����ί����д����Ե�ע������
                options.AddPolicy("EditRolePolicy", policy => policy.RequireAssertion(context => AuthorizeAccess(context)));
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Admin/AccessDenied");
                options.Cookie.Name = "MockSchoolCookieName";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
            });

            //��ӵ�������¼��֤����(ʧ��......)

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
            services.AddSingleton<Extensions.DataProtectionPurposeStrings>();   //ID����
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
            //app.UseMvcWithDefaultRoute();   //���Ĭ��·�ɵ�Ӧ�ó����������ܵ���(Ĭ�ϵ�Home��������Index()��������)

            //��Ҫʹ���Զ���·�ɹ�����ʹ��app.UseMvc()���������չ������(***����ʹ�õ�MVCĬ��·�ɹ���***)

            app.UseAuthentication();    //�����Ȩ�м�� �ҳ���������Я������Ϣ
            app.UseAuthorization();     //�����֤�м��,�����·���м��֮ǰ��ӣ�Ŀ���ǵ���·��֮ǰ������֤


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                      name: "default",
                      pattern: "{controller=Student}"
                    );
            });

            /***���ʹ��<UseEndpoints>��ע��·�ɣ�UseRouting()ע���м����һ��Ҫ���ڸ÷���֮ǰ�����ﻹ���м���ļ���˳������!!!***/

            //app.UseMvc(routes => routes.MapRoute(
            //    name: "default",
            //    template: "{controller=Student}"
            //    ));

            /*****�м���ǰ���˳�������ص�******/

            //app.UseMvc();   //�����Σ���ʹ��<����·��>���趨·��


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
