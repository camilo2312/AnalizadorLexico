using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexico.model
{
    public enum Categoria
    {
        NO_RECONOCIDO, // Ok
        ESPACIO, // OK
        ENTERO, // OK
        DECIMAL, // OK
        IDENTIFICADOR, // OK
        PALABRA_RESERVADA, // OK
        CADENA_CARACTERES, // OK
        COMENTARIO_LINEA, // OK
        COMENTARIO_BLOQUE, // OK
        OPERADOR_ARITMETICO, // OK
        OPERADOR_RELACIONAL, // OK
        OPERADOR_LOGICO, // OK
        OPERADOR_INCREMENTO, // OK
        OPERADOR_ASIGNACION, // OK
        OPERADOR_DECREMENTO, // OK
        PARENTESIS_APERTURA, // OK
        PARENTESIS_CIERRE, // OK
        LLAVE_APERTURA, // OK
        LLAVE_CIERRE, // OK
        CORCHETE_APERTURA, // OK,
        CORCHETE_CIERRE, // OK
        FINAL_SENTENCIA, // OK
        HEXADECIMAL // OK
    }
}
