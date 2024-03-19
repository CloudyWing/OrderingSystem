using CloudyWing.OrderingSystem.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudyWing.OrderingSystem.DataAccess.Config {
    public class ProductConfiguration : IEntityTypeConfiguration<Product> {
        public void Configure(EntityTypeBuilder<Product> builder) {
            string tableName = "Products";

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

            builder.Property(e => e.Price)
                .IsRequired()
                .HasColumnOrder(2)
                .HasComment("價格");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode()
                .HasColumnOrder(3)
                .HasComment("名稱");

            builder.HasOne(e => e.Category)
                .WithMany(e => e.Products)
                .HasForeignKey(e => e.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName($"FK_{tableName}_CategoryId");

            builder.ToTable(tableName, t => t.HasComment("商品資料檔"));
        }
    }
}
