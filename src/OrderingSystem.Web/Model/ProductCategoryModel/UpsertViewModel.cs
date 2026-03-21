using System.ComponentModel.DataAnnotations;

namespace CloudyWing.OrderingSystem.Web.Model.ProductCategoryModel;

public class UpsertViewModel {
    public bool IsExisting => Id.HasValue;

    public Guid? Id { get; set; }

    [Display(Name = "商品分類")]
    [Required]
    public string? Name { get; set; }
}