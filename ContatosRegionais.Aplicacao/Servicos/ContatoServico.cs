using ContatosRegionais.Aplicacao.Interfaces;
using ContatosRegionais.Aplicacao.Modelos;
using ContatosRegionais.Dominio.Entidades;
using ContatosRegionais.Dominio.Interfaces;

namespace ContatosRegionais.Aplicacao.Servicos
{
    public class ContatoServico: IContatoServico
    {
        private readonly IContatoRepositorio _repositorio;
        private readonly ITelefoneServico _telefoneServico;
        public ContatoServico(IContatoRepositorio repositorio, ITelefoneServico telefoneServico)
        {
            _repositorio = repositorio;
            _telefoneServico = telefoneServico;
        }

        public async Task<IEnumerable<ContatoModelo>> BuscarTodosAsync()
        {
            IEnumerable<Contato> _contatos =  await _repositorio.BuscaTodosAsync();
            if (_contatos == null)
                return null;

            return MontaListaContatoModelo(_contatos);
        }

        public async Task<ContatoModelo> BuscarPorIdAsync(int id) 
        {
            Contato _contato = await _repositorio.BuscaPorIdAsync(id);
            if (_contato == null)
                return null;

            return MontaContatoModelo(_contato);
        }

        public async Task<ContatoModelo> BuscarPorEmailAsync(string email)
        {
            Contato? _contato = await _repositorio.BuscaPorEmailAsync(email);
            if (_contato == null)
                return null;

            return MontaContatoModelo(_contato);
        }

        public async Task<IEnumerable<ContatoModelo>> BuscarPorNomeAsync(string nome) 
        {
            IEnumerable<Contato> _contatos = await _repositorio.BuscaPorNomeAsync(nome);
            if (_contatos.Count() == 0)
                return null;

            return MontaListaContatoModelo(_contatos);
        }

        public async Task<IEnumerable<ContatoModelo>> BuscarPorCodigoAsync(int codigo)
        {
            IEnumerable<Contato> _contatos = await _repositorio.BuscaPorCodigoAsync(codigo);
            if (_contatos.Count() == 0)
                return null;

            return MontaListaContatoModelo(_contatos);
        }

        public async Task<IEnumerable<ContatoModelo>> BuscarPorTelefoneAsync(int codigo, int numero)
        {
            List<Contato> _contatos = new List<Contato>();

            EntidadeValidacao.ValidaTamanhoTexto(codigo.ToString(), 2, 2, nameof(codigo));
            EntidadeValidacao.ValidaTamanhoTexto(numero.ToString(), 8, 9, nameof(numero));

            Telefone _telefone = new Telefone(codigo, numero);
            IEnumerable<Telefone> _telefones = await _telefoneServico.BuscarPorTelefoneAsync(_telefone);
            if (!_telefones.Any() || _telefone.Equals(null))
                return null;

            foreach (Telefone telefone in _telefones)
            {
                _contatos.Add(await _repositorio.BuscaPorIdAsync(telefone.ContatoId));
            }

            return MontaListaContatoModelo(_contatos);
        }

        public async Task<ContatoModelo> RegistrarAsync(ContatoNovoModelo modelo) 
        {
            List<Telefone> _telefones = new List<Telefone>();

            if(modelo.Telefones != null)
            {
                foreach (TelefoneModelo telefone in modelo.Telefones)
                {
                    Telefone _telefone = new Telefone(telefone.Codigo, telefone.Numero);
                    _telefones.Add(_telefone);
                }
            }

            var _telefonesFiltrados = _telefones.Distinct().ToList();
            
            Contato _contato = new Contato(modelo.Nome, modelo.Email, _telefonesFiltrados);

            await _repositorio.SalvaAsync(_contato);

            return MontaContatoModelo(_contato);
        }

        public async Task<ContatoModelo> AtualizarAsync(ContatoAlteradoModelo contato) 
        {
            List<Telefone> _telefones = new List<Telefone>();

            Contato _contato = await _repositorio.BuscaPorIdAsync(contato.Id);
            if (_contato == null)
                throw new Exception(message: $"Falha ao tentar atualizar o registro.");

            foreach (TelefoneModelo telefone in contato.Telefones)
            {
                Telefone _telefone = new Telefone(telefone.Codigo, telefone.Numero);
                if (!_contato.Telefones.Contains(_telefone))
                    _contato.Telefones.Add(_telefone);
            }
            _contato.Nome = contato.Nome;
            _contato.Email = contato.Email; 
            await _repositorio.AlteraAsync(_contato);

            return MontaContatoModelo(_contato);
        }

        public async Task<ContatoModelo> RemoverTelefoneAsync(int id, int codigo, int numero) 
        {
            Contato _contato = new Contato();

            EntidadeValidacao.ValidaTamanhoTexto(codigo.ToString(), 2, 2, nameof(codigo));
            EntidadeValidacao.ValidaTamanhoTexto(numero.ToString(), 8, 9, nameof(numero));

            Telefone _telefone = new Telefone(codigo, numero, id);

            IEnumerable<Telefone> _telefones = await _telefoneServico.BuscarPorTelefoneAsync(_telefone);
            if (!_telefones.Any())
                return null;
            
            var _response = _telefones.FirstOrDefault();

            if(!await _telefoneServico.RemoverAsync(_response.Id))
                throw new Exception(message: "Não foi possível remover o número informado.");

            _contato = await _repositorio.BuscaPorIdAsync(id);
            if(_contato.Telefones.Any())
                return MontaContatoModelo(_contato);

            if (!await RemoverAsync(_contato.Id))
                throw new Exception(message: "Um problema inesperado ocorreu durante a remoção do registro.");

            return new ContatoModelo();
        }

        public async Task<bool> RemoverAsync(int id) 
        {
            Contato _contato = await _repositorio.BuscaPorIdAsync(id);
            if (_contato == null)
                return false;

            if (!await _repositorio.DeletaAsync(_contato))
                return false;

            return true;    
        }

        private IEnumerable<ContatoModelo> MontaListaContatoModelo(IEnumerable<Contato> Contatos) 
        { 
            List<ContatoModelo> _modelos  = new List<ContatoModelo>();

            foreach(Contato contato in Contatos)
            {
                _modelos.Add(MontaContatoModelo(contato));
            }

            return _modelos.OrderBy(x => x.Id);
        }

        private ContatoModelo MontaContatoModelo(Contato contato)
        {
            ContatoModelo _modelo = new ContatoModelo()
            {
                Id = contato.Id,
                Nome = contato.Nome,
                Email = contato.Email,
                DataCriacao = contato.DataCriacao,
                DataAlteracao = contato.DataAlteracao,
                Telefones = new List<TelefoneModelo>()
            };

            foreach (var telefone in contato.Telefones)
            {
                TelefoneModelo _telefone = new TelefoneModelo();
                _telefone.Numero = telefone.Numero;
                _telefone.Codigo = telefone.Codigo;

                _modelo.Telefones.Add(_telefone);
            }

            return _modelo;
        }
    }
}
