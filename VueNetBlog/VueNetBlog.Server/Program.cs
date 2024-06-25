using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using VueNetBlog.Server.DataBaseFramework;
using VueNetBlog.Server.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
builder.Services.AddSingleton<BlogDbContext>();
builder.Services.AddDbContextPool<BlogDbContext>(options =>
{
    options.UseMySQL("Data Source=localhost;Initial Catalog=VueNetBlogDB_Test;User=root;Password=123456");
    options.EnableSensitiveDataLogging();
});
builder.Services.AddTransient(typeof(IRepository<,>), typeof(RepositoryBase<,>));
builder.Services.AddIdentity<User, IdentityRole<int>>().AddEntityFrameworkStores<BlogDbContext>().AddDefaultTokenProviders();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder => builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
