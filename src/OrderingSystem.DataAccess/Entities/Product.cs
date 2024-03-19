namespace CloudyWing.OrderingSystem.DataAccess.Entities {
    public class Product {
        public Guid Id { get; set; }

        public int DisplayOrder { get; set; }

        public string? Name { get; set; }

        public int Price { get; set; }

        public Guid CategoryId { get; set; }

        public ProductCategory? Category { get; set; }

        public IList<OrderDetail> OrderDetails { get; } = [];
    }
}
