using CloudyWing.OrderingSystem.Infrastructure.Util;
using CloudyWing.OrderingSystem.Web.Model;
using CloudyWing.OrderingSystem.Web.Model.OrderModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrderingSystem.Model;

namespace CloudyWing.OrderingSystem.Web.Pages.Orders {
    [Authorize(Roles = "Administrator,Member")]
    public class UpsertModel : PageModelBase {
        private readonly OrderAppService orderAppService;

        public UpsertModel(OrderAppService orderAppService) {
            ExceptionUtils.ThrowIfNull(() => orderAppService);

            this.orderAppService = orderAppService;
        }

        [BindProperty]
        public UpsertViewModel? Data { get; set; }

        public async void OnGet(Guid? id) {
            Data = new UpsertViewModel {
                Id = id,
                Date = id.HasValue
                    ? await orderAppService.GetOrderDateAsync(id.Value)
                    : DateTime.Today
            };
        }

        public async Task<ActionResult> OnPostUpsertAsync([FromBody] UpsertViewModel viewModel) {
            ResponseResult result = new();
            ModelState.Clear();

            if (!TryValidateModel(viewModel)) {
                result.IsOk = false;
                result.Message = ModelState.GetFirstErrorMessage();
            } else {
                result.IsOk = await orderAppService.UpsertAsync(viewModel);
                result.Message = result.IsOk ? "點餐成功。" : "點餐失敗。";
            }

            return new JsonResult(result);
        }

        public async Task<IActionResult> OnPostGetDetailsAsync([FromBody] Command command) {
            return new JsonResult(await orderAppService.GetDetailsByUpsertAsync(command.Id));
        }

        public async Task<IActionResult> OnPostGetProductCategoriesAsync() {
            return new JsonResult(await orderAppService.GetProductCategoriesAsync());
        }

        public async Task<IActionResult> OnPostGetProductsAsync() {
            return new JsonResult(await orderAppService.GetProductsAsync());
        }
    }
}
