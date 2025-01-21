using ContatosRegionais.Aplicacao.Interfaces;
using ContatosRegionais.Aplicacao.Servicos;
using ContatosRegionais.Dominio.Interfaces;
using ContatosRegionais.Persistencia.Repositorios;
using Microsoft.Extensions.DependencyInjection;

namespace ContatosRegionais.InjecaoDependencia
{
    public static class InversaoDeControle
    {
        public static void ServicosRegistrados(IServiceCollection services)
        {
            services.AddTransient<ICacheServico, CacheServico>();
            services.AddScoped<IContatoServico, ContatoServico>();
            services.AddScoped<ITelefoneServico, TelefoneServico>();
        }

        public static void RepositoriosRegistrados(IServiceCollection services)
        {
            services.AddScoped<IContatoRepositorio, ContatoRepositorio>();
            services.AddScoped<ITelefoneRepositorio, TelefoneRepositorio>();
        }
    }
}
