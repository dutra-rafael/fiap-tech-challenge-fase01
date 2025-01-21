using ContatosRegionais.Aplicacao.Modelos;
using ContatosRegionais.Dominio.Entidades;
using ContatosRegionais.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContatosRegionais.Aplicacao.Interfaces
{
    public interface IContatoServico
    {
        Task<IEnumerable<ContatoModelo>> BuscarTodosAsync();
        Task<ContatoModelo> BuscarPorIdAsync(int id);
        Task<IEnumerable<ContatoModelo>> BuscarPorNomeAsync(string nome);
        Task<ContatoModelo> BuscarPorEmailAsync(string email);
        Task<IEnumerable<ContatoModelo>> BuscarPorTelefoneAsync(int codigo, int numero);
        Task<IEnumerable<ContatoModelo>> BuscarPorCodigoAsync(int codigo);
        Task<ContatoModelo> RegistrarAsync(ContatoNovoModelo modelo);
        Task<ContatoModelo> AtualizarAsync(ContatoAlteradoModelo modelo);
        Task<ContatoModelo> RemoverTelefoneAsync(int contatoId, int codigo, int numero);
        Task<bool> RemoverAsync(int id);

    }
}
