namespace CloudyWing.OrderingSystem.Web.Model.OrderModel {
    public class UpsertDetailViewModel {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public int Cost { get; set; }

        public string? Remark { get; set; }
    }
}
