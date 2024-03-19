using System.Security.Claims;
using CloudyWing.OrderingSystem.DataAccess.Entities;
using CloudyWing.OrderingSystem.Domain.Services.UserModel;
using CloudyWing.OrderingSystem.Infrastructure.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CloudyWing.OrderingSystem.Web.Model.LoginModel {
    public class LoginAppService : ApplicationService<LoginAppService> {
        private readonly UserService userService;

        public LoginAppService(IHttpContextAccessor httpContextAccessor, UserService userService, ILogger<LoginAppService> logger) : base(httpContextAccessor, logger) {
            ExceptionUtils.ThrowIfNull(() => userService);

            this.userService = userService;
        }

        public async Task<LoginResult> ValidateLoginAsync(LoginViewModel viewModel) {
            User? user = await userService.GetSingleOrDefaultAsync(viewModel.Email!);

            if (user == null) {
                return LoginResult.Fail;
            }

            if (!userService.VerifyPasseord(viewModel.Password, user.Password!)) {
                return LoginResult.Fail;
            }

            return LoginResult.Success;
        }

        public async Task LoginAsync(string email) {
            ExceptionUtils.ThrowIfNullOrWhiteSpace(() => email);

            User? user = await userService.GetSingleOrDefaultAsync(email);

            ExceptionUtils.ThrowIfItemNotFound(user);

            List<Claim> claims = new List<Claim>
             {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, user!.Name!),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties {
                    AllowRefresh = true,
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                }
            );
        }

        public async Task LogoutAsync() {
            await HttpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
