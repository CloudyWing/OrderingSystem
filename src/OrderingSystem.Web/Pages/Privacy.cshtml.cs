using CloudyWing.OrderingSystem.Web.Model;

namespace CloudyWing.OrderingSystem.Web.Pages;

public class PrivacyModel : PageModelBase {
    private readonly ILogger<PrivacyModel> logger;

    public PrivacyModel(ILogger<PrivacyModel> logger) {
        this.logger = logger;
    }

    public void OnGet() {
    }
}