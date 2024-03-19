using System.Security.Cryptography;
using System.Text;
using CloudyWing.OrderingSystem.Infrastructure.Util;

namespace CloudyWing.OrderingSystem.Domain.Util {
    internal static class PasswordUtil {
        public static string ComputeHash(string? password) {
            ExceptionUtils.ThrowIfNullOrWhiteSpace(() => password);

            byte[] bytes = Encoding.UTF8.GetBytes(password!);
            byte[] hash = SHA512.HashData(bytes);

            return Convert.ToBase64String(hash);
        }
    }
}
