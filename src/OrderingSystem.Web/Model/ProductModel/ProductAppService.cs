using CloudyWing.OrderingSystem.Domain.Services.ProductModel;
using CloudyWing.OrderingSystem.Infrastructure.Util;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CloudyWing.OrderingSystem.Web.Model.ProductModel;

public class ProductAppService : ApplicationService<ProductAppService> {
    private readonly ProductService productService;
    private readonly ProductCategoryService productCategoryService;

    public ProductAppService(IHttpContextAccessor httpContextAccessor, ILogger<ProductAppService> logger, ProductService productService, ProductCategoryService productCategoryService)
        : base(httpContextAccessor, logger) {
        ExceptionUtils.ThrowIfNull(() => productService);
        ExceptionUtils.ThrowIfNull(() => productCategoryService);

        this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
        this.productCategoryService = productCategoryService ?? throw new ArgumentNullException(nameof(productCategoryService));
    }

    public async Task<IEnumerable<SelectListItem>> GetCategoriesAsync() {
        IReadOnlyList<DataAccess.Entities.ProductCategory> list = await productCategoryService.GetListAsync(
            x => x,
            orderBy: x => x.OrderBy(y => y.DisplayOrder)
        );

        return new SelectList(list, "Id", "Name");
    }

    public async Task<IReadOnlyList<IndexListItemViewModel>> GetListAsync() {
        return await productService.GetListAsync(
            x => new IndexListItemViewModel {
                Id = x.Id,
                DisplayOrder = x.DisplayOrder,
                Name = x.Name,
                Price = x.Price,
                CategoryName = x.Category == null ? "" : x.Category.Name ?? ""
            },
            null,
            x => x.OrderBy(y => y.DisplayOrder)
        );
    }

    public async Task<ResponseResult> MoveUpAsync(Guid id) {
        ResponseResult<IReadOnlyList<IndexListItemViewModel>> result = new();
        if (await productService.MoveUpAsync(id)) {
            result.Data = await GetListAsync();
        }

        return result;
    }

    public async Task<ResponseResult> MoveDownAsync(Guid id) {
        ResponseResult<IReadOnlyList<IndexListItemViewModel>> result = new();
        if (await productService.MoveDownAsync(id)) {
            result.Data = await GetListAsync();
        }

        return result;
    }

    public async Task<ResponseResult> DeleteAsync(Guid id) {
        ResponseResult<IReadOnlyList<IndexListItemViewModel>> result = new();
        if (await productService.DeleteAsync(id)) {
            result.Data = await GetListAsync();
        }

        return result;
    }

    public async Task<UpsertViewModel?> GetItemAsync(Guid id) {
        return await productService.GetSingleOrDefaultAsync(
            x => new UpsertViewModel {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                CategoryId = x.CategoryId
            },
            x => x.Id == id
        );
    }

    public async Task<bool> UpsertAsync(UpsertViewModel viewModel) {
        ProductEditor editor = viewModel.Id is Guid id
                ? new ProductEditor(id)
                : new ProductEditor();

        editor.Name = viewModel.Name;
        editor.Price = viewModel.Price;
        editor.CategoryId = viewModel.CategoryId;

        return viewModel.Id.HasValue
            ? await productService.UpdateAsync(editor)
            : await productService.CreateAsync(editor);
    }
}