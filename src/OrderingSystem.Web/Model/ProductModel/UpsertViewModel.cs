using System.ComponentModel.DataAnnotations;

namespace CloudyWing.OrderingSystem.Web.Model.ProductModel {
    public class UpsertViewModel {
        public bool IsExisting => Id.HasValue;

        public Guid? Id { get; set; }

        [Display(Name = "商品分類")]
        [Required]
        public string? Name { get; set; }

        [Display(Name = "單價")]
        [Required]
        public int Price { get; set; }

        [Display(Name = "分類")]
        [Required]
        public Guid CategoryId { get; set; }
    }
}
