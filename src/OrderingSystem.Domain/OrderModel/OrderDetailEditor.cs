namespace CloudyWing.OrderingSystem.Domain.Services.OrderModel {
    public class OrderDetailEditor {

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public int Cost { get; set; }

        public string? Remark { get; set; }
    }
}
