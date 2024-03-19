namespace CloudyWing.OrderingSystem.Domain.Services {
    public abstract class BasicEditor {
        protected BasicEditor() {
        }

        protected BasicEditor(Guid id) {
            Id = id;
        }

        public Guid? Id { get; private set; }

        internal void SetId(Guid id) {
            Id = id;
        }
    }
}
