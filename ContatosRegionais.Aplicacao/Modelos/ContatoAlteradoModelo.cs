using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContatosRegionais.Aplicacao.Modelos
{
    public class ContatoAlteradoModelo
    {
        private int _id;
        private string _nome;
        private string _email;
        private List<TelefoneModelo> _telefones;

        public int Id { get { return _id; } set { _id = value; } }
        public string Nome { get { return _nome; } set { _nome = value; } }
        public string Email { get { return _email; } set { _email = value; } }
        public List<TelefoneModelo> Telefones { get { return _telefones; } set { _telefones = value; } }
    }
}
