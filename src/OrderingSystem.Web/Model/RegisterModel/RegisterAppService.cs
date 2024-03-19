using CloudyWing.OrderingSystem.DataAccess.Entities;
using CloudyWing.OrderingSystem.Domain.Services.UserModel;
using CloudyWing.OrderingSystem.Infrastructure.Util;

namespace CloudyWing.OrderingSystem.Web.Model.RegisterModel {
    public class RegisterAppService : ApplicationService<RegisterAppService> {
        private readonly UserService userService;

        public RegisterAppService(IHttpContextAccessor httpContextAccessor, ILogger<RegisterAppService> logger, UserService userService)
            : base(httpContextAccessor, logger) {
            ExceptionUtils.ThrowIfNull(() => userService);

            this.userService = userService;
        }

        public async Task<RegisterResult> RegisterAsync(RegisterViewModel viewModel) {
            if (await userService.IsExistsAsync(viewModel.Email)) {
                return RegisterResult.EmailExists;
            }

            UserEditor editor = new UserEditor {
                Email = viewModel.Email,
                Name = viewModel.Name,
                Password = viewModel.Password,
                Role = Role.Member
            };

            return await userService.CreateAsync(editor)
                ? RegisterResult.Success
                : RegisterResult.Fail;
        }
    }
}
