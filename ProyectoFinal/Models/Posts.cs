using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal.Models
{
    public class Posts
    {
        public int IdPost { get; set; }
        public String Titulo { get; set; }
        public String Asunto { get; set; }
        public String Descripcion { get; set; }
        public int IdPadre { get; set; }
        public int IdUsuario { get; set; }
    }
}
