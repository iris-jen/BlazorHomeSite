using BlazorHomeSite.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Pages;

public partial class AllPhotosPage
{
    [Inject] private IDbContextFactory<ApplicationDbContext>? DbFactory { get; set; }
    
}