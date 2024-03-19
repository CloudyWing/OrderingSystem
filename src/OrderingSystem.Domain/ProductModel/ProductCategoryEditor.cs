namespace CloudyWing.OrderingSystem.Domain.Services.ProductModel {
    public class ProductCategoryEditor : BasicEditor {
        public ProductCategoryEditor() {
        }

        public ProductCategoryEditor(Guid id) : base(id) {
        }

        public ValueWatcher<string?>? Name { get; set; }
    }
}
