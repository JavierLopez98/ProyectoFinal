using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MvcCore.Helpers;
using ProyectoFinal.Data;
using ProyectoFinal.Helpers;
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
        private int ElementosPagina;
        public RepositoryJugadores(EquipoContext context,IConfiguration configuration)
        {
            this.configuration = configuration;
            this.context = context;
            ElementosPagina=6;
        }
        #region Jugadores
        public List<Jugador> GetJugadores()
        {
            return this.context.Jugadores.Where(x=>x.IdJugador>0).ToList();
        }

        public List<Jugador> buscarJugadorNick(String nick)
        {
            var consulta = from datos in this.context.Jugadores where datos.Nick.Contains(nick) select datos;
            return consulta.ToList();
        }
        public Jugador GetJugadorId(int id)
        {
            return this.context.Jugadores.Where(X => X.IdJugador == id).FirstOrDefault();
        }

        public List<Jugador> getJugadorNickEquipo(String nick,int equipo)
        {
            return this.context.Jugadores.Where(x => x.Nick.Contains(nick) && x.IdEquipo == equipo).ToList();
        }

        public Jugador ExisteJugador(String nick,String password)
        {
            return this.context.Jugadores.SingleOrDefault(x => x.Nick == nick);
        }
        

        public List<Jugador> GetJugadoresEquipo(int idequipo)
        {
            var consulta = from datos in this.context.Jugadores where datos.IdEquipo == idequipo select datos;
            return consulta.ToList();
        }

        public List<Jugador> PaginarJugador(int pagenumber)
        {
           
            int elementospagina = 6;
            //List<Jugador> jugadores = this.context.Jugadores.Where(x=>x.IdJugador>0).ToList()
            //  .Skip(numberOfObjectsPerPage * pagenumber)
            //  .Take(numberOfObjectsPerPage).ToList() ;

            List<Jugador> jugadores = this.context.Jugadores.Where(x => x.IdJugador > 0).Page(elementospagina, pagenumber).ToList();
            return jugadores;
        }
        #region ProceduresJugadores
//        alter procedure NuevoJugador(@Nombre nvarchar(50),@Nick nvarchar(50),@Funcion nvarchar(50),@idequipo int,@correo nvarchar(200),@Password nvarchar(50),@Foto nvarchar(100))as
//declare @maxId int
//select @maxId=max(IdJugador)+1 from Jugadores
//insert into Jugadores values(@maxId, @Nombre, @Nick, @Funcion, @idequipo, @correo, @Password, @Foto)

//go
        #endregion
        public void CrearJugador(String Nombre,String nick,int idEquipo,String correo, String password,String foto)
        {

            using(SqlConnection cn=new SqlConnection(this.configuration.GetConnectionString("cadenaSqlserver")))
            {
                SqlCommand com = new SqlCommand();
                com.Connection = cn;
                com.CommandText = "NuevoJugador";
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@nombre", Nombre);
                com.Parameters.AddWithValue("@Nick", nick);
                com.Parameters.AddWithValue("@funcion", "");
                com.Parameters.AddWithValue("idEquipo",idEquipo);
                com.Parameters.AddWithValue("@correo", correo);
                com.Parameters.AddWithValue("@password",password);
                com.Parameters.AddWithValue("@foto", foto);
                cn.Open();
                com.ExecuteNonQuery();
                cn.Close();
                com.Parameters.Clear();
            }

            

        }
        public void ModificarJugadorFoto(int id,String nick,String nombre,String Foto,String funcion, int idequipo)
        {
            Jugador jug = GetJugadorId(id);
            jug.Nombre = nombre;
            jug.Nick = nick;
            jug.Foto = Foto;
            jug.Funcion = funcion;
            jug.IdEquipo = idequipo;
            this.context.SaveChanges();
        }
        public void ModificarJugador(int id, String nick, String nombre,  String funcion, int idequipo)
        {
            Jugador jug = GetJugadorId(id);
            jug.Nombre = nombre;
            jug.Nick = nick;
            
            jug.Funcion = funcion;
            jug.IdEquipo = idequipo;
            this.context.SaveChanges();
        }
        public String CambiarContraseña(int id,String password)
        {
            Jugador jug = GetJugadorId(id);
            return "";
        }

        public void EliminarJugador(int id)
        {
            Jugador jug = GetJugadorId(id);
            this.context.Jugadores.Remove(jug);
            this.context.SaveChanges();
        }
        #endregion

        #region Equipos

        //        create procedure NuevoEquipo(@Nombre nvarchar(50),@Liga nvarchar(50),@Foto nvarchar(100))
        //as
        //declare @idliga int
        //declare @idequipo int
        //select @idliga= IdLiga from Ligas where Nombre=@Liga
        //select @idequipo=max(IdEquipo)+1 from Equipos
        //insert into Equipos values(@idequipo, @Nombre, @Liga, @Foto)

        //go

        public List<Equipo> GetEquipos()
        {
            return this.context.Equipos.ToList();
        }
        public Equipo GetEquipoId(int id)
        {
            return this.context.Equipos.Where(z => z.IdEquipo == id).FirstOrDefault();
        }


        public void CrearEquipo(String nombre,int Liga,String Foto)
        {
            using (SqlConnection cn = new SqlConnection(this.configuration.GetConnectionString("cadenaSqlserver")))
            {
                SqlCommand com = new SqlCommand();
                com.Connection = cn;
                com.CommandText = "NuevoEquipo";
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Nombre", nombre);
                com.Parameters.AddWithValue("@Liga", Liga);
                com.Parameters.AddWithValue("@Foto", Foto);
                cn.Open();
                com.ExecuteNonQuery();
                cn.Close();
                com.Parameters.Clear();
            }
        }

        public void ModificarEquipoFoto(int id, String nombre, int Liga, String foto)
        {
            Equipo eq = this.GetEquipoId(id);
            eq.Nombre = nombre;
            eq.Liga = Liga;
            eq.Foto = foto;
            this.context.SaveChanges();
        }
        public void ModificarEquipo(int id, String nombre, int Liga)
        {
            Equipo eq = this.GetEquipoId(id);
            eq.Nombre = nombre;
            eq.Liga = Liga;
            
            this.context.SaveChanges();
        }
        public void EliminarEquipos(int id)
        {
            List<Jugador> jugadores = this.GetJugadoresEquipo(id);
            foreach(Jugador jug in jugadores)
            {
                jug.IdEquipo = 0;
                this.context.SaveChanges();
            }
            Equipo eq = this.GetEquipoId(id);
            this.context.Equipos.Remove(eq);
            this.context.SaveChanges();
        }

        public List<Equipo> GetEquiposLiga(int idliga)
        {
            return this.context.Equipos.Where(z => z.Liga == idliga).ToList();
        }

        #endregion

        #region Ligas

        public List<Liga> GetLigas()
        {
            return this.context.Ligas.ToList();
        }
        public Liga GetLigaId(int id)
        {
            return this.context.Ligas.Where(x => x.IdLiga == id).FirstOrDefault();
        }
        public List<Liga> GetLigasNombre(String nombre)
        {
            return this.context.Ligas.Where(x => x.Nombre.Contains(nombre)).ToList();
        }

        #region procedure
