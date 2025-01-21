using ContatosRegionais.Aplicacao.Interfaces;
using ContatosRegionais.Aplicacao.Modelos;
using ContatosRegionais.Aplicacao.Servicos;
using ContatosRegionais.Dominio.Entidades;
using ContatosRegionais.Dominio.Interfaces;
using ContatosRegionais.Dominio.Testes.Entidades;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using Xunit;

namespace ContatosRegionais.Aplicacao.Testes
{
    [Collection("Contato Fixture")]
    public class ContatoServicoTeste
    {
        private readonly ContatoServico _contatoServico;
        private readonly Mock<IContatoRepositorio> _mockContatoRepositorio;
        private readonly Mock<ITelefoneServico> _mockTelefoneServico;
        public ContatoServicoTeste()
        {
            _mockContatoRepositorio = new Mock<IContatoRepositorio>();
            _mockTelefoneServico = new Mock<ITelefoneServico>();
            _contatoServico = new ContatoServico(_mockContatoRepositorio.Object, _mockTelefoneServico.Object);
        }

        [Trait("Contato", "Validando consultas")]
        [Fact(DisplayName = "Deve retornar nulo quando não há contatos no repositório")]
        public async Task TestaBuscarTodos_Retorna_Falha()
        {

            // Arrange
            List<Contato> _contatos = null;
            _mockContatoRepositorio.Setup(r => r.BuscaTodosAsync()).ReturnsAsync((IEnumerable<Contato>) _contatos);

            // Act
            var resposta = await _contatoServico.BuscarTodosAsync();

            // Assert
            Assert.Null(resposta);
        }

        [Trait("Contato", "Validando consultas")]
        [Fact(DisplayName = "Deve retornar uma lista de contatos quando o repositório possui dados")]
        public async Task TestaBuscarTodos_Retorna_Sucesso()
        {
            // Arrange
            ContatoFixture _contatoFixture = new ContatoFixture();
            List<Contato> contatos = new List<Contato>();
            for (int j = 0; j < 10; j++)
            {
                contatos.Add(_contatoFixture.GeraContatoComVariosTelefones());
            }

            _mockContatoRepositorio.Setup(r => r.BuscaTodosAsync()).ReturnsAsync(contatos);

            // Act
            var resposta = await _contatoServico.BuscarTodosAsync();

            // Assert
            Assert.NotNull(resposta);
            Assert.NotEmpty(resposta);
        }

        [Trait("Contato", "Validando consultas")]
        [Fact(DisplayName = "Deve retornar nulo quando o id não é encontrado")]
        public async Task TestaBuscarPorId_Retorna_Falha()
        {

            // Arrange
            ContatoFixture _contatoFixture = new ContatoFixture();
            _mockContatoRepositorio.Setup(r => r.BuscaPorIdAsync(It.IsAny<int>())).ReturnsAsync((Contato)null);

            // Act
            var resposta = await _contatoServico.BuscarPorIdAsync(1002);

            // Assert
            Assert.Null(resposta);
        }

        [Trait("Contato", "Validando consultas")]
        [Fact(DisplayName = "Deve retornar um contato válido quando o ID é encontrado")]
        public async Task TestaBuscarPorId_Retorna_Sucesso()
        {
            // Arrange
            ContatoFixture _contatoFixture = new ContatoFixture();
            var _contato = _contatoFixture.GeraContatoComVariosTelefones();
            _contato.Id = 1;

            _mockContatoRepositorio.Setup(r => r.BuscaPorIdAsync(1)).ReturnsAsync(_contato);

            // Act
            var resposta = await _contatoServico.BuscarPorIdAsync(1);

            // Assert
            Assert.NotNull(resposta);
            Assert.Equal(_contato.Id, resposta.Id);
        }

        [Trait("Contato", "Validando consultas")]
        [Fact(DisplayName = "Deve retornar nulo quando o e-mail não é encontrado")]
        public async Task TestaBuscarPorEmail_Retorna_Falha()
        {
            // Arrange
            _mockContatoRepositorio.Setup(r => r.BuscaPorEmailAsync(It.IsAny<string>())).ReturnsAsync((Contato)null);

            // Act
            var resposta = await _contatoServico.BuscarPorEmailAsync("jose.ninguem@example.com");

            // Assert
            Assert.Null(resposta);
        }

        [Trait("Contato", "Validando consultas")]
        [Fact(DisplayName = "Deve retornar um contato válido quando o e-mail é encontrado")]
        public async Task TestaBuscarPorEmail_Retorna_Sucesso()
        {
            // Arrange
            ContatoFixture _contatoFixture = new ContatoFixture();
            var _contato = _contatoFixture.GeraContatoComUmTelefone();
            _mockContatoRepositorio.Setup(r => r.BuscaPorEmailAsync(_contato.Email)).ReturnsAsync(_contato);

            // Act
            var resposta = await _contatoServico.BuscarPorEmailAsync(_contato.Email);

            // Assert
            Assert.NotNull(resposta);
            Assert.Equal(_contato.Email, resposta.Email);
        }

