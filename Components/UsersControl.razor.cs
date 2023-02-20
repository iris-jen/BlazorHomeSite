using BlazorHomeSite.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Components;

public partial class UsersControl
{
    [Inject] private IDbContextFactory<HomeSiteDbContext> _dbFactory { get; set; } = null!;
    [Inject] private UserManager<IdentityUser> _userManager { get; set; } = null!;
    [Inject] private SignInManager<IdentityUser> _signInManager { get; set; } = null!;
    [Inject] private IEmailSender _emailSender { get; set; } = null!;

    private List<IdentityUser> users = new List<IdentityUser>();

    protected override async Task OnInitializedAsync()
    {
        users = await _userManager.Users.ToListAsync();
    }

    public async Task DeleteUser(IdentityUser user)
    {
        await _userManager.DeleteAsync(user);
        users = await _userManager.Users.ToListAsync();
        StateHasChanged();
    }
}