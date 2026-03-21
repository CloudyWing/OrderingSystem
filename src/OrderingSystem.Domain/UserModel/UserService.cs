using CloudyWing.OrderingSystem.DataAccess.Entities;
using CloudyWing.OrderingSystem.Domain.Util;
using CloudyWing.OrderingSystem.Infrastructure.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudyWing.OrderingSystem.Domain.Services.UserModel;

public class UserService(ApplicationDbContext dbContext, ILogger<UserService> logger)
            : QueryableService<User, UserService>(dbContext, logger) {
    public async Task<bool> CreateAsync(UserEditor editor) {
        ExceptionUtils.ThrowIfNull(() => editor);

        User entity = Mapper.Map<User>(editor);
        DbSet.Add(entity);

        return await SaveChangesAsync() == 1;
    }

    public async Task<bool> IsExistsAsync(string? email) {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        string normalizedEmail = email.Trim().ToLower();

        return await IsExistsAsync(x => x.Email != null && x.Email.ToLower() == normalizedEmail);
    }

    public async Task<User?> GetSingleOrDefaultAsync(string? email) {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        string normalizedEmail = email.Trim().ToLower();

        return await DbSet.AsNoTracking().SingleOrDefaultAsync(x => x.Email != null && x.Email.ToLower() == normalizedEmail);
    }

    public bool VerifyPassword(string? password, string? hashedPassword) {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        ArgumentException.ThrowIfNullOrWhiteSpace(hashedPassword);

        return PasswordUtil.ComputeHash(password) == hashedPassword;
    }
}