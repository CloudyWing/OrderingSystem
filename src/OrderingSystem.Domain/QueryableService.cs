using System.Collections.ObjectModel;
using System.Linq.Expressions;
using CloudyWing.OrderingSystem.DataAccess.Entities;
using CloudyWing.OrderingSystem.Infrastructure.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudyWing.OrderingSystem.Domain.Services;

public abstract class QueryableService<TEntity, TUnjoinedEntity, TService>(ApplicationDbContext dbContext, ILogger<TService> logger)
    : ServiceBase<TUnjoinedEntity, TService>(dbContext, logger)
    where TEntity : class
    where TUnjoinedEntity : class
    where TService : ServiceBase<TService> {
    protected abstract IQueryable<TEntity> CreateDbSource(IQueryable<TUnjoinedEntity> unjoinedSource);

    public async Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>>? filter = null) {
        return await ApplyFilter(filter).AnyAsync();
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null) {
        return await ApplyFilter(filter).CountAsync();
    }

    public async Task<TRecord> GetFirstAsync<TRecord>(
        Expression<Func<TEntity, TRecord>> selector,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null
    ) where TRecord : class {
        ExceptionUtils.ThrowIfNull(() => selector);

        return await ApplyQuery(filter, orderBy).Select(selector).FirstAsync();
    }

    public async Task<TRecord?> GetFirstOrDefaultAsync<TRecord>(
        Expression<Func<TEntity, TRecord>> selector,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null
    ) where TRecord : class {
        ExceptionUtils.ThrowIfNull(() => selector);

        return await ApplyQuery(filter, orderBy).Select(selector).FirstOrDefaultAsync();
    }

    public async Task<TRecord> GetSingleAsync<TRecord>(
        Expression<Func<TEntity, TRecord>> selector,
        Expression<Func<TEntity, bool>>? filter = null
    ) where TRecord : class {
        ExceptionUtils.ThrowIfNull(() => selector);

        return await ApplyQuery(filter, null).Select(selector).SingleAsync();
    }

    public async Task<TRecord?> GetSingleOrDefaultAsync<TRecord>(
        Expression<Func<TEntity, TRecord>> selector,
        Expression<Func<TEntity, bool>>? filter = null
    ) where TRecord : class {
        ExceptionUtils.ThrowIfNull(() => selector);

        return await ApplyQuery(filter, null).Select(selector).SingleOrDefaultAsync();
    }

    public async Task<IReadOnlyList<TRecord>> GetListAsync<TRecord>(
        Expression<Func<TEntity, TRecord>> selector,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null
    ) where TRecord : class {
        ExceptionUtils.ThrowIfNull(() => selector);

        List<TRecord> list = await ApplyQuery(filter, orderBy).Select(selector).ToListAsync();
        return new ReadOnlyCollection<TRecord>(list);
    }

    public async Task<PagedList<TRecord>> GetPagedListAsync<TRecord>(
        Expression<Func<TEntity, TRecord>> selector,
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null
    ) where TRecord : class {
        ExceptionUtils.ThrowIfNull(() => selector);

        int totalCount = await ApplyFilter(filter).CountAsync();

        List<TRecord> list = await ApplyQuery(filter, orderBy)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(selector)
            .ToListAsync();

        PagingMetadata metaData = new(pageNumber, pageSize, totalCount);
        return new PagedList<TRecord>(list, metaData);
    }

    private IQueryable<TEntity> ApplyFilter(Expression<Func<TEntity, bool>>? filter) {
        IQueryable<TUnjoinedEntity> source = DbSet.AsNoTracking();
        IQueryable<TEntity> query = CreateDbSource(source);

        if (filter != null) {
            query = query.Where(filter);
        }

        return query;
    }

    private IQueryable<TEntity> ApplyQuery(
        Expression<Func<TEntity, bool>>? filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy
    ) {
        IQueryable<TEntity> query = ApplyFilter(filter);

        if (orderBy != null) {
            query = orderBy(query);
        }

        return query;
    }
}

public abstract class QueryableService<TEntity, TService>(ApplicationDbContext dbContext, ILogger<TService> logger)
    : QueryableService<TEntity, TEntity, TService>(dbContext, logger)
    where TEntity : class
    where TService : ServiceBase<TService> {
    protected override IQueryable<TEntity> CreateDbSource(IQueryable<TEntity> unjoinedSource) {
        return unjoinedSource;
    }
}