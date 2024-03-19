namespace CloudyWing.OrderingSystem.Domain.Services.OrderModel {
    public class OrderEditor : BasicEditor {
        public OrderEditor() { }

        public OrderEditor(Guid id) : base(id) { }

        public ValueWatcher<DateTime> Date { get; set; }

        public ValueWatcher<string?> OrderUserEmail { get; set; }

        public IList<OrderDetailEditor> OrderDetailEditors { get; } = new List<OrderDetailEditor>();
    }
}
