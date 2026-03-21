using CloudyWing.OrderingSystem.Infrastructure.Util;
using CloudyWing.OrderingSystem.Web.Model;
using CloudyWing.OrderingSystem.Web.Model.LoginModel;
using Microsoft.AspNetCore.Mvc;

namespace CloudyWing.OrderingSystem.Web.Pages;

[ValidateAntiForgeryToken]
public class LoginModel : PageModelBase {
    private readonly LoginAppService loginAppService;

    public LoginModel(LoginAppService loginAppService) {
        ExceptionUtils.ThrowIfNull(() => loginAppService);

        this.loginAppService = loginAppService;
    }

    [BindProperty]
    public LoginViewModel? Data { get; set; }

    public void OnGet() {
        Data = new LoginViewModel();
    }

    public async Task<IActionResult> OnPostAsync() {
        if (!ModelState.IsValid) {
            return Page();
        }

        LoginViewModel? loginData = Data;
        if (loginData is null || string.IsNullOrWhiteSpace(loginData.Email)) {
            return Page();
        }

        if (await loginAppService.ValidateLoginAsync(loginData) == LoginResult.Fail) {
            ModelState.AddModelError("", "帳號或密碼錯誤。");
            return Page();
        }

        await loginAppService.LoginAsync(loginData.Email);

        return RedirectToPage("./Orders/Index");
    }
}