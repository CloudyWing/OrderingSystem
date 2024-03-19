using CloudyWing.OrderingSystem.DataAccess.Entities;
using CloudyWing.OrderingSystem.Domain.Services.OrderModel;
using CloudyWing.OrderingSystem.Domain.Services.ProductModel;
using CloudyWing.OrderingSystem.Infrastructure.Util;
using CloudyWing.OrderingSystem.Web.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CloudyWing.OrderingSystem.Web.Model.OrderModel {
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
                x => x.OrderBy(y => y.Date),
                x => x.Include(y => y.OrderDetails)
            );
        }

        public async Task<IReadOnlyList<DetailListItemViewModel>> GetDetailsAsync(Guid orderId) {
            return await orderDetailService.GetListAsync(
                x => new DetailListItemViewModel {
                    ProductName = x.Product!.Name,
                    Quantity = x.Quantity,
                    Cost = x.Cost,
                    Remark = x.Remark
                },
                x => x.OrderId == orderId,
                x => x.OrderBy(y => y.Product!.DisplayOrder),
                x => x.Include(y => y.Product!)
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
            Order? order = await orderService.GetSingleOrDefaultAsync(x => x.Id == id);
            ExceptionUtils.ThrowIfItemNotFound(order);

            return order!.Date;
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
                Data = await productCategoryService.GetListAsync()
            };
        }

        public async Task<ResponseResult> GetProductsAsync() {
            return new ResponseResult<IReadOnlyList<Product>> {
                Data = await productService.GetListAsync()
            };
        }

        public async Task<bool> UpsertAsync(UpsertViewModel viewModel) {
            OrderEditor orderEditor = viewModel.IsExisting ? new OrderEditor(viewModel.Id!.Value) : new OrderEditor();
            orderEditor.Date = viewModel.Date;
            orderEditor.OrderUserEmail = HttpContextAccessor.HttpContext!.User.Identity!.GetEmail();

            foreach (UpsertDetailViewModel detail in viewModel.Details) {
                orderEditor.OrderDetailEditors.Add(new OrderDetailEditor {
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    Cost = detail.Cost,
                    Remark = detail.Remark
                });
            }

            return viewModel.IsExisting
                ? await orderService.UpdateAsync(orderEditor)
                : await orderService.CreateAsync(orderEditor);
        }
    }
}
