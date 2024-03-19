using CloudyWing.OrderingSystem.DataAccess.Entities;
using CloudyWing.OrderingSystem.Infrastructure.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudyWing.OrderingSystem.Domain.Services.ProductModel {
    public class ProductService(ApplicationDbContext context, ILogger<ProductService> logger)
                : QueryableService<Product, ProductService>(context, logger) {
        public async Task<bool> CreateAsync(ProductEditor editor) {
            ExceptionUtils.ThrowIfNull(() => editor);

            Product entity = Mapper.Map<Product>(editor);
            entity.DisplayOrder = await GetNewDisplayOrderAsync();
            DbSet.Add(entity);
            bool isOk = await SaveChangesAsync() == 1;

            editor.SetId(entity.Id);

            return isOk;
        }

        private async Task<int> GetNewDisplayOrderAsync() {
            return await DbSet
                        .OrderByDescending(x => x.DisplayOrder)
                        .Select(x => x.DisplayOrder)
                        .FirstOrDefaultAsync() + 1;
        }

        public async Task<bool> UpdateAsync(ProductEditor editor) {
            ExceptionUtils.ThrowIfNull(() => editor);
            ExceptionUtils.ThrowIfNull(() => editor.Id);

            Product? entity = await FindByIdAsync(editor.Id!.Value);

            ExceptionUtils.ThrowIfItemNotFound(entity);

            Mapper.Map(editor, entity);
            return DbContext.Entry(entity!).State == EntityState.Unchanged || await SaveChangesAsync() == 1;
        }

        private async Task<Product?> FindByIdAsync(Guid id) {
            return await DbSet.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> DeleteAsync(Guid id) {
            Product? entity = await FindByIdAsync(id);
            ExceptionUtils.ThrowIfItemNotFound(entity);

            bool isOk = await UseTransactionAsync(async () => {
                DbSet.Remove(entity!);
                bool isOk = await SaveChangesAsync() == 1;

                await RefreshDisplayOrderAsync();

                return isOk;
            });
            return isOk;
        }

        private async Task RefreshDisplayOrderAsync() {
            List<Product> list = await DbSet.ToListAsync();
            int i = 1;

            foreach (Product item in list) {
                item.DisplayOrder = i++;
                DbSet.Update(item);
            }
            await SaveChangesAsync();
        }

        public async Task<bool> MoveUpAsync(Guid id) {
            Product? currentEntity = await FindByIdAsync(id);
            ExceptionUtils.ThrowIfItemNotFound(currentEntity);

            Product prevEntity = DbSet.Where(
                    x => x.DisplayOrder == (currentEntity!.DisplayOrder - 1)
                )
                .Single();

            currentEntity!.DisplayOrder--;
            prevEntity.DisplayOrder++;

            return await SaveChangesAsync() == 2;
        }

        public async Task<bool> MoveDownAsync(Guid id) {
            Product? currentEntity = await FindByIdAsync(id);
            ExceptionUtils.ThrowIfItemNotFound(currentEntity);

            Product nextEntity = DbSet.Where(
                    x => x.DisplayOrder == (currentEntity!.DisplayOrder + 1)
                )
                .Single();

            DateTime now = DateTime.Now;

            currentEntity!.DisplayOrder++;
            nextEntity.DisplayOrder--;

            return await SaveChangesAsync() == 2;
        }
    }
}
