using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Map
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Criado).HasColumnName("Criado").HasColumnType("datetime");
            builder.Property(x => x.Alterado).HasColumnName("Alterado").HasColumnType("datetime");
            builder.Property(x => x.UserName).HasMaxLength(250).IsRequired();
            builder.Property(x => x.Password).HasMaxLength(512).IsRequired();
            builder.Property(x => x.GuidI).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Cpf).HasMaxLength(11).IsRequired();
            builder.Property(x => x.Role).HasMaxLength(30).IsRequired();
        }
    }
}
