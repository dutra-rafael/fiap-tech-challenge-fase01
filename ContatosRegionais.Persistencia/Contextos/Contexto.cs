using ContatosRegionais.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ContatosRegionais.Persistencia.Contextos
{
    public class Contexto: DbContext
    {
        public Contexto(DbContextOptions<Contexto> option) : base(option) { }

        public DbSet<Contato> Contatos { get; set; }
        public DbSet<Telefone> Telefones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Contexto).Assembly);
        }
    }
}
