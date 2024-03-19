namespace CloudyWing.OrderingSystem.DataAccess.Entities {
    public class Order {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public string? OrderUserEmail { get; set; }

        public IList<OrderDetail> OrderDetails { get; set; } = [];
    }
}
