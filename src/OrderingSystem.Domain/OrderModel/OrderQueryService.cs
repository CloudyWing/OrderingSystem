using CloudyWing.OrderingSystem.DataAccess.Entities;
using CloudyWing.OrderingSystem.Domain.Services;
using Microsoft.Extensions.Logging;

namespace CloudyWing.OrderingSystem.Domain.OrderModel {
    public class OrderQueryService(ApplicationDbContext dbContext, ILogger<OrderQueryService> logger)
        : QueryableService<OrderQueryEntity, Order, OrderQueryService>(dbContext, logger) {
        protected override IQueryable<OrderQueryEntity> CreateDbSource(IQueryable<Order> unjoinedSource) {
            return unjoinedSource
                .Join(DbContext.Users, x => x.OrderUserEmail, x => x.Email, (o, u) => new OrderQueryEntity {
                    Order = o,
                    User = u
                });
        }
    }
}
