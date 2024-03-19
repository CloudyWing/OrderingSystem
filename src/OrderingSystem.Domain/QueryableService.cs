using System.Collections.ObjectModel;
using System.Linq.Expressions;
using CloudyWing.OrderingSystem.DataAccess.Entities;
using CloudyWing.OrderingSystem.Infrastructure.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace CloudyWing.OrderingSystem.Domain.Services {
    public abstract class QueryableService<TEntity, TUnjoinedEntity, TService>(ApplicationDbContext dbContext, ILogger<TService> logger)
        : ServiceBase<TUnjoinedEntity, TService>(dbContext, logger)
        where TEntity : class
        where TUnjoinedEntity : class
        where TService : ServiceBase<TService> {
        protected abstract IQueryable<TEntity> CreateDbSource(IQueryable<TUnjoinedEntity> unjoinedSource);

        public async Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>>? filter = null) {
            return await IsExistsAsync(
                new Specification<TEntity, TUnjoinedEntity>(filter, null, null)
            );
        }

        public async Task<bool> IsExistsAsync(ISpecification<TEntity, TUnjoinedEntity> spec) {
            ExceptionUtils.ThrowIfNull(() => spec);

            return await ApplySpecification(spec).AnyAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null) {
            return await CountAsync(
                new Specification<TEntity, TUnjoinedEntity>(filter, null, null)
            );
        }

        public async Task<int> CountAsync(ISpecification<TEntity, TUnjoinedEntity> spec) {
            ExceptionUtils.ThrowIfNull(() => spec);

            return await ApplySpecification(spec).CountAsync();
        }

        public async Task<TRecord> GetFirstAsync<TRecord>(
            Expression<Func<TEntity, TRecord>> selector,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderByGenerator = null,
            Func<IQueryable<TUnjoinedEntity>, IIncludableQueryable<TUnjoinedEntity, object>>? includeGenerator = null
        ) where TRecord : class {
            ExceptionUtils.ThrowIfNull(() => selector);

            return await GetFirstAsync(
                selector,
                new Specification<TEntity, TUnjoinedEntity>(filter, orderByGenerator, includeGenerator)
            );
        }

        public async Task<TRecord> GetFirstAsync<TRecord>(
            Expression<Func<TEntity, TRecord>> selector,
            ISpecification<TEntity, TUnjoinedEntity> spec
        ) where TRecord : class {
            ExceptionUtils.ThrowIfNull(() => selector);
            ExceptionUtils.ThrowIfNull(() => spec);

            return await ApplySpecification(spec).Select(selector).FirstAsync();
        }

        public async Task<TEntity> GetFirstAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderByGenerator = null,
            Func<IQueryable<TUnjoinedEntity>, IIncludableQueryable<TUnjoinedEntity, object>>? includeGenerator = null
        ) {
            return await GetFirstAsync(
                new Specification<TEntity, TUnjoinedEntity>(filter, orderByGenerator, includeGenerator)
            );
        }

        public async Task<TEntity> GetFirstAsync(ISpecification<TEntity, TUnjoinedEntity> spec) {
            ExceptionUtils.ThrowIfNull(() => spec);

            return await ApplySpecification(spec).FirstAsync();
        }

        public async Task<TRecord?> GetFirstOrDefaultAsync<TRecord>(
            Expression<Func<TEntity, TRecord>> selector,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderByGenerator = null,
            Func<IQueryable<TUnjoinedEntity>, IIncludableQueryable<TUnjoinedEntity, object>>? includeGenerator = null
        ) where TRecord : class {
            ExceptionUtils.ThrowIfNull(() => selector);

            return await GetFirstOrDefaultAsync(
                selector,
                new Specification<TEntity, TUnjoinedEntity>(filter, orderByGenerator, includeGenerator)
            );
        }

        public async Task<TRecord?> GetFirstOrDefaultAsync<TRecord>(
            Expression<Func<TEntity, TRecord>> selector,
            ISpecification<TEntity, TUnjoinedEntity> spec
        ) where TRecord : class {
            ExceptionUtils.ThrowIfNull(() => selector);
            ExceptionUtils.ThrowIfNull(() => spec);

            return await ApplySpecification(spec).Select(selector).FirstOrDefaultAsync();
        }

        public async Task<TEntity?> GetFirstOrDefaultAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderByGenerator = null,
            Func<IQueryable<TUnjoinedEntity>, IIncludableQueryable<TUnjoinedEntity, object>>? includeGenerator = null
        ) {
            return await GetFirstOrDefaultAsync(
                new Specification<TEntity, TUnjoinedEntity>(filter, orderByGenerator, includeGenerator)
            );
        }

        public async Task<TEntity?> GetFirstOrDefaultAsync(ISpecification<TEntity, TUnjoinedEntity> spec) {
            ExceptionUtils.ThrowIfNull(() => spec);

            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<TRecord> GetSingleAsync<TRecord>(
            Expression<Func<TEntity, TRecord>> selector,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TUnjoinedEntity>, IIncludableQueryable<TUnjoinedEntity, object>>? includeGenerator = null
        ) where TRecord : class {
            ExceptionUtils.ThrowIfNull(() => selector);

            return await GetSingleAsync(
                selector,
                new Specification<TEntity, TUnjoinedEntity>(filter, includeGenerator: includeGenerator)
            );
        }

        public async Task<TRecord> GetSingleAsync<TRecord>(
            Expression<Func<TEntity, TRecord>> selector,
            ISpecification<TEntity, TUnjoinedEntity> spec
        ) where TRecord : class {
            ExceptionUtils.ThrowIfNull(() => selector);
            ExceptionUtils.ThrowIfNull(() => spec);

            return await ApplySpecification(spec).Select(selector).SingleAsync();
        }

        public async Task<TEntity> GetSingleAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TUnjoinedEntity>, IIncludableQueryable<TUnjoinedEntity, object>>? includeGenerator = null
        ) {
            return await GetSingleAsync(
                new Specification<TEntity, TUnjoinedEntity>(filter, includeGenerator: includeGenerator)
            );
        }

        public async Task<TEntity> GetSingleAsync(ISpecification<TEntity, TUnjoinedEntity> spec) {
            ExceptionUtils.ThrowIfNull(() => spec);

            return await ApplySpecification(spec).SingleAsync();
        }

        public async Task<TRecord?> GetSingleOrDefaultAsync<TRecord>(
            Expression<Func<TEntity, TRecord>> selector,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TUnjoinedEntity>, IIncludableQueryable<TUnjoinedEntity, object>>? includeGenerator = null
        ) where TRecord : class {
            ExceptionUtils.ThrowIfNull(() => selector);

            return await GetSingleOrDefaultAsync(
                selector,
                new Specification<TEntity, TUnjoinedEntity>(filter, includeGenerator: includeGenerator)
            );
        }

        public async Task<TRecord?> GetSingleOrDefaultAsync<TRecord>(
            Expression<Func<TEntity, TRecord>> selector,
            ISpecification<TEntity, TUnjoinedEntity> spec
        ) where TRecord : class {
            ExceptionUtils.ThrowIfNull(() => selector);
            ExceptionUtils.ThrowIfNull(() => spec);

            return await ApplySpecification(spec).Select(selector).SingleOrDefaultAsync();
        }

        public async Task<TEntity?> GetSingleOrDefaultAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TUnjoinedEntity>, IIncludableQueryable<TUnjoinedEntity, object>>? includeGenerator = null
        ) {
            return await GetSingleOrDefaultAsync(
                new Specification<TEntity, TUnjoinedEntity>(filter, includeGenerator: includeGenerator)
            );
        }

        public async Task<TEntity?> GetSingleOrDefaultAsync(ISpecification<TEntity, TUnjoinedEntity> spec) {
            ExceptionUtils.ThrowIfNull(() => spec);

            return await ApplySpecification(spec).SingleOrDefaultAsync();
        }

        public async Task<IReadOnlyList<TRecord>> GetListAsync<TRecord>(
            Expression<Func<TEntity, TRecord>> selector,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderByGenerator = null,
            Func<IQueryable<TUnjoinedEntity>, IIncludableQueryable<TUnjoinedEntity, object>>? includeGenerator = null
        ) where TRecord : class {
            ExceptionUtils.ThrowIfNull(() => selector);

            return await GetListAsync(
                selector,
                new Specification<TEntity, TUnjoinedEntity>(filter, orderByGenerator, includeGenerator)
            );
        }

        public async Task<IReadOnlyList<TRecord>> GetListAsync<TRecord>(
            Expression<Func<TEntity, TRecord>> selector,
            ISpecification<TEntity, TUnjoinedEntity> spec
        ) where TRecord : class {
            ExceptionUtils.ThrowIfNull(() => selector);
            ExceptionUtils.ThrowIfNull(() => spec);

            List<TRecord> list = await ApplySpecification(spec).Select(selector).ToListAsync();
            return new ReadOnlyCollection<TRecord>(list);
        }

        public async Task<IReadOnlyList<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderByGenerator = null,
            Func<IQueryable<TUnjoinedEntity>, IIncludableQueryable<TUnjoinedEntity, object>>? includeGenerator = null
        ) {
            return await GetListAsync(
                new Specification<TEntity, TUnjoinedEntity>(filter, orderByGenerator, includeGenerator)
            );
        }

        public async Task<IReadOnlyList<TEntity>> GetListAsync(ISpecification<TEntity, TUnjoinedEntity> spec) {
            ExceptionUtils.ThrowIfNull(() => spec);

            List<TEntity> list = await ApplySpecification(spec).ToListAsync();
            return new ReadOnlyCollection<TEntity>(list);
        }

        public async Task<PagedList<TRecord>> GetPagedListAsync<TRecord>(
            Expression<Func<TEntity, TRecord>> selector,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderByGenerator,
            int pageNumber,
            int pageSize,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TUnjoinedEntity>, IIncludableQueryable<TUnjoinedEntity, object>>? includeGenerator = null
        ) where TRecord : class {
            ExceptionUtils.ThrowIfNull(() => selector);
            ExceptionUtils.ThrowIfNull(() => orderByGenerator);

            int totalCount = await CountAsync(new Specification<TEntity, TUnjoinedEntity> {
                Filter = filter,
                IncludeGenerator = includeGenerator
            });

            Specification<TEntity, TUnjoinedEntity> spec = new() {
                OrderByGenerator = orderByGenerator,
                Skip = (pageNumber - 1) * pageSize,
                Take = pageSize,
                Filter = filter,
                IncludeGenerator = includeGenerator
            };

            List<TRecord> list = await ApplySpecification(spec).Select(selector).ToListAsync();
            PagingMetadata metaData = new(pageNumber, pageSize, totalCount);
            return new PagedList<TRecord>(list, metaData);
        }

        public async Task<PagedList<TEntity>> GetPagedListAsync(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderByGenerator,
            int pageNumber,
            int pageSize,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TUnjoinedEntity>, IIncludableQueryable<TUnjoinedEntity, object>>? includeGenerator = null
        ) {
            ExceptionUtils.ThrowIfNull(() => orderByGenerator);

            int totalCount = await CountAsync(new Specification<TEntity, TUnjoinedEntity> {
                Filter = filter,
                IncludeGenerator = includeGenerator
            });

            Specification<TEntity, TUnjoinedEntity> spec = new() {
                OrderByGenerator = orderByGenerator,
                Skip = (pageNumber - 1) * pageSize,
                Take = pageSize,
                Filter = filter,
                IncludeGenerator = includeGenerator
            };

            List<TEntity> list = await ApplySpecification(spec).ToListAsync();
            PagingMetadata metaData = new(pageNumber, pageSize, totalCount);
            return new PagedList<TEntity>(list, metaData);
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity, TUnjoinedEntity>? spec)
            => SpecificationEvaluator<TEntity, TUnjoinedEntity>.GetQuery(DbSet.AsNoTracking(), CreateDbSource, spec ?? new Specification<TEntity, TUnjoinedEntity>());
    }

    public abstract class QueryableService<TEntity, TService>(ApplicationDbContext dbContext, ILogger<TService> logger)
        : QueryableService<TEntity, TEntity, TService>(dbContext, logger)
        where TEntity : class
        where TService : ServiceBase<TService> {
        protected override IQueryable<TEntity> CreateDbSource(IQueryable<TEntity> unjoinedSource) {
            return unjoinedSource;
        }
    }
}
