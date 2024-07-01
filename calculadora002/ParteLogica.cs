using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calculadora002
{
    internal class ParteLogica
    {
        public bool EvaluarRangoNumero(double dato)
        {
            return dato < 10000000000 && dato > -10000000000 ? true : false;
        }

        //este método sólo cuenta la parte entera
        public bool EvaluarDimensionString(string numeroString)
        {
            // Encontrar la posición del punto decimal
            int indicePuntoDecimal = numeroString.IndexOf('.');
            // Obtener la parte entera del número
            string parteEntera;

            if (indicePuntoDecimal != -1)
            {
                //sí hay punto decimal
                parteEntera = numeroString.Substring(0, indicePuntoDecimal);
            }
            else
            {
                //no hay punto, es un entero
                parteEntera = numeroString;
            }
            //quitar el signo, para contar los digitos
            parteEntera = QuitarSignoNegativoString(parteEntera);
            return parteEntera.Length <= 10 ? true : false;
        }

        public string QuitarSignoNegativoString(string miString)
        {
            if (miString.Length > 0 && miString[0] == '-')
            {
                miString = miString.Substring(1);//elimina el "-"
            }
            return miString;
        }

    }
}
