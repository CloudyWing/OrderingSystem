using System.Security.Claims;
using CloudyWing.OrderingSystem.DataAccess.Entities;
using CloudyWing.OrderingSystem.Domain.Services.UserModel;
using CloudyWing.OrderingSystem.Infrastructure.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CloudyWing.OrderingSystem.Web.Model.LoginModel;

public class LoginAppService : ApplicationService<LoginAppService> {
    private readonly UserService userService;

    public LoginAppService(IHttpContextAccessor httpContextAccessor, UserService userService, ILogger<LoginAppService> logger) : base(httpContextAccessor, logger) {
        ExceptionUtils.ThrowIfNull(() => userService);

        this.userService = userService;
    }

    public async Task<LoginResult> ValidateLoginAsync(LoginViewModel viewModel) {
        ArgumentNullException.ThrowIfNull(viewModel);
        if (string.IsNullOrWhiteSpace(viewModel.Email) || string.IsNullOrWhiteSpace(viewModel.Password)) {
            return LoginResult.Fail;
        }

        User? user = await userService.GetSingleOrDefaultAsync(viewModel.Email);

        if (user == null || string.IsNullOrWhiteSpace(user.Password)) {
            return LoginResult.Fail;
        }

        if (!userService.VerifyPassword(viewModel.Password, user.Password)) {
            return LoginResult.Fail;
        }

        return LoginResult.Success;
    }

    public async Task LoginAsync(string email) {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        User? user = await userService.GetSingleOrDefaultAsync(email);

        ExceptionUtils.ThrowIfItemNotFound(user);
        HttpContext httpContext = HttpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext 不可為 Null。");
        string userName = string.IsNullOrWhiteSpace(user.Name) ? email : user.Name;

        List<Claim> claims = new List<Claim>
         {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };

        ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await httpContext.SignInAsync(
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
        HttpContext httpContext = HttpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext 不可為 Null。");
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}