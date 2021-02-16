using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal.Models
{
    [Table("Partidos")]
    public class Partidos
    {
        [Key]
        [Column("IdPartido")]
        public int Id { get; set; }
        public int Equipo1 { get; set; }
        public int Equipo2 { get; set; }
        public int ResultadoEquipo1 { get; set; }
        public int ResultadoEquipo2 { get; set; }
        public String fecha { get; set; }
    }
}
