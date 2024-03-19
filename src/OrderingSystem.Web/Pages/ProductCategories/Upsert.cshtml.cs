using CloudyWing.OrderingSystem.Infrastructure.Util;
using CloudyWing.OrderingSystem.Web.Model;
using CloudyWing.OrderingSystem.Web.Model.ProductCategoryModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudyWing.OrderingSystem.Web.Pages.ProductCategories {
    [Authorize(Roles = "Administrator")]
    public class UpsertModel : PageModelBase {
        private readonly ProductCategoryAppService productCategoryAppService;

        public UpsertModel(ProductCategoryAppService productCategoryAppService) {
            ExceptionUtils.ThrowIfNull(() => productCategoryAppService);

            this.productCategoryAppService = productCategoryAppService;
        }

        [BindProperty]
        public UpsertViewModel? Data { get; set; }

        public async Task<ActionResult> OnGetAsync(Guid? id) {
            Data = id.HasValue
                ? await productCategoryAppService.GetItemAsync(id.Value)
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

            if (await productCategoryAppService.UpsertAsync(Data!)) {
                SetFormResult(FormResultLevel.Success, "儲存成功。");

                return RedirectToPage(nameof(Index));
            } else {
                SetFormResult(FormResultLevel.Danger, "儲存失敗。");

                return Page();
            }
        }
    }
}
