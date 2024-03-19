namespace CloudyWing.OrderingSystem.Web.Model {
    [Serializable]
    public class FormResult {
        public FormResultLevel Level { get; set; }

        public string? Message { get; set; }
    }
}
