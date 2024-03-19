namespace CloudyWing.OrderingSystem.DataAccess.Entities {
    public class OrderDetail {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public int Cost { get; set; }

        public string? Remark { get; set; }

        public Order? Order { get; set; }

        public Product? Product { get; set; }
    }
}
