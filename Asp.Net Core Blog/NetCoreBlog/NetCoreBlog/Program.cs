using DataBaseFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DataBaseFramework.Infrastructure;
using Microsoft.Extensions.FileProviders;
using NetCoreBlog.Models;
using BlogModels.ModelHelpers;
using NetCoreBlog.Middlewares;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

//配置日志Serilog组件
string SerilogOutputTemplate = "{NewLine}{NewLine}Date：{Timestamp:yyyy-MM-dd HH:mm:ss.fff}{NewLine}LogLevel：{Level}{NewLine}Message：{Message}{NewLine}{Exception}" + new string('-', 50);
string LogFilePath(string LogEvent) => $@"{AppContext.BaseDirectory}00_logs\{LogEvent}\log.log";
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Debug).WriteTo.File(LogFilePath("Debug"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate))
    .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Information).WriteTo.File(LogFilePath("Information"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate))
    .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Warning).WriteTo.File(LogFilePath("Warning"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate))
    .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Error).WriteTo.File(LogFilePath("Error"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate))
    .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Fatal).WriteTo.File(LogFilePath("Fatal"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate))
    .CreateLogger();
builder.Services.AddSingleton<ILoggerFactory>(services =>
{
    var loggerFactory = new LoggerFactory();
    loggerFactory.AddSerilog();
    return loggerFactory;
});
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);


// Add services to the container.
builder.Services.AddControllersWithViews().AddNewtonsoftJson(option => option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddMvc();
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
builder.Services.AddSingleton<AppDbContext>();
builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("NetCoreBlogDBConnection"), new MySqlServerVersion(new Version(8, 0, 26)), mysqlOptions => mysqlOptions.MigrationsAssembly("NetCoreBlog"));
    options.EnableSensitiveDataLogging();
});
builder.Services.AddTransient(typeof(IRepository<,>), typeof(RepositoryBase<,>));
builder.Services.AddIdentity<CustomerIdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    builder.Logging.AddConfiguration(app.Configuration.GetSection("Logging"));
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();    //添加验证中间件

app.UseRouting();
app.UseMiddleware<VisitCounterMiddleware>();    //注册中间件，记录博客访问次数
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
