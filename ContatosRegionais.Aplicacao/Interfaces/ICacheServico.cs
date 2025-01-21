using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContatosRegionais.Aplicacao.Interfaces
{
    public interface ICacheServico
    {
        object Get(string key);
        void Set (string key, object value);
        void Remove(string key);
        void Dispose();
    }
}
