using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Comun
{
    public enum TipoOperacion : int
    {   
        [EnumMember(Value = "Suma")]
        Suma = 65,
        [EnumMember(Value = "Resta")]
        resta = 76,
        multiplicacion = 87,
        division = 90
    }
}
