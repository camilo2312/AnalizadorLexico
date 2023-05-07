using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexico.model
{
    public class PalabrasReservadas
    {
        public List<string> lstPalabrasReservadas { get; set; }

        public PalabrasReservadas()
        {
            lstPalabrasReservadas = new List<string>()
            {
                "PUBLICO",
                "PRIVADO",
                "PROTEGIDO",
                "DECIMAL",
                "ENTERO",
                "NORETORNA",
                "CADENA",
                "CARACTER",
                "SI",
                "YY",
                "OO",
                "ENTONCES",
                "SINO",
                "MIENTRAS",
                "PARA",
                "HACERMIENTRAS",
                "IMPRIMIR"
            };
            
        }        
    }
}
