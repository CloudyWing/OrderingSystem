using System.Security.Principal;
using CloudyWing.OrderingSystem.DataAccess.Entities;
using CloudyWing.OrderingSystem.Domain.Services.OrderModel;
using CloudyWing.OrderingSystem.Domain.Services.ProductModel;
using CloudyWing.OrderingSystem.Infrastructure.Util;
using CloudyWing.OrderingSystem.Web.Infrastructure.Extensions;

namespace CloudyWing.OrderingSystem.Web.Model.OrderModel;

public class OrderAppService : ApplicationService<OrderAppService> {
    private readonly OrderService orderService;
    private readonly OrderDetailService orderDetailService;
    private readonly ProductCategoryService productCategoryService;
    private readonly ProductService productService;

    public OrderAppService(IHttpContextAccessor httpContextAccessor, ILogger<OrderAppService> logger,
        OrderService orderService, OrderDetailService orderDetailService,
        ProductCategoryService productCategoryService, ProductService productService
    ) : base(httpContextAccessor, logger) {
        ExceptionUtils.ThrowIfNull(() => orderService);
        ExceptionUtils.ThrowIfNull(() => orderDetailService);
        ExceptionUtils.ThrowIfNull(() => productCategoryService);
        ExceptionUtils.ThrowIfNull(() => productService);

        this.orderService = orderService;
        this.orderDetailService = orderDetailService;
        this.productCategoryService = productCategoryService;
        this.productService = productService;
    }

    public async Task<IReadOnlyList<IndexListItemViewModel>> GetListAsync() {
        DateTime limitDate = DateTime.Today.AddDays(7);
        return await orderService.GetListAsync(
            x => new IndexListItemViewModel {
                Id = x.Id,
                Date = x.Date,
                Money = x.OrderDetails.Sum(y => y.Cost)
            },
            x => x.Date < limitDate,
            x => x.OrderBy(y => y.Date)
        );
    }

    public async Task<IReadOnlyList<DetailListItemViewModel>> GetDetailsAsync(Guid orderId) {
        return await orderDetailService.GetListAsync(
            x => new DetailListItemViewModel {
                ProductName = x.Product == null ? "" : x.Product.Name ?? "",
                Quantity = x.Quantity,
                Cost = x.Cost,
                Remark = x.Remark
            },
            x => x.OrderId == orderId,
            x => x.OrderBy(y => y.Product == null ? int.MaxValue : y.Product.DisplayOrder)
        );
    }

    public async Task<ResponseResult> DeleteAsync(Guid id) {
        ResponseResult<IReadOnlyList<IndexListItemViewModel>> result = new();
        if (await orderService.DeleteAsync(id)) {
            result.Data = await GetListAsync();
        }

        return result;
    }

    public async Task<DateTime> GetOrderDateAsync(Guid id) {
        DateTime? date = await orderService.GetDateAsync(id);
        if (!date.HasValue) {
            ExceptionUtils.ThrowItemNotFound();
        }

        return date.Value;
    }

    public async Task<ResponseResult> GetDetailsByUpsertAsync(Guid orderId) {
        return new ResponseResult<IReadOnlyList<OrderDetailEditor>> {
            Data = await orderDetailService.GetListAsync(
                x => new OrderDetailEditor {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Cost = x.Cost,
                    Remark = x.Remark

                },
                x => x.OrderId == orderId
            )
        };
    }

    public async Task<ResponseResult> GetProductCategoriesAsync() {
        return new ResponseResult<IReadOnlyList<ProductCategory>> {
            Data = await productCategoryService.GetListAsync(x => new ProductCategory {
                Id = x.Id,
                Name = x.Name,
                DisplayOrder = x.DisplayOrder
            })
        };
    }

    public async Task<ResponseResult> GetProductsAsync() {
        return new ResponseResult<IReadOnlyList<Product>> {
            Data = await productService.GetListAsync(x => new Product {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                CategoryId = x.CategoryId,
                DisplayOrder = x.DisplayOrder
            })
        };
    }

    public async Task<bool> UpsertAsync(UpsertViewModel viewModel) {
        OrderEditor orderEditor = viewModel.Id is Guid id ? new OrderEditor(id) : new OrderEditor();
        orderEditor.Date = viewModel.Date;

        IIdentity? userIdentity = HttpContextAccessor.HttpContext?.User.Identity;
        if (userIdentity is null) {
            throw new InvalidOperationException("目前沒有可用的使用者身分。");
        }

        orderEditor.OrderUserEmail = userIdentity.GetEmail();

        foreach (UpsertDetailViewModel detail in viewModel.Details) {
            orderEditor.OrderDetailEditors.Add(new OrderDetailEditor {
                ProductId = detail.ProductId,
                Quantity = detail.Quantity,
                Cost = detail.Cost,
                Remark = detail.Remark
            });
        }

        return viewModel.Id.HasValue
            ? await orderService.UpdateAsync(orderEditor)
            : await orderService.CreateAsync(orderEditor);
    }
}