//        create procedure NuevaLiga(@Nombre nvarchar(50),@descripcion nvarchar(max)) as
//declare @maxid int
//select @maxid=max(idliga)+1 from Ligas
//insert into Ligas values(@maxid, @Nombre, @descripcion)
//go
        #endregion
        public void NuevaLiga(String nombre,String descripcion)
        {
            using (SqlConnection cn = new SqlConnection(this.configuration.GetConnectionString("cadenaSqlserver")))
            {
                SqlCommand com = new SqlCommand();
                com.Connection = cn;
                com.CommandText = "NuevaLiga";
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Nombre", nombre);
                com.Parameters.AddWithValue("@Descripcion", descripcion);
                
                cn.Open();
                com.ExecuteNonQuery();
                cn.Close();
                com.Parameters.Clear();
            }
        }

        public void ModificarLiga(int id,String Nombre,String descripcion)
        {
            Liga lig = GetLigaId(id);
            lig.Nombre = Nombre;
            lig.Descripcion = descripcion;
            this.context.SaveChanges();
        }
        public void EliminarLiga(int id)
        {
            List<Equipo> equipos = GetEquiposLiga(id);
            foreach(Equipo eq in equipos)
            {
                eq.Liga = -1;
            }
            Liga lig = GetLigaId(id);
            this.context.Ligas.Remove(lig);
            this.context.SaveChanges();
        }

        #endregion

        #region partidos

        #region procedure

//              alter procedure NuevoPartido(@equipo1 int,@equipo2 int,@Resultado1 int,@Resultado2 int,@fecha nvarchar(50))as
//        declare @maxid int
//        select @maxid=max(IdPartido)+1 from Partidos
//insert into Partidos values(@maxid, @equipo1, @equipo2, @Resultado1, @Resultado2, @fecha)
//go

        #endregion


        public List<Partidos> GetPartidos()
        {
            return this.context.Partidos.ToList();
        }
        public Partidos GetPartidoId(int id)
        {
            return this.context.Partidos.Where(z => z.Id == id).SingleOrDefault();
        }
        public void NuevoPartido(int equipo1,int equipo2,int resultado1,int resultado2,String fecha)
        {
            using (SqlConnection cn = new SqlConnection(this.configuration.GetConnectionString("cadenaSqlserver")))
            {
                SqlCommand com = new SqlCommand();
                com.Connection = cn;
                com.CommandText = "NuevoPartido";
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@equipo1", equipo1);
                com.Parameters.AddWithValue("@equipo2", equipo2);
                com.Parameters.AddWithValue("@resultado1", resultado1);
                com.Parameters.AddWithValue("@resultado2", resultado2);
                com.Parameters.AddWithValue("@fecha", fecha);


                cn.Open();
                com.ExecuteNonQuery();
                cn.Close();
                com.Parameters.Clear();
            }
        }

        public void ModificarPartido(int id, int equipo1, int equipo2, int resultado1, int resultado2, String fecha)
        {
            Partidos game = this.GetPartidoId(id);
            game.Equipo1 = equipo1;
            game.Equipo2 = equipo2;
            game.ResultadoEquipo1 = resultado1;
            game.ResultadoEquipo2 = resultado2;
            game.fecha = fecha;
            this.context.SaveChanges();
        }

        //public void EliminarPartido(int id)
        //{
        //    Partidos game = this.GetPartidoId(id);
        //    this.context.Remove(game);
        //    this.context.SaveChanges();
        //}
        #endregion

    }
}
