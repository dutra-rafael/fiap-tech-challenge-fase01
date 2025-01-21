using ContatosRegionais.Dominio.Entidades;
using ContatosRegionais.Dominio.Interfaces;
using ContatosRegionais.Persistencia.Contextos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContatosRegionais.Persistencia.Repositorios
{
    public class TelefoneRepositorio: Repositorio<Telefone>, ITelefoneRepositorio
    {
        private readonly new Contexto _contexto;

        public TelefoneRepositorio(Contexto contexto):base(contexto)
        {
            _contexto = contexto;            
        }

        public async Task<IEnumerable<Telefone>> BuscaPorTelefoneAsync(Telefone telefone)
        {
            try
            {
                IQueryable<Telefone> _query = _contexto.Telefones;
                _query = _query.Include(c => c.Contato);
                _query = _query.Where(t => t.Codigo == telefone.Codigo && t.Numero == telefone.Numero);

                if (telefone.ContatoId > 0)
                    _query = _query.Where(t => t.ContatoId == telefone.ContatoId);

                _query = _query.AsNoTracking();

                return await _query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao tentar recuperar informações em BuscarPorTelefoneAsync. {ex.Message}");
            }
        }

    }
}
