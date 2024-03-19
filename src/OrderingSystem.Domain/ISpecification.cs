using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace CloudyWing.OrderingSystem.Domain.Services {
    public interface ISpecification<TEntity, TUnjoinedEntity>
        where TEntity : class
        where TUnjoinedEntity : class {
        Func<IQueryable<TUnjoinedEntity>, IIncludableQueryable<TUnjoinedEntity, object>>? IncludeGenerator { get; }

        IList<string> IncludeProperties { get; }

        Expression<Func<TEntity, bool>>? Filter { get; }

        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderByGenerator { get; }

        Expression<Func<TEntity, object>>? GroupBy { get; }

        int? Take { get; }

        int? Skip { get; }
    }
}
