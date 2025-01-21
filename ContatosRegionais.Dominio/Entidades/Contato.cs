using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ContatosRegionais.Dominio.Entidades
{
    public class Contato: EntidadeBase
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public List<Telefone> Telefones { get; set; } = new List<Telefone>();

        public Contato()
        {
        }

        public Contato(string nome, string email, List<Telefone> telefones): base()
        {
            if(nome != null)
                Nome = MantemUmEspacoEmBrancoPorPalavra(RemoveNumerosCaracteresEspeciais(nome.ToUpper()));

            if(email != null)
                Email = email.ToLower().Trim();

            Telefones = telefones;

            ValidaCampos();
        }

        public Contato(int id, string nome, string email, List<Telefone> telefones): base()
        {
            Id = id;
            Nome = MantemUmEspacoEmBrancoPorPalavra(RemoveNumerosCaracteresEspeciais(nome.ToUpper()));
            Email = email.ToLower().Trim();
            Telefones = telefones;

            ValidaCampos();
        }

        private string RemoveNumerosCaracteresEspeciais(string nome)
        {
            return new string(nome.Where(x => char.IsLetter(x) || char.IsWhiteSpace(x)).ToArray());
        }

        private string MantemUmEspacoEmBrancoPorPalavra(string nome)
        {
            string _nome = nome.Trim();
            while (_nome.Contains("  "))
                _nome = _nome.Replace("  ", " ");
            
            return _nome;
        }

        private void ValidaCampos()
        {
            if (!EntidadeValidacao.ValidaFormatoEmail(this.Email))
                throw new EntidadeExcessoes("É necessário a inclusão de um email válido.");

            if (!this.Telefones.Any() || this.Telefones.Count == 0)
                throw new EntidadeExcessoes("É necessário a inclusão de pelo menos um número de telefone.");

            if (string.IsNullOrEmpty(this.Nome))
                throw new EntidadeExcessoes("É necessário a inclusão de um nome para o contato.");

            EntidadeValidacao.ValidaTamanhoTexto(this.Nome, 4, 40, null);

        }
    }
}
