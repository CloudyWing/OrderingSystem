using CloudyWing.OrderingSystem.Infrastructure.Util;
using CloudyWing.OrderingSystem.Web.Model;
using CloudyWing.OrderingSystem.Web.Model.ProductModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Model;

namespace CloudyWing.OrderingSystem.Web.Pages.Products {
    [Authorize(Roles = "Administrator")]
    public partial class IndexModel : PageModelBase {
        private readonly ProductAppService productAppService;

        public IndexModel(ProductAppService productAppService) {
            ExceptionUtils.ThrowIfNull(() => productAppService);

            this.productAppService = productAppService;
        }

        public void OnGet() {
        }

        public async Task<IActionResult> OnPostAsync() {
            return new JsonResult(await productAppService.GetListAsync());
        }

        public async Task<IActionResult> OnPostMoveUpAsync([FromBody] Command command) {
            return new JsonResult(await productAppService.MoveUpAsync(command.Id));
        }

        public async Task<IActionResult> OnPostMoveDownAsync([FromBody] Command command) {
            return new JsonResult(await productAppService.MoveDownAsync(command.Id));
        }

        public async Task<IActionResult> OnPostDeleteAsync([FromBody] Command command) {
            return new JsonResult(await productAppService.DeleteAsync(command.Id));
        }
    }
}
