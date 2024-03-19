using CloudyWing.OrderingSystem.Infrastructure.Util;
using CloudyWing.OrderingSystem.Web.Model;
using CloudyWing.OrderingSystem.Web.Model.LoginModel;
using Microsoft.AspNetCore.Mvc;

namespace CloudyWing.OrderingSystem.Web.Pages {
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

            if (await loginAppService.ValidateLoginAsync(Data!) == LoginResult.Fail) {
                ModelState.AddModelError("", "±b¸¹©Î±K½X¿ù»~¡C");
                return Page();
            }

            await loginAppService.LoginAsync(Data!.Email!);

            return RedirectToPage("./Orders/Index");
        }
    }
}
