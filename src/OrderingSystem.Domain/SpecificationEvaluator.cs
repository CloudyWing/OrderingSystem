using Microsoft.EntityFrameworkCore;

namespace CloudyWing.OrderingSystem.Domain.Services {
    public static class SpecificationEvaluator<TEntity, TUnjoinedEntity>
        where TEntity : class
        where TUnjoinedEntity : class {
        public static IQueryable<TEntity> GetQuery(
             IQueryable<TUnjoinedEntity> queryableObj, Func<IQueryable<TUnjoinedEntity>, IQueryable<TEntity>> joinQueryFunc, ISpecification<TEntity, TUnjoinedEntity> specification
        ) {
            IQueryable<TUnjoinedEntity> source = queryableObj;

            if (specification.IncludeGenerator != null) {
                source = specification.IncludeGenerator(source);
            }

            if (specification.IncludeProperties != null) {
                foreach (string prop in specification.IncludeProperties) {
                    source = source.Include(prop);
                }
            }

            IQueryable<TEntity> joinedSource = joinQueryFunc(source);

            if (specification.Filter != null) {
                joinedSource = joinedSource.Where(specification.Filter);
            }

            if (specification.OrderByGenerator != null) {
                joinedSource = specification.OrderByGenerator(joinedSource);
            }

            if (specification.GroupBy != null) {
                joinedSource = joinedSource.GroupBy(specification.GroupBy).SelectMany(x => x);
            }

            if (specification.Skip.HasValue) {
                source = source.Skip(specification.Skip.Value);
            }

            if (specification.Take.HasValue) {
                joinedSource = joinedSource.Take(specification.Take.Value);
            }

            return joinedSource;
        }
    }
}
