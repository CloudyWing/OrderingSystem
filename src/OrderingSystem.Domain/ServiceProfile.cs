using AutoMapper;
using CloudyWing.OrderingSystem.DataAccess.Entities;
using CloudyWing.OrderingSystem.Domain.Services.OrderModel;
using CloudyWing.OrderingSystem.Domain.Services.ProductModel;
using CloudyWing.OrderingSystem.Domain.Services.UserModel;
using CloudyWing.OrderingSystem.Domain.Util;

namespace CloudyWing.OrderingSystem.Domain.Services {
    internal class ServiceProfile : Profile {
        public ServiceProfile() {
            CreateMap<ValueWatcher<IConvertible>, IConvertible>()
                 .ConvertUsing<ValueWatcherConverter<IConvertible>>();

            CreateMap<string?, string?>()
                .ConvertUsing(x => x == null ? x : x.Trim());

            CreateMap<ValueWatcher<string?>, string?>()
                .ConvertUsing<OptionalStringConverter>();

            CreateMap<ValueWatcher<byte[]>, byte[]>()
                .ConvertUsing<ValueWatcherConverter<byte[]>>();

            CreateMap<ValueWatcher<Guid>, Guid>()
                .ConvertUsing<ValueWatcherConverter<Guid>>();

            CreateMap<ValueWatcher<Guid?>, Guid?>()
                .ConvertUsing<ValueWatcherConverter<Guid?>>();

            CreateMap<ValueWatcher<bool?>, bool?>()
                .ConvertUsing<ValueWatcherConverter<bool?>>();

            CreateMap<ValueWatcher<byte?>, byte?>()
                .ConvertUsing<ValueWatcherConverter<byte?>>();

            CreateMap<ValueWatcher<short?>, short?>()
                .ConvertUsing<ValueWatcherConverter<short?>>();

            CreateMap<ValueWatcher<int?>, int?>()
                .ConvertUsing<ValueWatcherConverter<int?>>();

            CreateMap<ValueWatcher<long?>, long?>()
                .ConvertUsing<ValueWatcherConverter<long?>>();

            CreateMap<ValueWatcher<decimal?>, decimal?>()
                .ConvertUsing<ValueWatcherConverter<decimal?>>();

            CreateMap<ValueWatcher<float?>, float?>()
                .ConvertUsing<ValueWatcherConverter<float?>>();

            CreateMap<ValueWatcher<DateTime?>, DateTime?>()
                .ConvertUsing<ValueWatcherConverter<DateTime?>>();

            CreateUserMap();
            CreateProductMap();
            CreateOrderMap();
        }

        private void CreateUserMap() {
            CreateMap<UserEditor, User>()
                .ForMember(desc => desc.Password, opt => opt.MapFrom(src => PasswordUtil.ComputeHash(src.Password)));
        }

        private void CreateProductMap() {
            CreateMap<ProductEditor, Product>()
                .ForMember(desc => desc.Id, opt => {
                    opt.PreCondition((src, desc, context) => desc.Id == Guid.Empty);
                    opt.MapFrom(src => Guid.NewGuid());
                })
                .ForMember(desc => desc.DisplayOrder, opt => opt.Ignore())
                .ForMember(desc => desc.Category, opt => opt.Ignore())
                .ForMember(desc => desc.OrderDetails, opt => opt.Ignore());

            CreateMap<ProductCategoryEditor, ProductCategory>()
                .ForMember(desc => desc.Id, opt => {
                    opt.PreCondition((src, desc, context) => desc.Id == Guid.Empty);
                    opt.MapFrom(src => Guid.NewGuid());
                })
                .ForMember(desc => desc.DisplayOrder, opt => opt.Ignore())
                .ForMember(desc => desc.Products, opt => opt.Ignore());
        }

        private void CreateOrderMap() {
            CreateMap<OrderEditor, Order>()
                .ForMember(desc => desc.Id, opt => {
                    opt.PreCondition((src, desc, context) => desc.Id == Guid.Empty);
                    opt.MapFrom(src => Guid.NewGuid());
                })
                .ForMember(desc => desc.OrderDetails, opt => opt.Ignore());
        }

        private class ValueWatcherConverter<T> : ITypeConverter<ValueWatcher<T>, T> {
            public T Convert(ValueWatcher<T> source, T destination, ResolutionContext context) {
                if (source.HasValue) {
                    destination = source.Value;
                }
                return destination;
            }
        }

        private class OptionalStringConverter : ITypeConverter<ValueWatcher<string?>, string?> {
            public string? Convert(ValueWatcher<string?> source, string? destination, ResolutionContext context) {
                if (source.HasValue) {
                    destination = source.Value?.Trim();
                }
                return destination;
            }
        }
    }
}
