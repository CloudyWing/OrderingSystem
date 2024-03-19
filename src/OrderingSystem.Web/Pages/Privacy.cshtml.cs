using CloudyWing.OrderingSystem.Web.Model;

namespace CloudyWing.OrderingSystem.Web.Pages {
    public class PrivacyModel : PageModelBase {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger) {
            _logger = logger;
        }

        public void OnGet() {
        }
    }
}