        [Trait("Contato", "Validando consultas")]
        [Fact(DisplayName = "Deve retornar nulo quando o nome não é encontrado")]
        public async Task TestaBuscarPorNome_Retorna_Falha()
        {
            // Arrange
            ContatoFixture _contatoFixture = new ContatoFixture();
            List<Contato> _contatos = _contatoFixture.GeraContatosComVariosTelefones(10);

            _mockContatoRepositorio.Setup(r => r.BuscaPorNomeAsync(It.IsAny<string>())).ReturnsAsync((IEnumerable<Contato>) _contatos);

            // Act
            var resultado = await _contatoServico.BuscarPorNomeAsync("NOME INEXISTENTE");

            // Assert
            Assert.NotNull(resultado);
            _mockContatoRepositorio.Verify(r => r.BuscaPorNomeAsync("NOME INEXISTENTE"), Times.Once);
        }

        [Trait("Contato", "Validando consultas")]
        [Fact(DisplayName = "Deve retornar nulo quando o codigo ddd não é encontrado")]
        public async Task TestaBuscarPorCodigo_Retorna_Falha()
        {
            //Arrange
            ContatoFixture _contatoFixture = new ContatoFixture();
            List<Contato> _contatos = _contatoFixture.GeraContatosComVariosTelefones(1);

            _contatos.RemoveAll(x => x.Telefones.Any(x => x.Codigo == 35));
            _mockContatoRepositorio.Setup(r => r.BuscaPorCodigoAsync(It.IsAny<int>())).ReturnsAsync((IEnumerable<Contato>)_contatos);

            //Act
            var resultado = await _contatoServico.BuscarPorCodigoAsync(35);

            //Assert
            Assert.NotNull(resultado);
            _mockContatoRepositorio.Verify(r => r.BuscaPorCodigoAsync(35), Times.Once);
        }

        [Trait("Contato", "Operações de criação")]
        [Fact(DisplayName = "Deve criar um contato com sucesso")]
        public async Task TestaCriacaoContato_Sucesso()
        {
            // Arrange
            ContatoFixture _contatoFixture = new ContatoFixture();
            var novoContato = _contatoFixture.GeraContatoComUmTelefone();

            _mockContatoRepositorio
                .Setup(r => r.SalvaAsync(novoContato))
                .ReturnsAsync(novoContato);
            
            List<TelefoneModelo> _telefoneModelos = new List<TelefoneModelo>();
            _telefoneModelos = _contatoFixture.GerarTelefoneModelos(5);

            ContatoNovoModelo novoContatoModelo = new ContatoNovoModelo() { Nome = novoContato.Nome, Email = novoContato.Email, Telefones = _telefoneModelos };

            // Act
            var resultado = await _contatoServico.RegistrarAsync(novoContatoModelo);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(novoContato.Nome, resultado.Nome);
            Assert.Equal(novoContato.Email, resultado.Email);
            _mockContatoRepositorio.Verify(r => r.SalvaAsync(It.IsAny<Contato>()), Times.Once);
        }

        [Trait("Contato", "Operações de alteração")]
        [Fact(DisplayName = "Deve alterar um contato existente com sucesso")]
        public async Task TestaAlteracaoContato_Sucesso()
        {
            // Arrange
            ContatoFixture _contatoFixture = new ContatoFixture();
            Contato _contato = _contatoFixture.GeraContatoComUmTelefone();

            _mockContatoRepositorio
                .Setup(r => r.AlteraAsync(It.IsAny<Contato>()))
                .ReturnsAsync(_contato);

            _mockContatoRepositorio
                .Setup(r => r.BuscaPorIdAsync(_contato.Id))
                .ReturnsAsync(_contato);

            List<TelefoneModelo> _telefones = _contatoFixture.GerarTelefoneModelos(1);
            ContatoAlteradoModelo contatoAtualizado = new ContatoAlteradoModelo() { Id = _contato.Id, Email = _contato.Email, Nome = "OUTRO NOME", Telefones = _telefones};

            // Act
            var resultado = await _contatoServico.AtualizarAsync(contatoAtualizado);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(_contato.Nome, resultado.Nome);
            Assert.Equal(_contato.Email, resultado.Email);
            _mockContatoRepositorio.Verify(r => r.AlteraAsync(It.IsAny<Contato>()), Times.Once);
        }

        [Trait("Contato", "Operações de exclusão")]
        [Fact(DisplayName = "Deve excluir um contato existente com sucesso")]
        public async Task TestaExclusaoContato_Sucesso()
        {
            // Arrange
            ContatoFixture _contatoFixture = new ContatoFixture();
            var _contato = _contatoFixture.GeraContatoComUmTelefone();
            _mockContatoRepositorio
                .Setup(r => r.DeletaAsync(_contato))
                .ReturnsAsync(true);

            _mockContatoRepositorio
               .Setup(r => r.BuscaPorIdAsync(_contato.Id))
               .ReturnsAsync(_contato);

            // Act
            var resultado = await _contatoServico.RemoverAsync(_contato.Id);

            // Assert
            Assert.True(resultado);
            _mockContatoRepositorio.Verify(r => r.DeletaAsync(It.IsAny<Contato>()), Times.Once);
        }
    }
}
