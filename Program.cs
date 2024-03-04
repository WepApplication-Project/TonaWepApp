using TonaWebApp.Config;
using TonaWebApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<MongoDBContext>(provider =>
{
    var context = new MongoDBContext("mongodb+srv://dev:tona1234@cluster0.mqtupab.mongodb.net/", "Tona");
    return context;
});

builder.Services.AddSingleton<AuthRepository>();

builder.Services.AddSingleton<BoardRepository>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "user",
    pattern: "user",
    defaults: new { controller = "User", action = "Index" });

var dbContext = app.Services.GetRequiredService<MongoDBContext>();
dbContext.InitializeUserDataAsync().Wait();

app.Run();
