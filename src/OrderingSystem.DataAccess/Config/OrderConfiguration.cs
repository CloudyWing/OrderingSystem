using CloudyWing.OrderingSystem.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudyWing.OrderingSystem.DataAccess.Config {
    public class OrderConfiguration : IEntityTypeConfiguration<Order> {
        public void Configure(EntityTypeBuilder<Order> builder) {
            string tableName = "Orders";

            builder.HasKey(e => e.Id)
                .HasName($"PK_{tableName}");

            builder.Property(e => e.Id)
                .IsRequired()
                .HasColumnOrder(0)
                .HasComment("編號");

            builder.Property(e => e.Date)
                .IsRequired()
                .HasColumnOrder(1)
                .HasComment("交易日期");

            builder.Property(e => e.OrderUserEmail)
                .IsRequired()
                .HasColumnOrder(2)
                .HasComment("購買者信箱");

            builder.ToTable(tableName, t => t.HasComment("訂單資料檔"));
        }
    }
}
