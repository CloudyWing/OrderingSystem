using CloudyWing.OrderingSystem.DataAccess.Entities;
using CloudyWing.OrderingSystem.Infrastructure.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudyWing.OrderingSystem.Domain.Services.OrderModel {
    public class OrderService(ApplicationDbContext dbContext, ILogger<OrderService> logger)
                : QueryableService<Order, OrderService>(dbContext, logger) {
        public async Task<bool> CreateAsync(OrderEditor editor) {
            ExceptionUtils.ThrowIfNull(() => editor);

            return await UseTransactionAsync(async (transaction) => {
                Order entity = Mapper.Map<Order>(editor);
                DbSet.Add(entity);

                bool isOk = await SaveChangesAsync() == 1;

                editor.SetId(entity.Id);

                foreach (OrderDetailEditor detailEditor in editor.OrderDetailEditors) {
                    DbContext.OrderDetails.Add(new OrderDetail {
                        Id = Guid.NewGuid(),
                        OrderId = entity.Id,
                        ProductId = detailEditor.ProductId,
                        Quantity = detailEditor.Quantity,
                        Cost = detailEditor.Cost,
                        Remark = detailEditor.Remark
                    });
                }

                if (await SaveChangesAsync() != editor.OrderDetailEditors.Count) {
                    isOk = false;
                    await transaction.RollbackAsync();
                }

                return isOk;
            });
        }

        public async Task<bool> UpdateAsync(OrderEditor editor) {
            ExceptionUtils.ThrowIfNull(() => editor);
            ExceptionUtils.ThrowIfNull(() => editor.Id);

            Order? entity = await DbContext.Orders.SingleOrDefaultAsync(x => x.Id == editor.Id);

            ExceptionUtils.ThrowIfItemNotFound(entity);

            return await UseTransactionAsync(async (transaction) => {
                IQueryable<OrderDetail> detailEntities = DbContext.OrderDetails.Where(x => x.OrderId == editor.Id);
                DbContext.OrderDetails.RemoveRange(detailEntities);

                await SaveChangesAsync();

                Mapper.Map(editor, entity);

                bool isOk = true;

                foreach (OrderDetailEditor detailEditor in editor.OrderDetailEditors) {
                    DbContext.OrderDetails.Add(new OrderDetail {
                        Id = Guid.NewGuid(),
                        OrderId = editor.Id!.Value,
                        ProductId = detailEditor.ProductId,
                        Quantity = detailEditor.Quantity,
                        Cost = detailEditor.Cost,
                        Remark = detailEditor.Remark
                    });
                }

                if (await SaveChangesAsync() != editor.OrderDetailEditors.Count) {
                    isOk = false;
                    await transaction.RollbackAsync();
                }

                return isOk;
            });
        }

        public async Task<bool> DeleteAsync(Guid id) {
            Order? entity = await DbSet.Where(x => x.Id == id).SingleOrDefaultAsync();
            ExceptionUtils.ThrowIfItemNotFound(entity);

            List<OrderDetail> orderDetails = [.. DbContext.OrderDetails.Where(x => x.OrderId == id)];
            DbContext.OrderDetails.RemoveRange(orderDetails);
            DbContext.Orders.Remove(entity!);

            return await SaveChangesAsync() == orderDetails.Count + 1;
        }
    }
}
