using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vista
{
    class Respuesta
    {
        public static int EXITO = 1;
        public static int ERROR = 0;
        public int Estado { set; get; }
        public String Mensaje { get; set; }
        public object Data { set; get; }
    }
}
