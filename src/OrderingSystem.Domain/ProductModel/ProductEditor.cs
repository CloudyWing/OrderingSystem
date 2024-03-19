namespace CloudyWing.OrderingSystem.Domain.Services.ProductModel {
    public class ProductEditor : BasicEditor {
        public ProductEditor() {
        }

        public ProductEditor(Guid id) : base(id) {
        }

        public ValueWatcher<string>? Name { get; set; }

        public ValueWatcher<int> Price { get; set; }

        public ValueWatcher<Guid> CategoryId { get; set; }
    }
}
