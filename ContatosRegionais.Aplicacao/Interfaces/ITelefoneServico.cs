using ContatosRegionais.Aplicacao.Modelos;
using ContatosRegionais.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContatosRegionais.Aplicacao.Interfaces
{
    public interface ITelefoneServico
    {
        Task<IEnumerable<Telefone>> BuscarPorTelefoneAsync(Telefone telefone);
        Task<bool> RemoverAsync(int id);
    }
}
