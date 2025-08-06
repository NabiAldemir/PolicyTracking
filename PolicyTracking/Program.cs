using System.Reflection;
using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.Concerete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using DataAccessLayer.Interceptors;
using DataAccessLayer.SignalR;
using EntityLayer.Concrete;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});

builder.Services.AddIdentity<AppUser, AppRole>(x =>
{
    x.Password.RequireUppercase = false;
    x.Password.RequireLowercase = false;
    x.Password.RequireNonAlphanumeric = false;
    x.User.RequireUniqueEmail = true;
    x.Password.RequireDigit = false;
})
    .AddEntityFrameworkStores<Context>();

builder.Services.AddMvc();

builder.Services.AddAuthentication()
    .AddCookie("Customer", x =>
    {
        x.Cookie.Name = "Company";
        x.LoginPath = "/Account/Login";
        x.AccessDeniedPath = "/Login/AccessDenied";
        x.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        x.SlidingExpiration = true;
    })
    .AddCookie("Admin", x =>
    {
        x.Cookie.Name = "Admin";
        x.LoginPath = "/Account/Login";
        x.AccessDeniedPath = "/Login/AccessDenied";
        x.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        x.SlidingExpiration = true;
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<LogInterceptor>();
builder.Services.AddDbContext<Context>((serviceProvider, options) =>
{
 
    options.UseSqlServer("server=DESKTOP-N6O4A6E; database=PolicyTrackingDb; integrated security=true;TrustServerCertificate=True;").AddInterceptors(serviceProvider.GetRequiredService<LogInterceptor>());
});

builder.Services.AddMvc(config =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});


builder.Services.AddSession();

builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, NameUserIdProvider>();



builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<IUserDal, EfUserRepository>();

builder.Services.AddScoped<IActionsService, ActionsManager>();
builder.Services.AddScoped<IActionsDal, EfActionsRepository>();

builder.Services.AddScoped<ICustomerService, CustomerManager>();
builder.Services.AddScoped<ICustomerDal, EfCustomerRepository>();

builder.Services.AddScoped<IAgencyService, AgencyManager>();
builder.Services.AddScoped<IAgencyDal, EfAgencyRepository>();

builder.Services.AddScoped<IPolicyService, PolicyManager>();
builder.Services.AddScoped<IPolicyDal, EfPolicyRepository>();

builder.Services.AddScoped<IPolicyTypeService, PolicyTypeManager>();
builder.Services.AddScoped<IPolicyTypeDal, EfPolicyTypeRepository>();

builder.Services.AddScoped<IVehicleService, VehicleManager>();
builder.Services.AddScoped<IVehicleDal, EfVehicleRepository>();

builder.Services.AddScoped<IHousingService, HousingManager>();
builder.Services.AddScoped<IHousingDal, EfHousingRepository>();

builder.Services.AddScoped<ILogService, LogManager>();
builder.Services.AddScoped<ILogDal, EfLogRepository>();

builder.Services.AddScoped<IAddressService, AddressManager>();
builder.Services.AddScoped<IAddressDal, EfAddressRepository>();

builder.Services.AddScoped<ICompanyInformationService, CompanyInformationManager>();
builder.Services.AddScoped<ICompanyInformationDal, EfCompanyInformationRepository>();

builder.Services.AddHttpClient<ILocationService, LocationManager>();
builder.Services.AddScoped<ILocationService, LocationManager>();
builder.Services.AddAutoMapper(typeof(EntityLayer.Mapper.MapperProfile));
builder.Services.AddAutoMapper(typeof(PolicyTrackingWebUI.Mapper.MapperProfile));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.MapHub<UserStatusHub>("/userStatusHub");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=CustomerDashboard}/{action=Index}/{id?}");

app.Run();
