using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexico.model
{
    public class Token
    {
        public string palabra { get; set; }
        public Categoria categoria { get; set; }
        public int indiceStage { get; set; }

        public Token(string palabra, Categoria categoria, int indiceStage)
        {
            this.palabra = palabra;
            this.categoria = categoria;
            this.indiceStage = indiceStage;
        }
    }
}
