using CloudyWing.OrderingSystem.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudyWing.OrderingSystem.Domain.Services.OrderModel;

public class OrderDetailService(ApplicationDbContext dbContext, ILogger<OrderDetailService> logger)
            : QueryableService<OrderDetail, OrderDetailService>(dbContext, logger) {
    protected override IQueryable<OrderDetail> CreateDbSource(IQueryable<OrderDetail> unjoinedSource) {
        return unjoinedSource.Include(x => x.Product);
    }
}