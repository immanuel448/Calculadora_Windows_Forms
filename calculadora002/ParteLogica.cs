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

        public bool sEvaluarRangoNumero(double doubleIngresado)
        {
            // Verificar si está dentro del rango
            if (doubleIngresado > -10000000000 && doubleIngresado < 10000000000)
            {
                //DENTRO DEL RANGO
                // Convertir el número a string para verificar los ceros después del punto decimal
                string datoString = doubleIngresado.ToString();

                // Buscar el punto decimal
                int puntoIndex = datoString.IndexOf('.');
                Console.WriteLine("dentro de rango");
                if (puntoIndex != -1 && (datoString.Length >= puntoIndex + 1))
                {
                    Console.WriteLine("dentro de rango y con punto");
                    //si hay punto y hay almenos un decimal
                    string parteDecimal = datoString.Substring(puntoIndex + 1);
                    if (parteDecimal.Length >= 8)
                    {
                        Console.WriteLine("dentro de rango con mas de 8 decimales");
                        //mínimo 8 decimales
                        //recorrer el string
                        for (int i = 0; i < parteDecimal.Length; i++)
                        {
                            if (parteDecimal[i] != '0')
                            {
                                //mínimo uno no es cero
                                return true;
                            }
                            else
                            {
                                //Son mínimo 9 ceros, NO PASA
                                return false;
                            }
                        }
                    }
                    return true; // menor a 9 decimales es ok
                }

                return true; // Está dentro del rango y cumple con la condición de ceros después del punto
            }

            return false; // No está dentro del rango
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
