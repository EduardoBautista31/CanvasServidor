using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasServidor.Models
{
    public class Rectangulo
    {
        public int Alto { get; set; }
        public int Ancho { get; set; }
        public int CordenadaX { get; set; }
        public int CordenadaY { get; set; }
        public string? Fill { get; set; }
    }
}
