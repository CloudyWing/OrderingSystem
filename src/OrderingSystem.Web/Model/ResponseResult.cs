namespace CloudyWing.OrderingSystem.Web.Model {
    public class ResponseResult {
        public bool IsOk { get; set; }

        public string? Message { get; set; }
    }

    public class ResponseResult<T> : ResponseResult {
        public T? Data { get; set; }
    }
}
