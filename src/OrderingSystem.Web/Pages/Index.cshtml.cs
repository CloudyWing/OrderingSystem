using CloudyWing.OrderingSystem.Web.Model;

namespace CloudyWing.OrderingSystem.Web.Pages;

public class IndexModel : PageModelBase {
    private readonly ILogger<IndexModel> logger;

    public IndexModel(ILogger<IndexModel> logger) {
        this.logger = logger;
    }

    public void OnGet() {

    }
}