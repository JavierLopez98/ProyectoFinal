using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MvcCore.Helpers;
using ProyectoFinal.Data;
using ProyectoFinal.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal.Repositories
{
    
    public class RepositoryJugadores
    {
        EquipoContext context;
        IConfiguration configuration;
        public RepositoryJugadores(EquipoContext context,IConfiguration configuration)
        {
            this.configuration = configuration;
            this.context = context;
        }
        #region Jugadores
        public List<Jugador> GetJugadores()
        {
            return this.context.Jugadores.ToList();
        }

        public List<Jugador> buscarJugadorNick(String nick)
        {
            var consulta = from datos in this.context.Jugadores where datos.Nick.StartsWith(nick) select datos;
            return consulta.ToList();
        }

        public List<Jugador> BuscaJugadorEquipo(int idequipo)
        {
            var consulta = from datos in this.context.Jugadores where datos.IdEquipo == idequipo select datos;
            return consulta.ToList();
        }
        //@* IdJugador,Nombre,Nick,Funcion,IdEquipo,Correo,Password,Foto*@
        public void CrearJugador(String Nombre,String nick,String funcion,int idEquipo,String correo, String password,String foto)
        {

            using(SqlConnection cn=new SqlConnection(this.configuration.GetConnectionString("cadenaSqlserver")))
            {
                SqlCommand com = new SqlCommand();
                com.Connection = cn;
                com.CommandText = "NuevoJugador";
                com.CommandType = System.Data.CommandType.StoredProcedure;
                String salt= CypherService.GenerateSalt();
                com.Parameters.AddWithValue("@nombre", Nombre);
                com.Parameters.AddWithValue("@Nick", nick);
                com.Parameters.AddWithValue("@funcion", funcion);
                com.Parameters.AddWithValue("idEquipo",idEquipo);
                com.Parameters.AddWithValue("@correo", correo);
                com.Parameters.AddWithValue("@salt",salt);
                com.Parameters.AddWithValue("@password", CypherService.CifrarContenido(password,salt));
                com.Parameters.AddWithValue("@foto", foto);
                cn.Open();
                com.ExecuteNonQuery();
                cn.Close();
                com.Parameters.Clear();
            }
        }
        #endregion

        #region Equipos

        public List<Equipo> GetEquipos()
        {
            return this.context.Equipos.ToList();
        }
        #endregion

        #region Ligas

        public List<Liga> GetLigas()
        {
            return this.context.Ligas.ToList();
        }

        #endregion  

    }
}
