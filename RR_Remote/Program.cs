using Microsoft.EntityFrameworkCore;
using RR_Remote.Areas.Admin.IUtilities;
using RR_Remote.Areas.Admin.Utilities;
using RR_Remote.Context;
using RR_Remote.Repositry;
using RR_Remote.Services.Contract;
using RR_Remote.Services.ContractApi;
using RR_Remote.Services.Implementation;
using RR_Remote.Services.ImplementationApi;
using RR_Remote.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IDbRepository, DbRepository>();
builder.Services.AddScoped<IHome, HomeImplementation>();
builder.Services.AddScoped<IImageUpload, ImageUpload>();
builder.Services.AddScoped<IBanner, BannerImplementation>();

builder.Services.AddScoped<IAccount, AccountImplementation>();
builder.Services.AddScoped<IProduct, ProductImplementation>();
builder.Services.AddScoped<ICommon, CommonImplementation>();
builder.Services.AddScoped<IOrder, OrderImplementation>();
builder.Services.AddSingleton<EmailOperation>();
var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(connectionString));
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

});
builder.Services.AddAuthentication("Identity.Application").AddCookie("Identity.Application", options =>
{
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.Cookie.Name = ".AspNet.SharedCookie";
    options.Cookie.HttpOnly = true;
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

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Admin}/{controller=Home}/{action=Login}/{id?}");

app.Run();
