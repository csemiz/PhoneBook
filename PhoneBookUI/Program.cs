using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using PhoneBookDataLayer;
using PhoneBookEntityLayer.Mappings;

var builder = WebApplication.CreateBuilder(args);

//context bilgisi eklenir.

builder.Services.AddDbContext<MyContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Local"));
});

builder.Services.AddAutoMapper(x => 
{
    //x.AddExpressionMapping();
    x.AddProfile(typeof(Maps)); //Kimin kime donusecegini Maps class'i icinde tanimladik. Yaptigimiz tanimlamayi ayarlara ekledik.

});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles(); //wwwroot klasorunu gormesi icin

app.UseRouting(); //home7indexe gidebilmesi icin

app.UseAuthorization(); // Yetkilendirme icin

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // route' default pattern vermek icin

app.Run(); //uygulamayi calistirir
