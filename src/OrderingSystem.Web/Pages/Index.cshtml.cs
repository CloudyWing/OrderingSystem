using CloudyWing.OrderingSystem.Web.Model;

namespace CloudyWing.OrderingSystem.Web.Pages {
    public class IndexModel : PageModelBase {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger) {
            _logger = logger;
        }

        public void OnGet() {

        }
    }
}
