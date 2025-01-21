using ContatosRegionais.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContatosRegionais.Persistencia.Configuracoes
{
    internal class TelefoneConfiguracao : IEntityTypeConfiguration<Telefone>
    {
        public void Configure(EntityTypeBuilder<Telefone> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
            builder.HasIndex(t => new { t.ContatoId, t.Codigo, t.Numero }).IsUnique();
            builder.Property(t => t.Codigo).HasColumnType("int").IsRequired();
            builder.Property(t => t.Numero).HasColumnType("int").IsRequired();
            builder.Property(t => t.ContatoId).HasColumnType("int").IsRequired();
            builder.HasOne(t => t.Contato).WithMany(c => c.Telefones).HasForeignKey(t => t.ContatoId);
        }
    }
}
