using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContatosRegionais.Dominio.Entidades
{
    public class EntidadeExcessoes: Exception
    {
        public EntidadeExcessoes() { }

        public EntidadeExcessoes(string mensagem):base(mensagem) { }

        public EntidadeExcessoes(string mensagem, Exception innerException):base(mensagem, innerException) { }
    }
}
