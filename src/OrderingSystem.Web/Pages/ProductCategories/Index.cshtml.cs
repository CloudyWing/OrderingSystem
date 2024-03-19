using CloudyWing.OrderingSystem.Infrastructure.Util;
using CloudyWing.OrderingSystem.Web.Model;
using CloudyWing.OrderingSystem.Web.Model.ProductCategoryModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Model;

namespace CloudyWing.OrderingSystem.Web.Pages.ProductCategories {
    [Authorize(Roles = "Administrator")]
    public partial class IndexModel : PageModelBase {
        private readonly ProductCategoryAppService productCategoryAppService;

        public IndexModel(ProductCategoryAppService productCategoryAppService) {
            ExceptionUtils.ThrowIfNull(() => productCategoryAppService);

            this.productCategoryAppService = productCategoryAppService;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync() {
            return new JsonResult(await productCategoryAppService.GetListAsync());
        }

        public async Task<IActionResult> OnPostMoveUpAsync([FromBody] Command command) {
            return new JsonResult(await productCategoryAppService.MoveUpAsync(command.Id));
        }

        public async Task<IActionResult> OnPostMoveDownAsync([FromBody] Command command) {
            return new JsonResult(await productCategoryAppService.MoveDownAsync(command.Id));
        }

        public async Task<IActionResult> OnPostDeleteAsync([FromBody] Command command) {
            return new JsonResult(await productCategoryAppService.DeleteAsync(command.Id));
        }
    }
}
