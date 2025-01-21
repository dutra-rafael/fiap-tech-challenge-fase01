using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ContatosRegionais.Dominio.Entidades
{
    public static class EntidadeValidacao
    {
        public static void ValidaTamanhoTexto(string texto, int tamanhoMin, int tamanhoMax, string? nomeVariavel)
        {
            int _tamanhoTexto = texto.Trim().Length;
            
            if (_tamanhoTexto > tamanhoMax)
                throw new EntidadeExcessoes($"{nomeVariavel} -> Quantidade máxima de {tamanhoMax} caracteres;");

            if (_tamanhoTexto < tamanhoMin)
                throw new EntidadeExcessoes($"{nomeVariavel } -> Quantidade mínima de {tamanhoMin} caracteres;");
        }

        public static bool ValidaFormatoEmail(string email)
        {
            if(string.IsNullOrEmpty(email))
                return false;

            string _condicao = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email, _condicao);
        }
    }
}
