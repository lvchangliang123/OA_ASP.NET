using DataBaseFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DataBaseFramework.Infrastructure;
using Microsoft.Extensions.FileProviders;
using NetCoreBlog.Models;
using DataBaseFramework.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
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
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();    //�����֤�м��

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
