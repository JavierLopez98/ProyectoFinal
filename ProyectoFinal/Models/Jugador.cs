using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal.Models
{
    [Table("Jugadores")]
    public class Jugador
    {
        [Key]
        public int IdJugador { get; set; }
        public String Nombre { get; set; }
        public String Nick { get; set; }
        public String Funcion { get; set; }
        public int IdEquipo { get; set; }
        public String Correo { get; set; }
        public byte[] Password { get; set; }
        public String Salt { get; set; }
        public String Foto { get; set; }
    }
}
