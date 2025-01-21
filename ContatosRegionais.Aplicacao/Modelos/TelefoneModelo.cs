using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContatosRegionais.Aplicacao.Modelos
{
    public class TelefoneModelo
    {
        private int _codigo;
        private int _numero;

        [Required(AllowEmptyStrings = false, ErrorMessage = "O código de área do telefone deve ser informado.")]
        [Range(10, 99, ErrorMessage = "O código deve conter dois digítos.")]
        public int Codigo { get { return _codigo; } set { _codigo = value; } }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O número do telefone deve ser informado.")]
        public int Numero { get { return _numero; } set { _numero = value; } }
    }
}
