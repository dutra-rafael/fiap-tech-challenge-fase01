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
    public class ContatoRepositorio: Repositorio<Contato>, IContatoRepositorio
    {
        private readonly new Contexto _contexto;

        public ContatoRepositorio(Contexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }

        public override async Task<IEnumerable<Contato>?> BuscaTodosAsync() 
        {
            try
            {
                IQueryable<Contato> _query = _contexto.Contatos;
                _query = _query.Include(t => t.Telefones);
                _query = _query.AsNoTracking();

                return await _query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao tentar recuperar informações em BuscaTodosAsync. {ex.Message}");
            }
        }

        public override async Task<Contato?> BuscaPorIdAsync(int id)
        {
            try
            {
                IQueryable<Contato> _query = _contexto.Contatos;
                _query = _query.Include(t => t.Telefones);
                _query = _query.Where(c => c.Id.Equals(id));
                _query = _query.AsNoTracking();

                return await _query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao tentar recuperar informações em BuscaPorIdAsync. {ex.Message}");
            }
        }

        public async Task<Contato?> BuscaPorEmailAsync(string email)
        {
            try
            {
                IQueryable<Contato> _query = _contexto.Contatos;
                _query = _query.Include(t => t.Telefones);
                _query = _query.Where(c => email.Contains(c.Email.ToUpper().Trim()));
                _query = _query.AsNoTracking();

                return await _query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao tentar recuperar informações em BuscaPorEmailAsync. {ex.Message}");
            }
        }

        public async Task<IEnumerable<Contato>> BuscaPorNomeAsync(string nome)
        {
            try
            {
                IQueryable<Contato> _query = _contexto.Contatos;
                _query = _query.Include(t => t.Telefones);
                _query = _query.Where(c => c.Nome.ToUpper().Contains(nome.ToUpper()));
                _query = _query.AsNoTracking();

                return await _query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao tentar recuperar informações em BuscaPorNomeAsync. {ex.Message}");
            }
        }

        public async Task<IEnumerable<Contato>> BuscaPorCodigoAsync(int codigo)
        {
            try
            {
                IQueryable<Contato> _query = _contexto.Contatos;
                _query = _query.Include(t => t.Telefones);
                _query = _query.Where(c => c.Telefones.Any(x => x.Codigo == codigo));
                _query = _query.AsNoTracking();

                return await _query.ToListAsync();
            }
            catch (Exception ex)
            {

                throw new Exception($"Falha ao tentar recuperar informações em BuscaPorCodigoAsync. {ex.Message}");
            }
        }

    }
}
