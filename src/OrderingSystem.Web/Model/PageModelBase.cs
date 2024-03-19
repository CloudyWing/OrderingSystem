using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CloudyWing.OrderingSystem.Web.Model {
    public abstract class PageModelBase : PageModel {
        protected void SetFormResult(FormResultLevel level, string message) {
            TempData.Set(
                "FormResult",
                new FormResult {
                    Level = level,
                    Message = message
                }
            );
        }
    }
}
