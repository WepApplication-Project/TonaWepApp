using TonaWebApp.Config;
using TonaWebApp.Repositories;
using TonaWebApp.Services;
using TonaWebApp.Interface;
using TonaWebApp.ViewComponents;
using TonaWebApp.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<MongoDBContext>(provider =>
{
    var context = new MongoDBContext("mongodb+srv://dev:tona1234@cluster0.mqtupab.mongodb.net/", "Tona");
    return context;
});

builder.Services.AddSingleton<AuthRepository>();

builder.Services.AddSingleton<BoardRepository>();

builder.Services.AddSingleton<NotificationRepository>();

builder.Services.AddHostedService<BoardCronJob>();

builder.Services.AddScoped<NavbarViewComponent>();

builder.Services.AddSingleton<CloudinaryController>();

builder.Services.AddScoped<IEmailService, EmailService>();
// Using Session
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>{
    options.IdleTimeout =  TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true ;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


// Session Call after routing
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.MapControllerRoute(
    name: "boardDetail",
    pattern: "{controller=Board}/{action=Detail}/{id?}");

app.MapControllerRoute(
    name: "user",
    pattern: "user",
    defaults: new { controller = "User", action = "Index"});

var dbContext = app.Services.GetRequiredService<MongoDBContext>();
dbContext.InitializeUserDataAsync().Wait();
dbContext.InitializeBoardDataAsync().Wait();
// dbContext.InitializeNotificationDataAsync().Wait();

app.Run();
