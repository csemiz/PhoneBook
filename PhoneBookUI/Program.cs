using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PhoneBookBusinessLayer.EmailSenderBusiness;
using PhoneBookBusinessLayer.ImplementationsOfManagers;
using PhoneBookBusinessLayer.InterfacesOfManagers;
using PhoneBookDataLayer;
using PhoneBookDataLayer.ImplementationsOfRepo;
using PhoneBookDataLayer.InterfacesOfRepo;
using PhoneBookEntityLayer.Mappings;

var builder = WebApplication.CreateBuilder(args);

//context bilgisi eklenir.

builder.Services.AddDbContext<MyContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Local"));
});

//CookieAutehntication ayari eklendi.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

builder.Services.AddAutoMapper(x => 
{
    x.AddExpressionMapping();
    x.AddProfile(typeof(Maps)); //Kimin kime donusecegini Maps class'i icinde tanimladik. Yaptigimiz tanimlamayi ayarlara ekledik.

});

// Add services to the container.
builder.Services.AddControllersWithViews();

//Interface lerin islerini gerceklestirecek classlarý burada ''yasam dongulerini'' tanimlamaliyiz.
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IMemberManager, MemberManager>();

builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddScoped<IPhoneTypeRepository, PhoneTypeRepository>();
builder.Services.AddScoped<IPhoneTypeManager, PhoneTypeManager>();

builder.Services.AddScoped<IMemberPhoneRepository, MemberPhoneRepository>();
builder.Services.AddScoped<IMemberPhoneManager, MemberPhoneManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles(); //wwwroot klasorunu gormesi icin

app.UseRouting(); //home7indexe gidebilmesi icin
app.UseAuthentication();//Login ve Logout islemlerimiz icin
app.UseAuthorization(); // Yetkilendirme icin

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // route' default pattern vermek icin

app.Run(); //uygulamayi calistirir
