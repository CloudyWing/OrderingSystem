using CloudyWing.OrderingSystem.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudyWing.OrderingSystem.DataAccess.Config {
    public class UserConfiguration : IEntityTypeConfiguration<User> {
        public void Configure(EntityTypeBuilder<User> builder) {
            string tableName = "Users";

            builder.HasKey(b => b.Email)
                .HasName($"PK_{tableName}");

            builder.Property(b => b.Email)
                .IsRequired()
                .HasColumnOrder(0)
                .IsUnicode(false)
                .HasMaxLength(255)
                .HasComment("電子信箱");

            builder.Property(b => b.Password)
                .IsRequired()
                .HasColumnOrder(1)
                .IsUnicode(false)
                .HasMaxLength(255)
                .HasComment("密碼");

            builder.Property(b => b.Name)
                .IsRequired()
                .HasColumnOrder(2)
                .IsUnicode()
                .HasMaxLength(maxLength: 50)
                .HasComment("姓名");

            builder.Property(b => b.Role)
                .IsRequired()
                .HasColumnOrder(3)
                .HasComment("角色")
                .HasConversion(
                    v => (short)v,
                    v => (Role)v
                );

            builder.ToTable(tableName, t => t.HasComment("使用者資料檔"));
        }
    }
}
