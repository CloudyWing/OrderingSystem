using System.Collections;

namespace CloudyWing.OrderingSystem.Domain.Services {
    public sealed class PagedList<T> : PagingMetadata, IEnumerable<T>, IEnumerable {
        public PagedList(IEnumerable<T> items, PagingMetadata metadata)
            : this(items, metadata.PageNumber, metadata.PageSize, metadata.TotalItemCount) { }

        private readonly IList<T> items;

        public PagedList(IEnumerable<T> items, int pageNumber, int pageSize, int totalItemCount)
            : base(pageNumber, pageSize, totalItemCount
        ) {
            this.items = items is IList<T> ? (items as IList<T> ?? new List<T>()) : items.ToList();
        }

        public T this[int index] => items[index];

        /// <summary>
        /// Gets the page count.
        /// </summary>
        public int Count => items.Count;

        public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();
    }
}
