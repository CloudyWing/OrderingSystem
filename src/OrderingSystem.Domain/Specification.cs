using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace CloudyWing.OrderingSystem.Domain.Services {
    public class Specification<TEntity, TUnjoinedEntity> : ISpecification<TEntity, TUnjoinedEntity>
        where TEntity : class
        where TUnjoinedEntity : class {
        public Specification() { }

        public Specification(
            Expression<Func<TEntity, bool>>? filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderByGenerator = null,
            Func<IQueryable<TUnjoinedEntity>, IIncludableQueryable<TUnjoinedEntity, object>>? includeGenerator = null) {
            Filter = filter;
            OrderByGenerator = orderByGenerator;
            IncludeGenerator = includeGenerator;
        }

        public Func<IQueryable<TUnjoinedEntity>, IIncludableQueryable<TUnjoinedEntity, object>>? IncludeGenerator { get; set; }

        public IList<string> IncludeProperties { get; } = new List<string>();

        public Expression<Func<TEntity, bool>>? Filter { get; set; }

        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderByGenerator { get; set; }

        public Expression<Func<TEntity, object>>? GroupBy { get; set; }

        public int? Take { get; set; }

        public int? Skip { get; set; }
    }
}
