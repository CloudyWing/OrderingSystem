using CloudyWing.OrderingSystem.DataAccess.Entities;
using CloudyWing.OrderingSystem.Infrastructure.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudyWing.OrderingSystem.Domain.Services.ProductModel {
    public class ProductCategoryService(ApplicationDbContext dbContext, ILogger<ProductCategoryService> logger)
                : QueryableService<ProductCategory, ProductCategoryService>(dbContext, logger) {
        public async Task<bool> CreateAsync(ProductCategoryEditor editor) {
            ExceptionUtils.ThrowIfNull(() => editor);

            ProductCategory entity = Mapper.Map<ProductCategory>(editor);
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

        public async Task<bool> UpdateAsync(ProductCategoryEditor editor) {
            ExceptionUtils.ThrowIfNull(() => editor);
            ExceptionUtils.ThrowIfNull(() => editor.Id);

            ProductCategory? entity = await FindByIdAsync(editor.Id!.Value);

            ExceptionUtils.ThrowIfItemNotFound(entity);

            Mapper.Map(editor, entity);
            return DbContext.Entry(entity!).State == EntityState.Unchanged || await SaveChangesAsync() == 1;
        }

        private async Task<ProductCategory?> FindByIdAsync(Guid id) {
            return await DbSet.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> DeleteAsync(Guid id) {
            ProductCategory? entity = await FindByIdAsync(id);
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
            List<ProductCategory> list = await DbSet.ToListAsync();
            int i = 1;

            foreach (ProductCategory item in list) {
                item.DisplayOrder = i++;
            }
            await SaveChangesAsync();
        }

        public async Task<bool> MoveUpAsync(Guid id) {
            ProductCategory? currentEntity = await FindByIdAsync(id);
            ExceptionUtils.ThrowIfItemNotFound(currentEntity);

            ProductCategory prevEntity = DbSet.Where(
                    x => x.DisplayOrder == (currentEntity!.DisplayOrder - 1)
                )
                .Single();

            currentEntity!.DisplayOrder--;
            prevEntity.DisplayOrder++;

            return await SaveChangesAsync() == 2;
        }

        public async Task<bool> MoveDownAsync(Guid id) {
            ProductCategory? currentEntity = await FindByIdAsync(id);
            ExceptionUtils.ThrowIfItemNotFound(currentEntity);

            ProductCategory nextEntity = DbSet.Where(
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
