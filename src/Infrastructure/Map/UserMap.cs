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

            builder.OwnsOne(x => x.Pessoa, Pessoa =>
            {
                Pessoa.Property(x => x.Celular).HasColumnName("Celular").HasMaxLength(11);

                Pessoa.Property(x => x.Foto).HasColumnName("Foto");
           });

            builder.OwnsOne(x => x.Endereco, endereco =>
            {
                endereco.Property(x => x.logradouro).HasColumnName("Logradouro").HasMaxLength(50);

                endereco.Property(x => x.complemento).HasColumnName("Complemento").HasMaxLength(20);

                endereco.Property(x => x.numero).HasColumnName("Numero").HasMaxLength(5);

                endereco.Property(x => x.cep).HasColumnName("Cep").HasMaxLength(8);

                endereco.Property(x => x.bairro).HasColumnName("Bairro").HasMaxLength(30);

                endereco.Property(x => x.cidade).HasColumnName("Cidade").HasMaxLength(30);

                endereco.Property(x => x.estado).HasColumnName("Estado").HasMaxLength(2);

                endereco.Property(x => x.municipio).HasColumnName("Municipio").HasMaxLength(30);
            });
        }
    }
}
