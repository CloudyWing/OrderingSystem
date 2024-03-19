using CloudyWing.OrderingSystem.Infrastructure.Util;
using CloudyWing.OrderingSystem.Web.Model;
using CloudyWing.OrderingSystem.Web.Model.ProductModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CloudyWing.OrderingSystem.Web.Pages.Products {
    [Authorize(Roles = "Administrator")]
    public class UpsertModel : PageModelBase {
        private readonly ProductAppService productAppService;
        private IEnumerable<SelectListItem>? categories;

        public UpsertModel(ProductAppService productAppService) {
            ExceptionUtils.ThrowIfNull(() => productAppService);

            this.productAppService = productAppService;
        }

        [BindProperty]
        public UpsertViewModel? Data { get; set; }

        public IEnumerable<SelectListItem> Categories {
            get {
                categories ??= productAppService.GetCategoriesAsync().GetAwaiter().GetResult();

                return categories;
            }
        }

        public async Task<ActionResult> OnGetAsync(Guid? id) {
            Data = id.HasValue
                ? await productAppService.GetItemAsync(id.Value)
                : new UpsertViewModel();

            if (Data is null) {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            if (await productAppService.UpsertAsync(Data!)) {
                SetFormResult(FormResultLevel.Success, "儲存成功。");

                return RedirectToPage(nameof(Index));
            } else {
                SetFormResult(FormResultLevel.Danger, "儲存失敗。");

                return Page();
            }
        }
    }
}
