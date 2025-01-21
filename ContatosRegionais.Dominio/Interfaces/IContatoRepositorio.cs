using ContatosRegionais.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContatosRegionais.Dominio.Interfaces
{
    public interface IContatoRepositorio: IRepositorio<Contato>
    {
        Task<Contato?> BuscaPorEmailAsync(string email);
        Task<IEnumerable<Contato>> BuscaPorNomeAsync(string nome);
        Task<IEnumerable<Contato>> BuscaPorCodigoAsync(int codigo);
    }
}
