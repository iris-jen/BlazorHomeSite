using BlazorHomeSite.Data;
using BlazorHomeSite.Data.Accounts;
using BlazorHomeSite.Data.Music;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text;
using System;

namespace BlazorHomeSite.Components;

public partial class RegisterControl
{
    [Inject] private IDbContextFactory<HomeSiteDbContext> _dbFactory { get; set; } = null!;
    [Inject] private UserManager<IdentityUser> _userManager { get; set; } = null!;

    private RegisterModel model = new RegisterModel();
    private bool success;

    private void OnValidSubmit(EditContext context)
    {
        success = true;
        StateHasChanged();
    }

    public async Task RegisterNewUser(RegisterModel model)
    {
        var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
        var result = await _userManager.CreateAsync(user, Input.Password);
        if (result.Succeeded)
        {
            _logger.LogInformation("User created a new account with password.");

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
            "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                //return RedirectToPage("RegisterConfirmation",
                //  new { email = Input.Email });
            }
            else
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                /// return LocalRedirect(returnUrl);
            }
        }
        foreach (var error in result.Errors)
        {
            //ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    // If we got this far, something failed, redisplay form
}
}