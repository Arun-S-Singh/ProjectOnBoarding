using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProjectOnBoarding.Data;
using ProjectOnBoarding.Services.Email;
using Serilog;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

//Add support to logging with SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Logging
//builder.Host.UseSerilog();

//Log.Logger = new LoggerConfiguration()
//    .ReadFrom
//    .Configuration(configuration.)
//    .CreateLogger();

var configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build();

// DB context 
var connectionString = builder.Configuration.GetConnectionString("ProjectDB") ??
    throw new InvalidOperationException("Connection string not found");

// Add services to the container.

builder.Services.AddDbContext<ProjectDBContext>(options =>
    options.UseSqlite(connectionString));
//options.UseSqlServer(connectionString));

//builder.Services.AddDbContext<ProjectDBContext>(options => options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ProjectDBUser>(options =>
    {
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
    })
    .AddEntityFrameworkStores<ProjectDBContext>();

builder.Services.AddControllersWithViews();

//Email
builder.Services.AddSingleton<IEmailService, EmailService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
