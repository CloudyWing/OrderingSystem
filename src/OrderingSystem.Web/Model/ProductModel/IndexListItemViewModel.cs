namespace OrderingSystem.Domain.Services.ProductModel {
    public class IndexListItemViewModel {
        public Guid Id { get; set; }

        public int DisplayOrder { get; set; }

        public string? Name { get; set; }

        public int Price { get; set; }

        public string? CategoryName { get; set; }
    }
}
