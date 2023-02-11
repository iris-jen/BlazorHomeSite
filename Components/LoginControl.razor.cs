using BlazorHomeSite.Data;
using BlazorHomeSite.Data.Accounts;
using BlazorHomeSite.Data.Music;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Components;

public partial class LoginControl
{
    [Inject] private IDbContextFactory<HomeSiteDbContext> DbFactory { get; set; } = null!;

    private LoginModel model = new LoginModel();
    private bool success;

    private void OnValidSubmit(EditContext context)
    {
        success = true;
        StateHasChanged();
    }

    private void Login(LoginModel model)
    {
    }
}