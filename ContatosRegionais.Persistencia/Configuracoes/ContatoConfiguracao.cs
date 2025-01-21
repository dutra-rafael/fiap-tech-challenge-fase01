using ContatosRegionais.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContatosRegionais.Persistencia.Configuracoes
{
    internal class ContatoConfiguracao : IEntityTypeConfiguration<Contato>
    {
        public void Configure(EntityTypeBuilder<Contato> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnType("int").ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
            builder.Property(c => c.Nome).HasColumnType("varchar(40)").IsRequired();
            builder.HasIndex(c => c.Email).IsUnique();
            builder.Property(c => c.Email).HasColumnType("varchar(80)").IsRequired();
            builder.HasMany(t => t.Telefones).WithOne(c => c.Contato).HasForeignKey(c => c.ContatoId);
        }
    }
}
