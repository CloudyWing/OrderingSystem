namespace CloudyWing.OrderingSystem.DataAccess.Entities {
    public class ProductCategory {
        public Guid Id { get; set; }

        public int DisplayOrder { get; set; }

        public string? Name { get; set; }

        public IList<Product> Products { get; } = [];
    }
}
