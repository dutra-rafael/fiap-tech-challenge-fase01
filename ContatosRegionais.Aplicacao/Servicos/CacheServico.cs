using ContatosRegionais.Aplicacao.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContatosRegionais.Aplicacao.Servicos
{
    public class CacheServico : ICacheServico
    {
        private readonly IMemoryCache _cache;

        public CacheServico(IMemoryCache cache)
        {
            _cache = cache;
        }
        public object? Get(string key)
        {
            return _cache.TryGetValue(key, out var value) ? value : null;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public void Set(string key, object value)
        {
            _cache.Set(key, value, TimeSpan.FromSeconds(10));
        }

        public void Dispose() 
        {
            _cache.Dispose();
        }


    }
}
