using CloudyWing.OrderingSystem.DataAccess.Entities;
using Microsoft.Extensions.Logging;

namespace CloudyWing.OrderingSystem.Domain.Services.OrderModel {
    public class OrderDetailService(ApplicationDbContext dbContext, ILogger<OrderDetailService> logger)
                : QueryableService<OrderDetail, OrderDetailService>(dbContext, logger) {
    }
}
