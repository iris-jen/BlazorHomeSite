using BlazorHomeSite.Data;
using BlazorHomeSite.Data.Music;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Components;

public partial class RegisterControl
{
    [Inject] private IDbContextFactory<HomeSiteDbContext> DbFactory { get; set; } = null!;
}