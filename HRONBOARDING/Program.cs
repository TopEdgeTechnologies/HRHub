
using HRHUBAPI.Models.Configuration;
using HRHUBWEB.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var BaseUrl = "https://localhost:7152/";
//var BaseUrl = "http://192.168.90.20:8056/";

builder.Services.AddHttpClient("APIClient", configureClient =>
{
    configureClient.BaseAddress = new Uri(BaseUrl);

});


builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<DbConnection>();
builder.Services.AddSingleton<APIHelper>();
builder.Services.AddSingleton<IEmailHelper, EmailHelper>();
//builder.Services.AddSingleton<CacheExtensions>();
//builder.Services.AddMemoryCache();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseExceptionHandler("/Error");



//app.UseExceptionHandler("/Error");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Company}/{action=OnBoard}/{id?}");

app.Run();
