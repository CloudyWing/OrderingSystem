using CloudyWing.OrderingSystem.Infrastructure.Util;
using CloudyWing.OrderingSystem.Web.Model;
using CloudyWing.OrderingSystem.Web.Model.LoginModel;
using Microsoft.AspNetCore.Mvc;

namespace CloudyWing.OrderingSystem.Web.Pages {
    [ValidateAntiForgeryToken]
    public class LogoutModel : PageModelBase {
        private readonly LoginAppService loginAppService;

        public LogoutModel(LoginAppService loginAppService) {
            ExceptionUtils.ThrowIfNull(() => loginAppService);

            this.loginAppService = loginAppService;
        }

        public async Task<IActionResult> OnGetAsync() {
            await loginAppService.LogoutAsync();
            return RedirectToPage("./Index");
        }
    }
}
