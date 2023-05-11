using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalizadorLexico.model
{
    public class AnalizadorLexico
    {
        public string codigo { get; set; }
        public List<Token> lstTokens { get; set; }
        public PalabrasReservadas palabrasReservadas { get; set; }

        public AnalizadorLexico(string codigo) 
        {
            this.codigo = codigo;
            lstTokens = new List<Token>();
            palabrasReservadas = new PalabrasReservadas();
        }

        /// <summary>
        /// Método que permite extraer todos los tokens asociados al código fuente
        /// ingresado 
        /// </summary>
        public void extraerTokens() 
        {
            Token token;

            int posicion = 0;
            while (posicion < codigo.Length)
            {
                token = extraerSiguienteToken(posicion);
                lstTokens.Add(token);
                posicion = token.indiceStage;
            }
        }

        /// <summary>
        /// Método que permite extraer el token siguiente del caracter del código fuente
        /// por medio de su posición
        /// </summary>
        /// <param name="posicion">Permite especificar la posicón de la cual se va a encontrar su token asociado</param>
        /// <returns></returns>
        private Token extraerSiguienteToken(int posicion)
        {
            Token token;

            token = extraerDecimal(posicion);
            if (token != null)
                return token;

            token = extraerEntero(posicion);
            if (token != null)
                return token;

            token = extraerOperadorAsignacion(posicion);
            if (token != null)
                return token;

            token = extraerOperadorIncremento(posicion);
            if (token != null)
                return token;

            token = extraerComentarioLinea(posicion);
            if (token != null)
                return token;

            token = extraerComentarioBloque(posicion);
            if (token != null)
                return token;

            token = extraerOperadorDecremento(posicion);
            if (token != null)
                return token;

            token = extraerOperadorAritmetico(posicion);
            if (token != null)
                return token;

            token = extraerEspacio(posicion);
            if (token != null)
                return token;

            token = extraerIdentificador(posicion);
            if (token != null)
                return token;

            token = extraerPalabraReservada(posicion);
            if (token != null)
                return token;

            token = extraerParentesisApertura(posicion);
            if (token != null)
                return token;

            token = extraerParentesisCierre(posicion);
            if (token != null)
                return token;

            token = extraerLlaveApertura(posicion);
            if (token != null)
                return token;

            token = extraerLlaveCierre(posicion);
            if (token != null)
                return token;

            token = extraerCorcheteApertura(posicion);
            if (token != null)
                return token;

            token = extraerCorcheteCierre(posicion);
            if (token != null)
                return token;

            token = extraerFinalSentencia(posicion);
            if (token != null)
                return token;

            token = extraerCadenaCaracteres(posicion);
            if (token != null)
                return token;

            token = extraerHexadecimal(posicion);
            if (token != null)
                return token;

            token = extraerOperadorRelacional(posicion);
            if (token != null)
                return token;

            token = extraerOperadorLogico(posicion);
            if (token != null)
                return token;

            token = extraerTokenNoReconocido(posicion);
            return token;
        }

        /// <summary>
        /// Método que permite extraer el decimal del código fuente
        /// </summary>
        /// <param name="posicion">Parametro que señala la posición actual de la cual se va a extraer el número decimal</param>
        /// <returns></returns>
        private Token extraerDecimal(int posicion) 
        {
            if (Char.IsDigit(codigo[posicion]))
            {
                int inicio = posicion;
                while (posicion < codigo.Length)
                {
                    if (Char.IsDigit(codigo[posicion]))
                    {
                        posicion++;
                    }
                    else if (codigo[posicion] == '.')
                    {
                        int valorSiguiente = posicion + 1;
                        if (valorSiguiente < codigo.Length && Char.IsDigit(codigo[valorSiguiente]))
                        {
                            posicion++;
                        }
                        else
                        {
                            return null;
                        }
                    } 
                    else 
                    {
                        break;
                    }
                }

                string lexema = codigo.Substring(inicio, posicion - inicio);

                if (lexema.Contains("."))
                {
                    return new Token(lexema, Categoria.DECIMAL, posicion);
                }
                else
                {
                    return null;
                }
            }

            return null;
        }
        /// <summary>
        /// Método que permite extraer un número entero del código fuente
        /// </summary>
        /// <param name="posicion">Parametro que señala la posición actual de la cual se va a extraer el número entero</param>
        /// <returns></returns>
        private Token extraerEntero(int posicion)
        {

            if (Char.IsDigit(codigo[posicion]))
            {
                int inicio = posicion;
                while (posicion < codigo.Length && Char.IsDigit(codigo[posicion]))
                {
                    posicion++;
                }

                return new Token(codigo.Substring(inicio, posicion - inicio), Categoria.ENTERO, posicion);
            }

            return null;
        }
        /// <summary>
        /// Método que permite extraer el operador aritmetico del código fuente ingresado
        /// </summary>
        /// <param name="posicion">Parametro que señala la posición actual de la cual se va a extraer un operador aritmetico</param>
        /// <returns></returns>
        private Token extraerOperadorAritmetico(int posicion)
        {
            if (codigo[posicion] == '+' || codigo[posicion] == '-' || codigo[posicion] == '*' || codigo[posicion] == '/' || codigo[posicion] == '%') 
            {
                return new Token(codigo.Substring(posicion, 1), Categoria.OPERADOR_ARITMETICO, posicion + 1);
            }

            return null;
        }
        /// <summary>
        /// Método que permite extraer los espacios del código fuente
        /// </summary>
        /// <param name="posicion">Parametro que señala la posición actual de la cual se va a extraer el espacio</param>
        /// <returns></returns>
        private Token extraerEspacio(int posicion) 
        {
            if (Char.IsWhiteSpace(codigo[posicion]) || codigo[posicion] == '\n')
                return new Token(codigo.Substring(posicion, 1), Categoria.ESPACIO, posicion + 1);

            return null;
        }
        /// <summary>
        /// Método que permite extraer un identificador del código fuente
        /// </summary>
        /// <param name="posicion">Parametro que señala la posición actual de la cual se va a extraer el identificador</param>
        /// <returns></returns>
        private Token extraerIdentificador(int posicion)
        {
            if (codigo[posicion] == '$')
            {
                int inicio = posicion + 1;
                int cantidad = 0;
                while (inicio < codigo.Length && Char.IsLetter(codigo[inicio]) && cantidad < 10)
                {
                    inicio++;
                    cantidad++;
                }

                return new Token(codigo.Substring(posicion, inicio - posicion), Categoria.IDENTIFICADOR, inicio);
            }

            return null;
        }
        /// <summary>
        /// Método que permite extraer los caracteres no reconocidos del código fuente ingresado
        /// </summary>
        /// <param name="posicion">Parametro que señala la posición actual de la cual se va a extraer los caracteres no reconocidos</param>
        /// <returns></returns>
        /// 
        private Token extraerTokenNoReconocido(int posicion)
        {
            string lexema = codigo.Substring(posicion, 1);
            return new Token(lexema, Categoria.NO_RECONOCIDO, posicion + 1);
        }
        /// <summary>
        /// Método que permite extraer las palabras reservadas asociadas a nuestro lenguaje
        /// </summary>
        /// <param name="posicion">Parametro que señala la posición actual de la cual se va a extraer las palabras reservadas</param>
        /// <returns></returns>
        private Token extraerPalabraReservada(int posicion)
        {

            int inicio = posicion;
            while (posicion < codigo.Length && Char.IsLetter(codigo[posicion]))
            {
                posicion++;
            }

            if (palabrasReservadas.lstPalabrasReservadas.Contains(codigo.Substring(inicio, posicion - inicio).ToUpper()))
            {
                return new Token(codigo.Substring(inicio, posicion - inicio), Categoria.PALABRA_RESERVADA, posicion);
            }

            return null;
        }

        /// <summary>
        /// Método que permite extraer un parentesis de apertura "(" del código fuente ingresado
        /// </summary>
        /// <param name="posicion">Parametros que señala la posición actual de la cual se va a extraer el parentesis de apertura</param>
        /// <returns></returns>
        private Token extraerParentesisApertura(int posicion)
        {
            if (codigo[posicion] == '(') 
            {
                return new Token(codigo.Substring(posicion, 1), Categoria.PARENTESIS_APERTURA, posicion + 1);
            }

            return null;
        }

        /// <summary>
        /// Método que permite extraer un parentesis de cierre ")" del código fuente ingresado
        /// </summary>
        /// <param name="posicion">Parametros que señala la posición actual de la cual se va a extraer el parentesis de cierre</param>
        /// <returns></returns>
        private Token extraerParentesisCierre(int posicion)
        {
            if (codigo[posicion] == ')')
            {
                return new Token(codigo.Substring(posicion, 1), Categoria.PARENTESIS_CIERRE, posicion + 1);
            }

            return null;
        }

        /// <summary>
        /// Método que permite extraer una llave de apertura "{" del código fuente ingresado
        /// </summary>
        /// <param name="posicion">Parametros que señala la posición actual de la cual se va a extraer la llave de cierre</param>
        /// <returns></returns>
        private Token extraerLlaveApertura(int posicion)
        {
            if (codigo[posicion] == '{')
            {
                return new Token(codigo.Substring(posicion, 1), Categoria.LLAVE_APERTURA, posicion + 1);
            }

            return null;
        }

        /// <summary>
        /// Método que permite extraer una llave de cierre "}" del código fuente ingresado
        /// </summary>
        /// <param name="posicion">Parametros que señala la posición actual de la cual se va a extraer la llave de cierre</param>
        /// <returns></returns>
        private Token extraerLlaveCierre(int posicion)
        {
            if (codigo[posicion] == '}')
            {
                return new Token(codigo.Substring(posicion, 1), Categoria.LLAVE_CIERRE, posicion + 1);
            }

            return null;
        }

        /// <summary>
        /// Método que permite extraer un corchete de apertura "[" del código fuente ingresado
        /// </summary>
        /// <param name="posicion">Parametros que señala la posición actual de la cual se va a extraer el corchete de apertura</param>
        /// <returns></returns>
        private Token extraerCorcheteApertura(int posicion)
        {
            if (codigo[posicion] == '[')
            {
                return new Token(codigo.Substring(posicion, 1), Categoria.CORCHETE_APERTURA, posicion + 1);
            }

            return null;
        }

        /// <summary>
        /// Método que permite extraer un corchete de cierre "]" del código fuente ingresado
        /// </summary>
        /// <param name="posicion">Parametro que señala la posición actual de la cual se va a extraer el corchete de cierre</param>
        /// <returns></returns>
        private Token extraerCorcheteCierre(int posicion)
        {
            if (codigo[posicion] == ']')
            {
                return new Token(codigo.Substring(posicion, 1), Categoria.CORCHETE_CIERRE, posicion + 1);
            }

            return null;
        }

        /// <summary>
        /// Método que permite extraer del código de fuente el ";" que señala el final de una setencia
        /// </summary>
        /// <param name="posicion">Parametro que señala la posición actual de la cual se va a extraer el final de sentencia</param>
        /// <returns></returns>
        private Token extraerFinalSentencia(int posicion) 
        {
            if (codigo[posicion] == ';')
            {
                return new Token(codigo.Substring(posicion, 1), Categoria.FINAL_SENTENCIA, posicion + 1);
            }

            return null;
        }
        /// <summary>
        /// Método que permite extraer una cadena de caracteres limitada por "@" en el código fuente ingresado
        /// </summary>
        /// <param name="posicion">Parametro que señala la posición actual de la cual se va a extraer la cadena de caracteres</param>
        /// <returns></returns>
        private Token extraerCadenaCaracteres(int posicion)
        {
            if (codigo[posicion] == '@') 
            {
                int indice = posicion + 1;
                while (indice < codigo.Length)
                {
                    if (codigo[indice] == '@')
                    {
                        indice++;
                        break;
                    }

                    indice++;

                }
                return new Token(codigo.Substring(posicion, indice - posicion), Categoria.CADENA_CARACTERES, indice);
            }
            return null;
        }

        /// <summary>
        /// Método que permite obtener los operadores relacionales del código ingresado
        /// </summary>
        /// <param name="posicion">Parametros que señala la posición actual de la cual se va a extraer el operador relacional</param>
        /// <returns></returns>
        private Token extraerOperadorRelacional(int posicion)
        {
            int posicionSiguiente = posicion + 1;
            switch (codigo[posicion])
            {
                case '>':
                case '<':
                    if (posicionSiguiente < codigo.Length && codigo[posicionSiguiente] == '=')
                    {
                        return new Token(codigo.Substring(posicion, 2), Categoria.OPERADOR_RELACIONAL, posicionSiguiente + 1);
                    }

                    return new Token(codigo.Substring(posicion, 1), Categoria.OPERADOR_RELACIONAL, posicion + 1);
                case '=':
                case '!':
                    if (posicionSiguiente < codigo.Length && codigo[posicionSiguiente] == '=')
                    {
                        return new Token(codigo.Substring(posicion, 2), Categoria.OPERADOR_RELACIONAL, posicionSiguiente + 1);
                    }
                    break;
            }

            return null;
        }

        /// <summary>
        /// Función que realiza la extracción de un operador lógico
        /// </summary>
        /// <param name="posicion">Dato que obtiene la posición actual para comenzar con la extracción del operador</param>
        /// <returns></returns>
        private Token extraerOperadorLogico(int posicion)
        {
            int valorSig = posicion + 1;
            if (codigo[posicion] == 'Y')
            {
                if (valorSig < codigo.Length && codigo[valorSig] == 'Y')
                {
                    return new Token(codigo.Substring(posicion, 2), Categoria.OPERADOR_LOGICO, valorSig + 1);
                }
            }
            else if (codigo[posicion] == 'O')
            {
                if (valorSig < codigo.Length && codigo[valorSig] == 'O')
                {
                    return new Token(codigo.Substring(posicion, 2), Categoria.OPERADOR_LOGICO, valorSig + 1);
                }
            }
            else if (codigo[posicion] == 'N')
            {
                if (valorSig < codigo.Length && codigo[valorSig] == 'O')
                {
                    int valorNot = valorSig + 1;
                    if (valorNot < codigo.Length && codigo[valorNot] == 'T')
                    {
                        return new Token(codigo.Substring(posicion, 3), Categoria.OPERADOR_LOGICO, valorNot + 1);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Función que realiza la extracción de un operador de incremento (++)
        /// </summary>
        /// <param name="posicion">Dato que muestra la posición actual para comenzar a extraer el operador de incremento</param>
        /// <returns></returns>
        private Token extraerOperadorIncremento(int posicion) 
        {
            int valorSig = posicion + 1;
            if (codigo[posicion] == '+')
            {
                if (valorSig < codigo.Length && codigo[valorSig] == '+')
                {
                    return new Token(codigo.Substring(posicion, 2), Categoria.OPERADOR_INCREMENTO, valorSig + 1);
                }
            }

            return null;
        }

        /// <summary>
        /// Función que realiza la extracción de un operador de decremento (--)
        /// </summary>
        /// <param name="posicion">Dato que muestra la posición actual para comenzar a extraer el operador de decremento</param>
        /// <returns></returns>
        private Token extraerOperadorDecremento(int posicion)
        {
            int valorSig = posicion + 1;
            if (codigo[posicion] == '-')
            {
                if (valorSig < codigo.Length && codigo[valorSig] == '-')
                {
                    return new Token(codigo.Substring(posicion, 2), Categoria.OPERADOR_DECREMENTO, valorSig + 1);
                }
            }

            return null;
        }
        
        /// <summary>
        /// Función que señala los caracteres del código que son comentarios de línea
        /// </summary>
        /// <param name="posicion">Determina la posicón donde se empiza a evaluar el comentario de línea</param>
        /// <returns></returns>
        private Token extraerComentarioLinea(int posicion)
        {
            if (codigo[posicion] == '/') 
            {
                int valorSig = posicion + 1;
                if (valorSig < codigo.Length && codigo[valorSig] == '/')
                {
                    while (valorSig < codigo.Length)
                    {
                        if (codigo[valorSig] == '\n')
                            break;
                        valorSig++;
                    }

                    return new Token(codigo.Substring(posicion, valorSig - posicion), Categoria.COMENTARIO_LINEA, valorSig);
                }
            }

            return null;
        }

        /// <summary>
        /// Función que permite obtener el token de comentario de bloque
        /// </summary>
        /// <param name="posicion">Dato que marca el inició de la extracción del token de comentario de bloque del código ingresado</param>
        /// <returns></returns>
        private Token extraerComentarioBloque(int posicion)
        {
            if (codigo[posicion] == '/')
            {
                int valorSig = posicion + 1;
                if (valorSig < codigo.Length && codigo[valorSig] == ':')
                {
                    while (valorSig < codigo.Length)
                    {
                        if (valorSig < codigo.Length && codigo[valorSig] == ':')
                        {
                            valorSig = valorSig + 1;
                            if (valorSig < codigo.Length && codigo[valorSig] == '/')
                            {
                                valorSig++;
                                break;
                            }
                        }
                        else
                        {
                            valorSig++;
                        }
                    }

                    return new Token(codigo.Substring(posicion, valorSig - posicion), Categoria.COMENTARIO_BLOQUE, valorSig);
                }
            }

            return null;
        }

        /// <summary>
        /// Función que extrae un operador de asignación
        /// </summary>
        /// <param name="posicion">El la posición donde se inicia a extraer el operador</param>
        /// <returns></returns>
        private Token extraerOperadorAsignacion(int posicion)
        {
            int valorSig = posicion + 1;
            switch (codigo[posicion])
            {
                case '=':
                    return new Token(codigo.Substring(posicion, 1), Categoria.OPERADOR_ASIGNACION, valorSig);
                case '+':
                case '-':
                case '*':
                case '/':
                case '%':
                    if (valorSig < codigo.Length && codigo[valorSig] == '=')
                    {
                        return new Token(codigo.Substring(posicion, 2), Categoria.OPERADOR_ASIGNACION, valorSig + 1);
                    }
                    break;
                default:
                    return null;
            }

            return null;
        }

        /// <summary>
        /// Función que extrae los números hexadecimales del código fuente ingresado
        /// </summary>
        /// <param name="posicion">Dato que indica la posicón actual de donde se parte para extraer el número hexadecimal</param>
        /// <returns></returns>
        private Token extraerHexadecimal(int posicion)
        {
            int posicionInicial = posicion + 1;
            if (codigo[posicion] == 'H')
            {
                if (posicionInicial < codigo.Length && ((codigo[posicionInicial] >= 65 && codigo[posicionInicial] <= 70) || (codigo[posicionInicial] >= 97 && codigo[posicionInicial] <= 102) || (codigo[posicionInicial] >= 48 && codigo[posicionInicial] <= 57)))
                {
                    while (posicionInicial < codigo.Length)
                    {
                        if ((codigo[posicionInicial] >= 65 && codigo[posicionInicial] <= 70) || (codigo[posicionInicial] >= 97 && codigo[posicionInicial] <= 102) || (codigo[posicionInicial] >= 48 && codigo[posicionInicial] <= 57))
                        {
                            posicionInicial++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    return new Token(codigo.Substring(posicion, posicionInicial - posicion), Categoria.HEXADECIMAL, posicionInicial);
                }
            }

            return null;
        }
    }
}
