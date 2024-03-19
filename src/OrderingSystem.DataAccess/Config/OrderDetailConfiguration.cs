using CloudyWing.OrderingSystem.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudyWing.OrderingSystem.DataAccess.Config {
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail> {
        public void Configure(EntityTypeBuilder<OrderDetail> builder) {
            string tableName = "OrderDetails";

            builder.HasKey(e => e.Id)
                .HasName($"PK_{tableName}");

            builder.Property(e => e.Id)
                .IsRequired()
                .HasColumnOrder(0)
                .HasComment("編號");

            builder.Property(e => e.OrderId)
                .IsRequired()
                .HasColumnOrder(1)
                .HasComment("訂單編號");

            builder.Property(e => e.ProductId)
                .IsRequired()
                .HasColumnOrder(2)
                .HasComment("商品編號");

            builder.Property(e => e.Quantity)
                .IsRequired()
                .HasColumnOrder(3)
                .HasComment("數量");

            builder.Property(e => e.Cost)
                .IsRequired()
                .HasColumnOrder(4)
                .HasComment("總價");

            builder.Property(e => e.Remark)
                .IsRequired(false)
                .HasMaxLength(255)
                .IsUnicode()
                .HasColumnOrder(5)
                .HasComment("名稱");

            builder.HasOne(e => e.Order)
                .WithMany(e => e.OrderDetails)
                .HasForeignKey(e => e.OrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName($"FK_{tableName}_OrderId");

            builder.HasOne(e => e.Product)
                .WithMany(e => e.OrderDetails)
                .HasForeignKey(e => e.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName($"FK_{tableName}_ProductId");

            builder.ToTable(tableName, t => t.HasComment("訂單明細檔"));
        }
    }
}
