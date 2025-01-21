using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContatosRegionais.Aplicacao.Modelos
{
    public class ContatoNovoModelo
    {
        private string _nome;
        private string _email;
        private IEnumerable<TelefoneModelo> _telefones;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Um nome de contato deve ser informado.")]
        [MaxLength(40, ErrorMessage = "O nome deve conter no máximo 40 caracteres.")]
        [MinLength(4, ErrorMessage = "O nome deve conter no mínimo 4 caracteres.")]
        public string Nome { get { return _nome; } set { _nome = value; } }

        [Required(AllowEmptyStrings = false, ErrorMessage ="O email do contato deve ser informado.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string Email { get { return _email; } set { _email = value; } }

        [Required(ErrorMessage = "Pelo menos um número de telefone deve ser informado.")]
        public IEnumerable<TelefoneModelo> Telefones { get { return _telefones; } set { _telefones = value; } }
    }
}
