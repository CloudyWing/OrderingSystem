using CloudyWing.OrderingSystem.Infrastructure.Util;
using CloudyWing.OrderingSystem.Web.Model;
using CloudyWing.OrderingSystem.Web.Model.OrderModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Model;

namespace CloudyWing.OrderingSystem.Web.Pages.Orders {
    [Authorize(Roles = "Administrator,Member")]
    public partial class IndexModel : PageModelBase {
        private readonly OrderAppService orderAppService;

        public IndexModel(OrderAppService orderAppService) {
            ExceptionUtils.ThrowIfNull(() => orderAppService);

            this.orderAppService = orderAppService;
        }

        public void OnGet() {
        }

        public async Task<IActionResult> OnPostAsync() {
            return new JsonResult(await orderAppService.GetListAsync());
        }

        public async Task<IActionResult> OnPostGetDetailsAsync([FromBody] Command command) {
            return new JsonResult(await orderAppService.GetDetailsAsync(command.Id));
        }

        public async Task<IActionResult> OnPostDeleteAsync([FromBody] Command command) {
            return new JsonResult(await orderAppService.DeleteAsync(command.Id));
        }
    }
}
