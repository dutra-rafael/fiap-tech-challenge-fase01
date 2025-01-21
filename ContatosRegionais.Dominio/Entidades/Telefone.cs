using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContatosRegionais.Dominio.Entidades
{
    public class Telefone : EntidadeBase, IEquatable<Telefone>
    { 
        public int Codigo { get; set; }
        public int Numero { get; set; }
        public int ContatoId { get; set; }
        public Contato Contato { get; set; }

        public Telefone(){}

        public Telefone(int codigo, int numero):base()
        {
            Codigo = codigo;
            Numero = numero;
            DataCriacao = DateTime.Now;
        }

        public Telefone(int codigo, int numero, int contatoId) { 
            Codigo = codigo;
            Numero = numero;
            ContatoId = contatoId;
        }

        public bool Equals(Telefone? other)
        {
            if (other == null) return false;
            return this.Id == other.Id && this.Codigo == other.Codigo && this.Numero == other.Numero && this.ContatoId == other.ContatoId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Codigo, Numero, ContatoId);
        }
    }
}
