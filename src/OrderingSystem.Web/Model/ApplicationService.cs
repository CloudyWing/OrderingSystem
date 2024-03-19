using CloudyWing.OrderingSystem.Infrastructure.DependencyInjection;

namespace CloudyWing.OrderingSystem.Web.Model {
    public abstract class ApplicationService<TAppService> : IApplicationService, IScopedDependency
        where TAppService : class {
        protected ApplicationService(IHttpContextAccessor httpContextAccessor, ILogger<TAppService> logger) {
            HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected IHttpContextAccessor HttpContextAccessor { get; private set; }

        protected ILogger<TAppService> Logger { get; private set; }
    }
}
