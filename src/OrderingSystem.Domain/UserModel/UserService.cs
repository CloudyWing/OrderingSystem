using CloudyWing.OrderingSystem.DataAccess.Entities;
using CloudyWing.OrderingSystem.Domain.Util;
using CloudyWing.OrderingSystem.Infrastructure.Util;
using Microsoft.Extensions.Logging;

namespace CloudyWing.OrderingSystem.Domain.Services.UserModel {
    public class UserService(ApplicationDbContext dbContext, ILogger<UserService> logger)
                : QueryableService<User, UserService>(dbContext, logger) {
        public async Task<bool> CreateAsync(UserEditor editor) {
            ExceptionUtils.ThrowIfNull(() => editor);

            User entity = Mapper.Map<User>(editor);
            DbSet.Add(entity);

            return await SaveChangesAsync() == 1;
        }

        public async Task<bool> IsExistsAsync(string? email) {
            ExceptionUtils.ThrowIfNull(() => email);

            return await IsExistsAsync(x => x.Email!.ToLower() == email!.ToLower());
        }

        public async Task<User?> GetSingleOrDefaultAsync(string? email) {
            ExceptionUtils.ThrowIfNull(() => email);

            return await GetSingleOrDefaultAsync(x => x.Email!.ToLower() == email!.ToLower());
        }

        public bool VerifyPasseord(string? password, string? hashedPassword) {
            ExceptionUtils.ThrowIfNullOrWhiteSpace(() => password);
            ExceptionUtils.ThrowIfNullOrWhiteSpace(() => hashedPassword);

            return PasswordUtil.ComputeHash(password!) == hashedPassword;
        }
    }
}
