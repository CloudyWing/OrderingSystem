using CloudyWing.OrderingSystem.Infrastructure.Util;

namespace CloudyWing.OrderingSystem.Domain.Services {
    [Serializable]
    public struct ValueWatcher<T> : IValueWatcher<T> {
        private readonly T value;

        public ValueWatcher(T value) {
            this.value = value;
            HasValue = true;
        }

        public bool HasValue { get; }

        public T Value {
            get {
                if (!HasValue) {
                    ExceptionUtils.ThrowItemNotFound();
                }
                return value;
            }
        }

        public static ValueWatcher<T> Empty { get; } = new ValueWatcher<T>();

        public T GetValueOrDefault() {
            return value;
        }

        public override int GetHashCode() {
            return HasValue && value != null ? value.GetHashCode() : base.GetHashCode();
        }

        public T GetValueOrDefault(T defaultValue) {
            return HasValue ? value : defaultValue;
        }

        public override bool Equals(object? other) {
            if (!HasValue) {
                return other == null;
            }

            if (other == null) {
                return false;
            }

            return other.Equals(value);
        }

        public override string ToString() {
            return HasValue ? (value?.ToString() ?? "") : "";
        }

        public static implicit operator ValueWatcher<T>(T value) {
            return new ValueWatcher<T>(value);
        }

        public static explicit operator T(ValueWatcher<T> value) {
            return value.Value;
        }

        public static bool operator ==(ValueWatcher<T> left, ValueWatcher<T> right) {
            return left.Equals(right);
        }

        public static bool operator !=(ValueWatcher<T> left, ValueWatcher<T> right) {
            return !(left == right);
        }

        public static bool operator ==(ValueWatcher<T> left, T right) {
            return (left.Value == null && right == null) || (left.Value != null && left.Value.Equals(right));
        }

        public static bool operator !=(ValueWatcher<T> left, T right) {
            return !(left == right);
        }

        public static bool operator ==(T left, ValueWatcher<T> right) {
            return (left == null && right.Value == null) || (right.Value != null && right.Value.Equals(left));
        }

        public static bool operator !=(T left, ValueWatcher<T> right) {
            return !(left == right);
        }
    }
}
