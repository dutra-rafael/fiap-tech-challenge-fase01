using ContatosRegionais.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContatosRegionais.Dominio.Interfaces
{
    public interface ITelefoneRepositorio: IRepositorio<Telefone>
    {
        Task<IEnumerable<Telefone>> BuscaPorTelefoneAsync(Telefone telefone);
    }
}
