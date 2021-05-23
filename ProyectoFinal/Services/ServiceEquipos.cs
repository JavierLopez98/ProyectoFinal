using Newtonsoft.Json;
using ProyectoFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Services
{
    public class ServiceEquipos
    {
        private Uri Uriapi;
        private MediaTypeWithQualityHeaderValue header;
        public ServiceEquipos()
        {
            this.Uriapi = new Uri("http://54.227.96.93/");
            this.header = new MediaTypeWithQualityHeaderValue("application/json");
        }
        #region Jugador

        public async Task<List<Jugador>> GetJugadoresAsync()
        {
            String request = "/api/Jugador";
            return await this.CallApi<List<Jugador>>(request);
        }
        public async Task<Jugador> BuscarJugadorAsync(int id)
        {
            String request = "/api/Jugador/"+id;
            return await this.CallApi<Jugador>(request);
        }
        public async Task<List<Jugador>> BuscarJugadoresNickAsync(String nick)
        {
            String request = "/api/Jugador/JugadorNick/"+nick;
            return await this.CallApi<List<Jugador>>(request);
        }

        public async Task<List<Jugador>> PaginarJugadores(int pagina)
        {
            String request = "/api/Jugador/PaginarJugadores/" + pagina;
            return await this.CallApi<List<Jugador>>(request);
        }
        public async Task<List<Jugador>> GetJugadoresEquipoAsync(String nick,int equipo)
        {
            String request= "/api/Jugador/JugadorNickEquipo/" + nick + "/"+equipo;
            return await this.CallApi<List<Jugador>>(request);
        }

        public  async Task<List<Jugador>> GetJugadoresEquipoAsync(int idequipo)
        {
            String request = "/api/Jugador/JugadoresEquipo/"+idequipo;
            return await this.CallApi<List<Jugador>>(request);
        }
        public async Task InsertarJugador(String nombre,String nick,int idequipo,String correo,String password,String foto)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Jugador";
                client.BaseAddress = this.Uriapi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                Jugador jug = new Jugador();
                jug.IdJugador = 0;
                jug.Nombre = nombre;
                jug.Nick = nick;
                jug.IdEquipo = idequipo;
                jug.Funcion = "";
                jug.Correo = correo;
                jug.Password = password;
                jug.Foto = foto;
                String json = JsonConvert.SerializeObject(jug);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync(request, content);
            }
        }
        public async Task ModificarJugador(int id, String nombre, String nick, int idequipo, String correo, String password, String foto)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Jugador";
                client.BaseAddress = this.Uriapi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                Jugador jug = new Jugador();
                jug.IdJugador = id;
                jug.Nombre = nombre;
                jug.Nick = nick;
                jug.IdEquipo = idequipo;
                jug.Funcion = "";
                jug.Correo = correo;
                jug.Password = password;
                jug.Foto = foto;
                String json = JsonConvert.SerializeObject(jug);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PutAsync(request, content);
            }
        }
        public async Task EliminarJugador(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Jugador/" + id;
                client.BaseAddress = this.Uriapi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                await client.DeleteAsync(request);
            }
        }
        public async Task<Jugador> ExisteJugador(String nick, String password)
        {
            String request = "/api/Jugador/ExisteJugador/" + nick + "/" + password;
            return await this.CallApi<Jugador>(request);
        }
        #endregion
        #region Equipos
        public async Task<List<Equipo>> GetEquiposAsync()
        {
            String request = "/api/Equipo";
            return await this.CallApi<List<Equipo>>(request);
        }
        public async Task<Equipo> BuscarEquipoAsync(int id)
        {
            String request = "/api/Equipo/" + id;
            return await this.CallApi<Equipo>(request);
        }
        public async Task<List<Equipo>>BuscarEquiposLigasAsync(int idliga)
        {
            String request = "/api/Equipo/BuscarEquiposLiga/" + idliga;
            return await this.CallApi<List<Equipo>>(request);
        }

        
        public async Task InsertarEquipo(String nombre,int liga,String foto)
        {
            using (HttpClient client=new HttpClient())
            {
                String request = "/api/Equipo";
                client.BaseAddress = this.Uriapi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                Equipo eq = new Equipo();
                eq.IdEquipo = 0;
                eq.Nombre = nombre;
                eq.Liga = liga;
                eq.Foto = foto;
                String json = JsonConvert.SerializeObject(eq);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync(request, content);
            }
        }
        public async Task ModificarEquipo(int id,String nombre,int liga,String foto)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Equipo";
                client.BaseAddress = this.Uriapi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                Equipo eq = new Equipo();
                eq.IdEquipo = id;
                eq.Nombre = nombre;
                eq.Liga = liga;
                eq.Foto = foto;
                String json = JsonConvert.SerializeObject(eq);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PutAsync(request, content);
            }
        }
        public async Task EliminarEquipoAsync(int id)
        {
            using (HttpClient client=new HttpClient())
            {
                String request = "/api/Equipo/" + id;
                client.BaseAddress = this.Uriapi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                await client.DeleteAsync(request);
            }
        }
        #endregion
        #region Partidos

        public async Task<List<Partidos>> GetPartidosAsync()
        {
            String request = "/api/Partidos";
            return await this.CallApi<List<Partidos>>(request);
        }
        public async Task<Partidos> BuscarPartidosAsync(int id)
        {
            String request = "/api/Partidos/" + id;
            return await this.CallApi<Partidos>(request);
        }
        public async Task<List<Partidos>> PaginarPartidosAsync(int pagina)
        {
            String request = "/api/Partidos/PaginarPartidos/" + pagina;
            return await this.CallApi<List<Partidos>>(request);
        }
        public async Task<List<Partidos>> BuscarPartidosEquiposAsync(int idequipo)
        {
            String request = "/api/Partidos/PartidosEquipo/" + idequipo;
            return await this.CallApi<List<Partidos>>(request);
        }

        public async Task InsertarPartidos(int equipo1,int equipo2, int resultado1, int resultado2, String fecha)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Partidos";
                client.BaseAddress = this.Uriapi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                Partidos p = new Partidos();
                p.Id = 0;
                p.Equipo1 = equipo1;
                p.Equipo2 = equipo2;
                p.ResultadoEquipo1 = resultado1;
                p.ResultadoEquipo2 = resultado2;
                p.fecha = fecha;
                String json = JsonConvert.SerializeObject(p);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync(request, content);
            }
        }
        public async Task ModificarPartidos(int id,int equipo1, int equipo2, int resultado1, int resultado2, String fecha)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Partidos";
                client.BaseAddress = this.Uriapi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                Partidos p = new Partidos();
                p.Id = id;
                p.Equipo1 = equipo1;
                p.Equipo2 = equipo2;
                p.ResultadoEquipo1 = resultado1;
                p.ResultadoEquipo2 = resultado2;
                p.fecha = fecha;
                String json = JsonConvert.SerializeObject(p);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PutAsync(request, content);
            }
        }
        #endregion
        #region Ligas
        public async Task<List<Liga>> GetLigasAsync()
        {
            String request = "/api/Liga";
            return await this.CallApi<List<Liga>>(request);
        }
        public async Task<Liga> BuscarLigaAsync(int id)
        {
            String request = "/api/Equipo/" + id;
            return await this.CallApi<Liga>(request);
        }
        public async Task<Liga> BuscarLigaNombreAsync(String nombre)
        {
            String request = "/api/Liga/BuscarLigasNombre/" + nombre;
            return await this.CallApi<Liga>(request);
        }
        public async Task<List<Liga>> BuscarEquiposLigasAsync(String nombre)
        {
            String request = "/api/Liga/BuscarLigasNombre/" + nombre;
            return await this.CallApi<List<Liga>>(request);
        }

        public async Task InsertarLigaAsync( String nombre, String descripcion)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Liga";
                client.BaseAddress = this.Uriapi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                Liga lig = new Liga();
                lig.IdLiga = 0;
                lig.Nombre = nombre;
                lig.Descripcion = descripcion;
                String json = JsonConvert.SerializeObject(lig);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync(request, content);
            }
        }
        public async Task ModificarLigaAsync(int id, String nombre,  String descripcion)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Liga";
                client.BaseAddress = this.Uriapi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                Liga lig = new Liga();
                lig.IdLiga = id;
                lig.Nombre = nombre;
                lig.Descripcion = descripcion;
                String json = JsonConvert.SerializeObject(lig);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PutAsync(request, content);
            }
        }
        public async Task EliminarLigaAsync(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Liga/" + id;
                client.BaseAddress = this.Uriapi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                await client.DeleteAsync(request);
            }
        }

        #endregion
        #region Posts

        public async Task<List<Posts>> GetPostsAsync()
        {
            String request = "/api/Posts";
            return await this.CallApi<List<Posts>>(request);
        }

        public async Task InsertarPost(String nombre, String descripcion)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Liga";
                client.BaseAddress = this.Uriapi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                Liga lig = new Liga();
                lig.IdLiga = 0;
                lig.Nombre = nombre;
                lig.Descripcion = descripcion;
                String json = JsonConvert.SerializeObject(lig);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync(request, content);
            }
        }
        public async Task ModificarPost(int id, String nombre, String descripcion)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Liga";
                client.BaseAddress = this.Uriapi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                Liga lig = new Liga();
                lig.IdLiga = id;
                lig.Nombre = nombre;
                lig.Descripcion = descripcion;
                String json = JsonConvert.SerializeObject(lig);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PutAsync(request, content);
            }
        }
        public async Task EliminarPost(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Liga/" + id;
                client.BaseAddress = this.Uriapi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                await client.DeleteAsync(request);
            }
        }
        #endregion


        private async Task<T> CallApi<T>(String request)
        {
            using(HttpClient client=new HttpClient())
            {
                client.BaseAddress = this.Uriapi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                HttpResponseMessage response = await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }
    }
}
