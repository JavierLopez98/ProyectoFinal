using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal.Models
{
    [Table("Equipos")]
    public class Equipo
    {
        [Key]
        public int IdEquipo { set; get; }
        public String Nombre { set; get; }
        public int Liga { get; set; }
        public String Foto { get; set; }
    }
}
