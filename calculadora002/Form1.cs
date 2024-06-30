
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace calculadora002
{
    public partial class form001 : Form
    {
        private string _operacion { get; set; }
        private double _primerNumero { get; set; }
        private double _segundoNumero { get; set; }
        private double _resultadoNumeros { get; set; }
        private bool _mostrarError { get; set; }
        private bool _btnIgualActivado { get; set; }
        private bool _btnOperacionActivado { get; set; }
        private bool _repetirIgual { get; set; }
        private ParteLogica _objParteLogica { get; set; }



        public form001()
        {
            InitializeComponent();
            ReiniciarVariables();
        }

        //métodos ------------------------------------------
        private void ReiniciarVariables()
        {
            //variables
            txtOperaciones.Text = "";
            _operacion = "";
            _mostrarError = false;
            _primerNumero = double.NaN;
            _segundoNumero = double.NaN;
            _resultadoNumeros = double.NaN;
            _btnIgualActivado = false;
            _btnOperacionActivado = false;
            _repetirIgual = false;
            MostrarEnPantalla("0");
            _objParteLogica = new ParteLogica();

            //botones
            btnSumar.Enabled = true;
            btnRestar.Enabled = true;
            btnMultiplicar.Enabled = true;
            btnDividir.Enabled = true;
            btnIgual.Enabled = true;
            btnMasMenos.Enabled = true;
            btnretroceder.Enabled = true;
        }

        private bool EvaluarDoubleDentroRango(string valor = "0", double miDouble = 0)
        {
            SiHayErrorReiniciaVariables();

            if (double.TryParse(valor, out double numeroDouble))
            {
                if (miDouble != 0)
                {
                    //se ingresó un double
                    numeroDouble = miDouble;
                }

                //sí es un double
                if (_objParteLogica.EvaluarRangoNumero(numeroDouble))
                {
                    //dentro de rango
                    return true;
                }
                else
                {
                    LlamarError("FueraRang2");
                    return false;
                }
            }
            else
            {
                LlamarError("No Num.");
                return false;
            }
        }

        //gestiona numeros y errores
        private void FormatearParaPantalla(string valor)
        {
            if (EvaluarDoubleDentroRango(valor))
            {
                //primero que sí sea un número dentro de rango
                bool negativo = false;
                //quitar signo negativo
                if (valor[0] == '-')
                {
                    negativo = true;
                    valor = valor.Substring(1);
                }

                //el double ya está en rango, este método evalúa solo enteros (es la prioridad)
                if (_objParteLogica.EvaluarDimensionString(valor))
                {
                    if (valor.Length >= 11)//aquí se evalúan enteros y decimales
                    {
                        //se asegura que solo haya 10 caracteres en la pantalla
                        if (valor.Length >= 11 && SaberSiHayPunto(valor))
                        {
                            //con punto
                            //permitir un dígito más
                            valor = valor.Substring(0, 11);

                        }
                        else
                        {
                            //sin punto
                            valor = valor.Substring(0, 10);
                        }
                    }

                    if (negativo == true)
                    {
                        //se regresa el signo
                        valor = "-" + valor;
                    }

                    MostrarEnPantalla(valor);
                }
                else
                {
                    LlamarError("FueraRang1");
                }
            }
        }
        
        private void LlamarError(string mensaje)
        {
            ReiniciarVariables();//eeee revisar esto

            btnSumar.Enabled = false;
            btnRestar.Enabled = false;
            btnMultiplicar.Enabled = false;
            btnDividir.Enabled = false;
            btnIgual.Enabled = false;
            btnMasMenos.Enabled = false;

            _mostrarError = true;
            MostrarEnPantalla(mensaje);
        }

        private void MostrarEnPantalla(string valor)
        {
            txtBox001.Text = valor;
        }

        private double RedondearDouble(double miDouble, int numDecimales)
        {
            //hay que redondear
            return Math.Round(miDouble, numDecimales);
        }

        private double ConvertirADouble(string dato)
        {
            double doubleTemporal = 0.0;
            //se convierte a double
            if (double.TryParse(dato, out doubleTemporal))
            {
                //es un double
                if (_objParteLogica.EvaluarRangoNumero(doubleTemporal))
                {
                        return doubleTemporal;
                }
                else
                {
                    LlamarError("FueraRang1");
                    return 0;
                }
            }
            else
            {
                LlamarError("No. Num1");
                return 0;
            }
        }

        private void SiHayErrorReiniciaVariables()
        {
            if (_mostrarError == true)
            {
                //muestra cero en pantalla
                ReiniciarVariables();
            }
        }

        private void btnIgual_Click(object sender, EventArgs e)
        {
            btnretroceder.Enabled = false;

            //muestra cero en pantalla
            SiHayErrorReiniciaVariables();

            if (_operacion == "")
            { 
                _primerNumero = ConvertirADouble(txtBox001.Text);
                _primerNumero = Math.Round(_primerNumero, 9);
                txtOperaciones.Text = _primerNumero.ToString();
                return;
            }

            _btnOperacionActivado = false;

            if (_operacion != "" && _repetirIgual == false)
            {
                //ya hay un número en la primera variable y una operación
                _segundoNumero = ConvertirADouble(txtBox001.Text);
                _segundoNumero = Math.Round(_segundoNumero, 9);
            }

            switch (_operacion)
            {
                case "sumar":
                    _resultadoNumeros = _primerNumero + _segundoNumero;
                    //_resultadoNumeros = RedondearDouble(_resultadoNumeros, CantidadMaximaDeDecimales());
                    if (EvaluarDoubleDentroRango("0", _resultadoNumeros))
                    {
                        txtOperaciones.Text = $"{_primerNumero} + {_segundoNumero}";
                        _primerNumero = _resultadoNumeros;
                    }
                    break;
                case "restar":
                    _resultadoNumeros = _primerNumero - _segundoNumero;
                    //_resultadoNumeros = RedondearDouble(_resultadoNumeros, CantidadMaximaDeDecimales());
                    if (EvaluarDoubleDentroRango("0", _resultadoNumeros))
                    {
                        txtOperaciones.Text = $"{_primerNumero} - {_segundoNumero}";
                        _primerNumero = _resultadoNumeros;
                    }
                    break;
                case "multiplicar":
                    _resultadoNumeros = _primerNumero * _segundoNumero;
                    //_resultadoNumeros = RedondearDouble(_resultadoNumeros, CantidadMaximaDeDecimales());
                    if (EvaluarDoubleDentroRango("0", _resultadoNumeros))
                    {
                        txtOperaciones.Text = $"{_primerNumero} * {_segundoNumero}";
                        _primerNumero = _resultadoNumeros;
                    }
                    break;
                case "dividir":
                    if (_segundoNumero != 0)
                    {
                        //procede
                        _resultadoNumeros = _primerNumero / _segundoNumero;
                        //_resultadoNumeros = RedondearDouble(_resultadoNumeros, CantidadMaximaDeDecimales());
                        if (EvaluarDoubleDentroRango("0", _resultadoNumeros))
                        {
                            txtOperaciones.Text = $"{_primerNumero} / {_segundoNumero}";
                            _primerNumero = _resultadoNumeros;
                        }
                    }
                    else
                    {
                        //no procede
                        LlamarError("DivEntreCero");
                        return;
                    }
                    break;

                default:
                    break;
            }

            _resultadoNumeros = Math.Round(_resultadoNumeros, 9);
            MostrarEnPantalla(_resultadoNumeros.ToString());
            _resultadoNumeros = double.NaN;
            _btnOperacionActivado = false;
            _btnIgualActivado = true;
            _repetirIgual = true;
        }

        //cuando se preciona sumar se guarda el dato actual en una variable
        private void btnSumar_Click(object sender, EventArgs e)
        {
            ComunParaOperaciones();
            _operacion = "sumar";
        }

        private void btnRestar_Click(object sender, EventArgs e)
        {
            ComunParaOperaciones();
            _operacion = "restar";
        }

        private void btnMultiplicar_Click(object sender, EventArgs e)
        {
            ComunParaOperaciones();
            _operacion = "multiplicar";
        }

        private void btnDividir_Click(object sender, EventArgs e)
        {
            ComunParaOperaciones();
            _operacion = "dividir";
        }

        private void ComunParaOperaciones()
        {
            //debe pasar cero
            SiHayErrorReiniciaVariables();

            //si se presiona, se guarda el dato actual en la variable uno
            btnretroceder.Enabled = true;
            _btnOperacionActivado = true;
            _repetirIgual = false;
            _primerNumero = ConvertirADouble(txtBox001.Text);
            _primerNumero = Math.Round(_primerNumero, 9);
            txtOperaciones.Text = $"{_primerNumero}";
        }


        private bool SaberSiHayPunto(string numeroString)
        {
            if (numeroString.IndexOf(".") == -1)
            {
                //no hay
                return false;
            }
            else
            {
                //sí hay
                return true;
            }
        }



        //forma el número, gestiona ceros, punto, signo, dimensión
        private void FormarNumero(string datoIngresado)
        {
            string datoPantalla = txtBox001.Text;
            //para un error
            if (_mostrarError == true)
            {
                ReiniciarVariables();
                datoPantalla = "0";//se forma un nuevo número
            }

            btnretroceder.Enabled = true;

            if (_btnIgualActivado == true)
            {
                //dato ingresado
                if (datoIngresado == "retroceso")
                {
                    //no hace nada, sale
                    return;
                }

                //operación presionado inmediatamente antes
                if (_btnOperacionActivado == false)
                {
                    //sin operación, debe de crear otro número
                    ReiniciarVariables();
                    datoPantalla = "0";//se forma un nuevo número
                }
                else
                {
                    //operación se acaba de presionar
                    datoPantalla = "0";//se forma un nuevo número
                    _btnOperacionActivado = false;
                    _segundoNumero = double.NaN;
                }

                _btnIgualActivado = false;//eeee
            }//igual presionado





            //operación presionado inmediatamente antes
            if (_btnOperacionActivado == true)
            {
                //operación se acaba de presionar
                datoPantalla = "0";//se forma un nuevo número
                _btnOperacionActivado = false;
                _segundoNumero = double.NaN;
            }

            if (_mostrarError == false && _btnIgualActivado == false && _btnOperacionActivado == false)
            {
                if (datoIngresado.Length > 1)
                {
                    //para retroceso o reiniciar----------------------------
                    if (datoIngresado == "retroceso")
                    {
                        if (datoPantalla.Length == 1 || datoPantalla.Length == 0)
                        {
                            //un dato
                            datoPantalla = "0";
                        }
                        else if(datoPantalla.Length > 1)
                        {
                            //más de un dato, elimina último caracter
                            datoPantalla = datoPantalla.Substring(0, datoPantalla.Length - 1);
                            if (datoPantalla == "-")
                            {
                                datoPantalla = "0";
                            }
                        }
                        FormatearParaPantalla(datoPantalla);
                        return;
                    }
                }
                else if (datoIngresado.Length == 1)
                {
                    //para los números, punto y signo-----------------------
                    //solo ingresa un punto
                    if (datoIngresado == "." && !SaberSiHayPunto(datoPantalla))
                    {
                        //no hay punto, se ingresa el punto
                        datoPantalla += datoIngresado;
                    }
                    else if (datoIngresado == "." && datoPantalla == "0")
                    {
                        //crea un nuevo dato
                        datoPantalla = "0.";
                    }

                    //cambiar de signo
                    if (datoIngresado == "-")
                    {
                        //cambiar signo
                        if (datoPantalla == "0")
                        {
                            //crea un nuevo dato
                            datoPantalla = "-0";
                        }
                        else if (datoPantalla.Length > 0 && datoPantalla[0] == '-')
                        {
                            //es negativo y cambia a positivo
                            datoPantalla = datoPantalla.Substring(1);//elimina el primer dato
                        }
                        else if(datoPantalla.Length > 0 && datoPantalla[0] != '-')
                        {
                            //es positivo y cambia a negativo
                            datoPantalla = "-" + datoPantalla;
                        }
                    }

                    //para ingresar ceros
                    if (datoIngresado == "0" && datoPantalla != "0")
                    {
                        //hay un número diferente de cero
                        if (double.TryParse(datoPantalla, out double parseado))
                        {
                            //que no tenga puros ceros enteros después del negativo
                            if (parseado != 0)
                            {
                                //que no incie con ceros en la parte entera
                                datoPantalla += "0";
                            }
                            else if (SaberSiHayPunto(datoPantalla))
                            {
                                //parte decimal
                                datoPantalla += "0";
                            }
                        }
                    }
                    else if (datoIngresado == "0" && datoPantalla == "0")
                    {
                        //hay un cero solo
                        //evita que se coloquen una sucesión de ceros como primeros elementos
                        datoPantalla = "0";
                    }
                    //todos los números menos el cero
                    if (datoIngresado != "0" && datoIngresado != "-" && datoIngresado != ".")
                    {
                        if (datoPantalla == "-0")
                        {
                            datoPantalla = "-" + datoIngresado;
                        }
                        else if (datoPantalla == "0")
                        {
                            datoPantalla = datoIngresado;
                        }
                        else
                        {
                            //todos los demás valores
                            datoPantalla += datoIngresado;
                        }
                    }
                }
            }
            _repetirIgual = false;
            FormatearParaPantalla(datoPantalla);
        }


        private void btnretroceder_Click(object sender, EventArgs e)
        {
            FormarNumero("retroceso");
        }

        //botones numéricos
        private void btn9_Click(object sender, EventArgs e)
        {
            FormarNumero("9");
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            FormarNumero("8");
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            FormarNumero("7");
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            FormarNumero("6");
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            FormarNumero("5");
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            FormarNumero("4");
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            FormarNumero("3");
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            FormarNumero("2");
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            FormarNumero("1");
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            FormarNumero("0");
        }

        private void btnPunto_Click(object sender, EventArgs e)
        {
            FormarNumero(".");
        }
        private void btnMasMenos_Click(object sender, EventArgs e)
        {
            FormarNumero("-");
        }

        private void btnBorrarTodo_Click(object sender, EventArgs e)
        {
            ReiniciarVariables();
        }

    }
}
