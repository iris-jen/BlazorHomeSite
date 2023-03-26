using BlazorHomeSite.Services;
using BlazorHomeSite.Services.Database;
using BlazorHomeSite.Services.Emails;
using BlazorHomeSite.Services.Photos;
using BlazorHomeSite.Services.Photos.PhotoAlbums;
using BlazorHomeSite.Services.SiteSettings;
using Howler.Blazor.Components;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IO;
using MudBlazor.Services;
using SixLabors.ImageSharp.Web.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

#region Services

// Db Stuff
builder.Services.AddDbContextFactory<DatabaseService>(options => options.UseSqlite("Data Source=HomeSite.db;Cache=Shared"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity Framework
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DatabaseService>();

// Blazor stuff
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

// UI Framework
builder.Services.AddMudServices();

// For music
builder.Services.AddScoped<IHowl, Howl>();
builder.Services.AddScoped<IHowlGlobal, HowlGlobal>();

// Images
builder.Services.AddImageSharp(
    options =>
    {
        // You only need to set the options you want to change here
        // All properties have been listed for demonstration purposes
        // only.
        options.Configuration = Configuration.Default;
        options.MemoryStreamManager = new RecyclableMemoryStreamManager();
        options.BrowserMaxAge = TimeSpan.FromDays(7);
        options.CacheMaxAge = TimeSpan.FromDays(365);
        options.CacheHashLength = 8;
        options.OnParseCommandsAsync = _ => Task.CompletedTask;
        options.OnBeforeSaveAsync = _ => Task.CompletedTask;
        options.OnProcessedAsync = _ => Task.CompletedTask;
        options.OnPrepareResponseAsync = _ => Task.CompletedTask;
    });

// Email
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
       o.TokenLifespan = TimeSpan.FromHours(3));

builder.Services.ConfigureApplicationCookie(o =>
{
    o.ExpireTimeSpan = TimeSpan.FromDays(5);
    o.SlidingExpiration = true;
});

// Secrets
builder.Services.Configure<AppAdminOptions>(builder.Configuration);

builder.Services.AddLogging();

builder.Services.AddTransient<ISiteSettingsService, SiteSettingsService>();
builder.Services.AddTransient<IUploadPhotoService, UploadPhotosService>();
builder.Services.AddTransient<IViewPhotoService, ViewPhotoService>();
builder.Services.AddTransient<IDatabaseService, DatabaseService>();
builder.Services.AddTransient<IPhotoAlbumService, PhotoAlbumService>();

#endregion Services

#region App

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseImageSharp();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

#endregion App