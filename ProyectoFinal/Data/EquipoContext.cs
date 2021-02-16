using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ProyectoFinal.Models;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinal.Data
{
    public class EquipoContext :DbContext
    {
       
        public EquipoContext(DbContextOptions<EquipoContext> options) : base(options) { }
        
        public DbSet<Jugador> Jugadores { get; set; }

        public DbSet<Equipo> Equipos { get; set; }

        public DbSet<Liga> Ligas { get; set; }

        public DbSet<Partidos> Partidos { get; set; }
    }
}
