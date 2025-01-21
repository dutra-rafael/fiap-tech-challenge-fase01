using ContatosRegionais.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContatosRegionais.Dominio.Interfaces
{
    public interface IRepositorio<T> where T : EntidadeBase
    {
        Task<IEnumerable<T>> BuscaTodosAsync();
        Task<T> BuscaPorIdAsync(int id);
        Task<T> AlteraAsync(T entidade);
        Task<T> SalvaAsync(T entidade);
        Task<bool> DeletaAsync(T entidade);
    }
}
