using AutoMapper;
using CloudyWing.OrderingSystem.DataAccess.Entities;
using CloudyWing.OrderingSystem.Infrastructure.DependencyInjection;
using CloudyWing.OrderingSystem.Infrastructure.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace CloudyWing.OrderingSystem.Domain.Services {
    public abstract class ServiceBase<TService> : IDomainService, IScopedDependency
            where TService : ServiceBase<TService> {
        private static readonly Lazy<IMapper> mapper = new(() => {
            MapperConfiguration config = new(cfg => {
                cfg.AddProfile(new ServiceProfile());
            });
            config.AssertConfigurationIsValid();
            return config.CreateMapper();
        });

        protected ServiceBase(ApplicationDbContext dbContext, ILogger<TService> logger) {
            ExceptionUtils.ThrowIfNull(() => dbContext);
            ExceptionUtils.ThrowIfNull(() => logger);

            DbContext = dbContext;
            Logger = logger;
        }

        protected ApplicationDbContext DbContext { get; }

        protected IMapper Mapper => mapper.Value;

        protected ILogger<TService> Logger { get; }

        private readonly IList<IDisposable> disposableObjects = [];

        protected async Task<int> SaveChangesAsync() {
            return await DbContext.SaveChangesAsync();
        }

        protected async Task UseTransactionAsync(Func<Task> executor) {
            if (DbContext.Database.CurrentTransaction == null) {
                using IDbContextTransaction transaction = await DbContext.Database.BeginTransactionAsync();
                try {
                    await executor();
                    // 避免Nested UseTransaction裡，會不會有自行Rollback的，所以還都加處理
                    if (DbContext.Database.GetDbConnection() != null) {
                        await transaction.CommitAsync();
                    }
                } catch {
                    if (DbContext.Database.GetDbConnection() != null) {
                        await transaction.RollbackAsync();
                    }
                    throw;
                }
            } else {
                await executor();
            }
        }

        protected async Task UseTransactionAsync(Func<IDbContextTransaction, Task> executor) {
            if (DbContext.Database.CurrentTransaction == null) {
                using IDbContextTransaction transaction = await DbContext.Database.BeginTransactionAsync();
                try {
                    await executor(transaction);
                    // 避免Nested UseTransaction裡，會不會有自行Rollback的，所以還都加處理
                    if (DbContext.Database.GetDbConnection() != null) {
                        await transaction.CommitAsync();
                    }
                } catch {
                    if (DbContext.Database.GetDbConnection() != null) {
                        await transaction.RollbackAsync();
                    }
                    throw;
                }
            } else {
                await executor(DbContext.Database.CurrentTransaction);
            }
        }

        protected async Task<T> UseTransactionAsync<T>(Func<Task<T>> executor) {
            if (DbContext.Database.CurrentTransaction == null) {
                using IDbContextTransaction transaction = await DbContext.Database.BeginTransactionAsync();
                try {
                    T result = await executor();
                    // 避免Nested UseTransaction裡，會不會有自行Rollback的，所以還都加處理
                    if (DbContext.Database.GetDbConnection() != null) {
                        await transaction.CommitAsync();
                    }
                    return result;
                } catch {
                    if (DbContext.Database.GetDbConnection() != null) {
                        await transaction.RollbackAsync();
                    }
                    throw;
                }
            } else {
                return await executor();
            }
        }

        protected async Task<T> UseTransactionAsync<T>(Func<IDbContextTransaction, Task<T>> executor) {
            if (DbContext.Database.CurrentTransaction == null) {
                using IDbContextTransaction transaction = await DbContext.Database.BeginTransactionAsync();
                try {
                    T result = await executor(transaction);
                    // 如果executor已自行Rollback，會導致Connection為NuLL，此時Commit會有問題
                    if (DbContext.Database.GetDbConnection() != null) {
                        await transaction.CommitAsync();
                    }
                    return result;
                } catch {
                    if (DbContext.Database.GetDbConnection() != null) {
                        await transaction.RollbackAsync();
                    }
                    throw;
                }
            } else {
                return await executor(DbContext.Database.CurrentTransaction);
            }
        }

        protected void AddDisposableObject(IDisposable disposable) {
            if (disposable != null) {
                disposableObjects.Add(disposable);
            }
        }

        #region IDisposable Support
        private bool disposed = false;

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void AddDisposableObject(params IDisposable[] objects) {
            foreach (IDisposable disposableObj in objects) {
                disposableObjects.Add(disposableObj);
            }
        }

        protected virtual void Dispose(bool disposing) {
            if (disposed) {
                return;
            }

            if (disposing) {
                foreach (IDisposable disposableObj in disposableObjects) {
                    disposableObj?.Dispose();
                }
                DbContext.Dispose();
            }

            disposed = true;
        }
        #endregion
    }

    public abstract class ServiceBase<TEntity, TService>
        : ServiceBase<TService>
            where TEntity : class
            where TService : ServiceBase<TService> {
        protected ServiceBase(ApplicationDbContext dbContext, ILogger<TService> logger) : base(dbContext, logger) { }

        protected DbSet<TEntity> DbSet => DbContext.Set<TEntity>();
    }
}
