using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (Char.IsWhiteSpace(codigo[posicion]))
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
                while (inicio < codigo.Length && Char.IsLetter(codigo[inicio]))
                {
                    inicio++;
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
    }
}
