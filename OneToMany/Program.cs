using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Options;
using OneToMany.Data;
using OneToMany.Services.Interface;
using OneToMany.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IProductService, ProductService>();


builder.Services.AddDbContext<AppDbContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISayService, SayService>();
builder.Services.AddScoped<ISliderInstaSerivce, SliderInstaService>();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();




var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

//admin panelin run olmasi ucun
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

//web saytin run olmasi ucun
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");




app.Run();

