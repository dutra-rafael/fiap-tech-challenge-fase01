using ContatosRegionais.Aplicacao.Interfaces;
using ContatosRegionais.Aplicacao.Modelos;
using ContatosRegionais.Dominio.Entidades;
using ContatosRegionais.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContatosRegionais.Aplicacao.Servicos
{
    public class TelefoneServico: ITelefoneServico
    {
        private readonly ITelefoneRepositorio _repositorio;
        public TelefoneServico(ITelefoneRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<IEnumerable<Telefone>> BuscarPorTelefoneAsync(Telefone telefone)
        {

            IEnumerable<Telefone> _telefones = await _repositorio.BuscaPorTelefoneAsync(telefone);
            return _telefones;
        }

        public async Task<bool> RemoverAsync(int id)
        {
            Telefone _telefone = await _repositorio.BuscaPorIdAsync(id);
            if (_telefone == null)
                return false;

            if (!await _repositorio.DeletaAsync(_telefone))
                return false;

            return true;
        }
    }
}
