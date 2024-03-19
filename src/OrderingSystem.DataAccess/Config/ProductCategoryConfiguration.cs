using CloudyWing.OrderingSystem.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudyWing.OrderingSystem.DataAccess.Config {
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory> {
        public void Configure(EntityTypeBuilder<ProductCategory> builder) {
            string tableName = "ProductCategories";

            builder.HasKey(e => e.Id)
                .HasName($"PK_{tableName}");

            builder.Property(e => e.Id)
                .IsRequired()
                .HasColumnOrder(0)
                .HasComment("編號");

            builder.Property(e => e.DisplayOrder)
                .IsRequired()
                .HasColumnOrder(1)
                .HasComment("顯示順序");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode()
                .HasColumnOrder(2)
                .HasComment("名稱");

            builder.ToTable(tableName, t => t.HasComment("商品分類檔"));
        }
    }
}
