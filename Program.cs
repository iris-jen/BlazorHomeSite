using BlazorHomeSite.Data;
using Howler.Blazor.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

#region Services

// Db Stuff
builder.Services.AddDbContextFactory<HomeSiteDbContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity Framework
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<HomeSiteDbContext>();

// Blazor stuff
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// UI Framework
builder.Services.AddMudServices();

// For music
builder.Services.AddScoped<IHowl, Howl>();
builder.Services.AddScoped<IHowlGlobal, HowlGlobal>();

#endregion Services

#region App

var app = builder.Build();

if (!app.Environment.IsDevelopment()) app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

#endregion App