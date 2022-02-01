using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Comun
{
    public class Resultado
    {
        public double Operando1 { get; set; }
        public double Operando2 { get; set; }
        public double Resultados{ get; set; }
        public TipoOperacion Operacion { get; set; }

        public override string ToString()
        {
            string operacionString = "";


            if (Operacion == TipoOperacion.Suma)
            {
                operacionString = "sumar";
            }
            else if (Operacion == TipoOperacion.resta)
            {
                operacionString = "restar";

            }
            else if (Operacion == TipoOperacion.division)
            {
                operacionString = "dividir";

            }
            else if (Operacion == TipoOperacion.multiplicacion)
            {
                operacionString = "multiplicar";

            }
            return string.Format("El resultado de {0} {1} y {2} es {3} ", operacionString, Operando1, Operando2, Resultados);

        }
    }
}
