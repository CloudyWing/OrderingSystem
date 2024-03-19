namespace CloudyWing.OrderingSystem.Domain.Services {
    public interface IValueWatcher<out T> {
        bool HasValue { get; }

        T Value { get; }
    }
}
