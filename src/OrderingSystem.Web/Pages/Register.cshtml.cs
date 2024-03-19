using CloudyWing.OrderingSystem.Infrastructure.Util;
using CloudyWing.OrderingSystem.Web.Model;
using CloudyWing.OrderingSystem.Web.Model.RegisterModel;
using Microsoft.AspNetCore.Mvc;

namespace CloudyWing.OrderingSystem.Web.Pages {
    public class RegisterModel : PageModelBase {
        private readonly RegisterAppService registerAppService;

        public RegisterModel(RegisterAppService registerAppService) {
            ExceptionUtils.ThrowIfNull(() => registerAppService);

            this.registerAppService = registerAppService;
        }

        [BindProperty]
        public RegisterViewModel? Data { get; set; }

        public void OnGet() {
            Data = new RegisterViewModel();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            RegisterResult result = await registerAppService.RegisterAsync(Data!);

            switch (result) {
                case RegisterResult.Success:
                    SetFormResult(FormResultLevel.Success, "註冊成功。");
                    return RedirectToPage(nameof(Index));
                case RegisterResult.Fail:
                    ModelState.AddModelError("", "註冊失敗。");
                    break;
                case RegisterResult.EmailExists:
                    ModelState.AddModelError($"{nameof(Data)}.{nameof(Data.Email)}", "電子信箱已註冊。");
                    break;
            }

            return Page();
        }
    }
}
