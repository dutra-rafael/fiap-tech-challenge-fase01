using ContatosRegionais.Dominio.Entidades;
using ContatosRegionais.Dominio.Interfaces;
using ContatosRegionais.Persistencia.Contextos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ContatosRegionais.Persistencia.Repositorios
{
    public class Repositorio<T>: IRepositorio<T> where T : EntidadeBase
    {
        protected Contexto _contexto;
        protected DbSet<T> _dbSet;
        public Repositorio(Contexto contexto)
        {
            _contexto = contexto;
            _dbSet = _contexto.Set<T>();
        }


        public virtual async Task<IEnumerable<T>> BuscaTodosAsync()
        {
            try
            {
                IEnumerable<T> _entidades = await _contexto.Set<T>().AsNoTracking().ToListAsync();
                //await _contexto.DisposeAsync();

                return _entidades;
            }
            catch (Exception ex)
            {
                string _error = ex.InnerException.Message ?? ex.Message;
                throw new Exception($"Falha ao tentar recuperar informações em BuscarTodosAsync. {_error}");
            }
        } 

        public virtual async Task<T> BuscaPorIdAsync(int id)
        {
            try
            {
                T entidade = await _contexto.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                //await _contexto.DisposeAsync();

                return entidade;
            }
            catch (Exception ex)
            {

                string _error = ex.InnerException.Message ?? ex.Message;
                throw new Exception($"Falha ao tentar recuperar informações em BuscaPorIdAsync. {_error}");
            }
        }

        public virtual async Task<T> AlteraAsync(T entidade)
        {
            try
            {
                entidade.DataAlteracao = DateTime.Now;

                _contexto.Set<T>().Update(entidade);
                await _contexto.SaveChangesAsync();
                await _contexto.DisposeAsync();

                return entidade;
            }
            catch (Exception ex)
            {
                string _error = ex.InnerException.Message ?? ex.Message;
                throw new Exception(message: $"Falha ao tentar alterar o registro em AlterarAsync. {_error}");
            }
        }

        public virtual async Task<T> SalvaAsync(T entidade)
        {
            try
            {
                entidade.DataCriacao = DateTime.Now;

                await _contexto.Set<T>().AddAsync(entidade);
                await _contexto.SaveChangesAsync();
                await _contexto.DisposeAsync();

                return entidade;
            }
            catch (Exception ex)
            {
                string _error = ex.InnerException.Message ?? ex.Message;
                throw new Exception(message: $"Falha ao tentar gravar o registro em SalvaAsync. {_error}");
            }
        }


        public virtual async Task<bool> DeletaAsync(T entidade)
        {
            try
            {
                _contexto.Set<T>().Remove(entidade);
                var _isModified = await _contexto.SaveChangesAsync();
                //await _contexto.DisposeAsync();

                return _isModified >= 1;
            }
            catch (Exception ex)
            {
                string _error = ex.InnerException.Message ?? ex.Message;
                throw new Exception(message: $"Falha ao tentar apagar o registro em DeletaAsync. {_error}");
            }
        }
    }
}