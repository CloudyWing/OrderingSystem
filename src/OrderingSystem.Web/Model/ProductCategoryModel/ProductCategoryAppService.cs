using CloudyWing.OrderingSystem.Domain.Services.ProductModel;
using CloudyWing.OrderingSystem.Infrastructure.Util;
using OrderingSystem.Domain.Services.ProductModel;

namespace CloudyWing.OrderingSystem.Web.Model.ProductCategoryModel {
    public class ProductCategoryAppService : ApplicationService<ProductCategoryAppService> {
        private readonly ProductCategoryService productCategoryService;

        public ProductCategoryAppService(IHttpContextAccessor httpContextAccessor,
            ILogger<ProductCategoryAppService> logger,
            ProductCategoryService productCategoryService)
            : base(httpContextAccessor, logger) {
            ExceptionUtils.ThrowIfNull(() => productCategoryService);

            this.productCategoryService = productCategoryService;
        }

        public async Task<IReadOnlyList<IndexListItemViewModel>> GetListAsync() {
            return await productCategoryService.GetListAsync(
                x => new IndexListItemViewModel {
                    Id = x.Id,
                    DisplayOrder = x.DisplayOrder,
                    Name = x.Name
                },
                null,
                x => x.OrderBy(y => y.DisplayOrder)
            );
        }

        public async Task<ResponseResult> MoveUpAsync(Guid id) {
            ResponseResult<IReadOnlyList<IndexListItemViewModel>> result = new();
            if (await productCategoryService.MoveUpAsync(id)) {
                result.Data = await GetListAsync();
            }

            return result;
        }

        public async Task<ResponseResult> MoveDownAsync(Guid id) {
            ResponseResult<IReadOnlyList<IndexListItemViewModel>> result = new();
            if (await productCategoryService.MoveDownAsync(id)) {
                result.Data = await GetListAsync();
            }

            return result;
        }

        public async Task<ResponseResult> DeleteAsync(Guid id) {
            ResponseResult<IReadOnlyList<IndexListItemViewModel>> result = new();
            if (await productCategoryService.DeleteAsync(id)) {
                result.Data = await GetListAsync();
            }

            return result;
        }

        public async Task<UpsertViewModel?> GetItemAsync(Guid id) {
            return await productCategoryService.GetSingleOrDefaultAsync(
                x => new UpsertViewModel {
                    Id = x.Id,
                    Name = x.Name
                },
                x => x.Id == id
            );
        }

        public async Task<bool> UpsertAsync(UpsertViewModel viewModel) {
            ProductCategoryEditor editor = viewModel.IsExisting
                    ? new ProductCategoryEditor(viewModel.Id!.Value)
                    : new ProductCategoryEditor();

            editor.Name = viewModel.Name;

            return viewModel.IsExisting
                ? await productCategoryService.UpdateAsync(editor)
                : await productCategoryService.CreateAsync(editor);
        }
    }
}
