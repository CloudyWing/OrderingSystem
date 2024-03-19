using System.ComponentModel.DataAnnotations;

namespace CloudyWing.OrderingSystem.Web.Model.OrderModel {
    public class UpsertViewModel : IValidatableObject {
        public bool IsExisting => Id.HasValue;

        public Guid? Id { get; set; }

        [Display(Name = "訂單日期")]
        [Required]
        public DateTime Date { get; set; }

        public IEnumerable<UpsertDetailViewModel> Details { get; set; } = new List<UpsertDetailViewModel>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            if (Date > DateTime.Today.AddDays(7)) {
                yield return new ValidationResult("訂單日期只能7日內。", [nameof(Date)]);
            }

            if (!Details.Any()) {
                yield return new ValidationResult("至少訂購一筆資料。", [nameof(Date)]);
            }
        }
    }
}
