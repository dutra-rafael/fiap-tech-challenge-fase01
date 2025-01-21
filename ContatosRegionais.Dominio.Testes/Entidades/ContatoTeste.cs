using Bogus;
using ContatosRegionais.Aplicacao.Modelos;
using ContatosRegionais.Dominio.Entidades;
using Xunit;

namespace ContatosRegionais.Dominio.Testes.Entidades
{
    [CollectionDefinition("Contato Fixture")]
    public class ContatoCollectionFixture : ICollectionFixture<ContatoFixture> { }


    [Collection("Contato Fixture")]
    public class ContatoTeste
    {
        private readonly ContatoFixture _contatoFixture;
        public ContatoTeste(ContatoFixture contatoFixture)
        {
            _contatoFixture = contatoFixture;
        }

        [Trait("Contato", "Validando entidade")]
        [Fact(DisplayName = "Deve retornar falha quando nenhum número de telefone é informado.")]
        public void TestaEntidadeContato_SemTelefone_Retorna_Falha()
        {
            var _resposta = Assert.Throws<EntidadeExcessoes>(() => _contatoFixture.GeraContatoSemTelefone());

            Assert.Equal("É necessário a inclusão de pelo menos um número de telefone.", _resposta.Message);
        }

        [Trait("Contato", "Validando entidade")]
        [Fact(DisplayName = "Deve retornar falha quando o e-mail nao é informado.")]
        public void TestaEntidadeContato_SemEmail_Retorna_Falha()
        {
            var _resposta = Assert.Throws<EntidadeExcessoes>(() => _contatoFixture.GeraContatoSemEmail());

            Assert.Equal("É necessário a inclusão de um email válido.", _resposta.Message);
        }

        [Trait("Contato", "Validando entidade")]
        [Fact(DisplayName = "Deve retornar falha quando um e-mail invalido é informado.")]
        public void TestaEntidadeContato_EmailInvalido_Retorna_Falha()
        {
            var _resposta = Assert.Throws<EntidadeExcessoes>(() => _contatoFixture.GeraContatoComEmailInvalido());

            Assert.Equal("É necessário a inclusão de um email válido.", _resposta.Message);
        }

        [Trait("Contato", "Validando entidade")]
        [Fact(DisplayName = "Deve retornar falha quando um nome de contato nao é informado.")]
        public void TestaEntidadeContato_SemNome_Retorna_Falha()
        {
            var _resposta = Assert.Throws<EntidadeExcessoes>(() => _contatoFixture.GeraContatoSemNome());

            Assert.Equal("É necessário a inclusão de um nome para o contato.", _resposta.Message);
        }

        [Trait("Contato","Validando entidade")]
        [Fact(DisplayName = "Deve instanciar um contato com sucesso.")]
        public void TestaEntidadeContato_Retona_Sucesso()
        {
            var _entidade = _contatoFixture.GeraContatoComVariosTelefones();

            Assert.NotNull(_entidade);
        }



    }

    #region [CONTATO FIXTURE]
    public class ContatoFixture
    {
        private readonly Faker _faker;
        public ContatoFixture()
        {
            _faker = new Faker();
        }

        public Contato GeraContatoSemNome() 
        {
            List<Telefone> _telefones = GerarTelefones(1);
            Contato _contato = new Contato(null, _faker.Person.Email, _telefones);

            return _contato;
        }

        public Contato GeraContatoSemEmail() {

            List<Telefone> _telefones = GerarTelefones(1);
            Contato _contato = new Contato(_faker.Name.FullName(), null, _telefones);

            return _contato;
        }

        public Contato GeraContatoComEmailInvalido() {

            List<Telefone> _telefones = GerarTelefones(1);
            Contato _contato = new Contato(_faker.Name.FullName(), "jose#materia$@exemplocom", _telefones);

            return _contato;
        }

        public Contato GeraContatoSemTelefone()
        {
            Contato _contato = new Contato(_faker.Name.FullName(), _faker.Person.Email, new List<Telefone>());

            return _contato;
        }

        public Contato GeraContatoComUmTelefone()
        {
            List<Telefone> _telefones = GerarTelefones(1);
            Contato _contato = new Contato(_faker.Name.FullName(), _faker.Person.Email, _telefones);

            return _contato;
        }

        public Contato GeraContatoComVariosTelefones()
        {
            List<Telefone> _telefones = GerarTelefones(_faker.Random.Int(2, 10));
            Contato _contato = new Contato(_faker.Name.FullName(), _faker.Person.Email, _telefones);

            return _contato;
        }

        public List<Contato> GeraContatosComVariosTelefones(int quantidade)
        {
            List<Contato> _contatos = new List<Contato>();

            for (int i = 0; i < quantidade; i++) {
                _contatos.Add(GeraContatoComVariosTelefones());
            }

            return _contatos;
        }

        public ContatoNovoModelo GeraContatoModeloComVariosTelefones()
        {
            List<Telefone> _telefones = GerarTelefones(_faker.Random.Int(2, 10));
            List<TelefoneModelo> _telefoneModelos = new List<TelefoneModelo>();

            foreach (var telefone in _telefones)
            {
                TelefoneModelo _telefone = new TelefoneModelo() { Codigo = telefone.Codigo, Numero = telefone.Numero};
                _telefoneModelos.Add(_telefone);
            }

            ContatoNovoModelo _contato = new ContatoNovoModelo() { Nome = _faker.Name.FullName(), Email = _faker.Person.Email, Telefones = _telefoneModelos  }; 

            return _contato;
        }

        public ContatoAlteradoModelo GeraContatoAlteradoModeloComVariosTelefones()
        {
            List<Telefone> _telefones = GerarTelefones(_faker.Random.Int(2, 10));
            List<TelefoneModelo> _telefoneModelos = new List<TelefoneModelo>();
            foreach (var telefone in _telefones)
            {
                TelefoneModelo _telefone = new TelefoneModelo() { Codigo = telefone.Codigo, Numero = telefone.Numero };
                _telefoneModelos.Add(_telefone);
            }

            ContatoAlteradoModelo _contato = new ContatoAlteradoModelo() { Nome= _faker.Name.FullName(), Email= _faker.Person.Email, Telefones = _telefoneModelos };

            return _contato;
        }

        private List<Telefone> GerarTelefones(int quantidade)
        {
            List<Telefone> _telefones = new List<Telefone>();
            for (int i = 0; i < quantidade ; i++)
            {
                Telefone _telefone = new Telefone(_faker.Random.Int(11, 99), int.Parse(_faker.Phone.PhoneNumber("#########")));
                _telefones.Add(_telefone);
            }

            return _telefones;
        }

        public List<TelefoneModelo> GerarTelefoneModelos(int quantidade)
        {
            List<TelefoneModelo> _telefones = new List<TelefoneModelo>();
            for (int i = 0;i < quantidade ; i++)
            {
                TelefoneModelo _telefone = new TelefoneModelo() { Codigo = _faker.Random.Int(11, 99), Numero = int.Parse(_faker.Phone.PhoneNumber("#########")) };
                _telefones.Add(_telefone);
            }    

            return _telefones;
        }
    }

    #endregion

}
