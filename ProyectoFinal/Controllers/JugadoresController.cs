using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Filters;
using ProyectoFinal.Helpers;
using ProyectoFinal.Models;
using ProyectoFinal.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProyectoFinal.Controllers
{
    public class JugadoresController : Controller
    {
        RepositoryJugadores repo;
        PathProvider provider;
        public JugadoresController(RepositoryJugadores repo,PathProvider provider)
        {
            this.repo = repo;
            this.provider = provider;
        }
        public IActionResult Index(int? pagina)
        {

            if (pagina == null)
            {
                pagina = 1;
            }
            
            ViewData["Equipos"] = this.repo.GetEquipos();
            ViewData["registros"] = this.repo.GetJugadores().Count;
            return View(this.repo.PaginarJugador(pagina.Value-1));
            
            
        }
        [HttpPost]
        public IActionResult Index(String nick,int?equipo)
        {
            ViewData["Equipo"] = new Equipo();
            ViewData["Equipos"] = this.repo.GetEquipos();
            ViewData["registros"] = this.repo.GetJugadores().Count;
            List<Jugador> jugadores;
           
            if (equipo != null)
            {
                ViewData["Equipo"] = this.repo.GetEquipoId(equipo.Value);
                if (nick != null)
                {
                    jugadores = this.repo.getJugadorNickEquipo(nick, equipo.Value);
                }
                else
                {
                    jugadores = this.repo.GetJugadoresEquipo(equipo.Value);
                }
            }
            else
            {
                if (nick != null)
                {
                    jugadores = this.repo.buscarJugadorNick(nick);
                    
                }
                else
                {
                    jugadores = this.repo.PaginarJugador(0);
                        if (jugadores.Count() == 0) ViewData["Mensaje"] = "No se han encontrado jugadores";
                }
                
            }
            return View(jugadores);
        }
       
        public IActionResult Detalles(int id)
        {
            Jugador jug = this.repo.GetJugadorId(id);
            ViewData["Equipo"] = this.repo.GetEquipoId(jug.IdEquipo);
            return View(jug);
        }
        [AuthorizeUsuario]
        public IActionResult Perfil()
        {
            String dato = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            int id = int.Parse(dato);
            Jugador jug = this.repo.GetJugadorId(id);
            ViewData["Equipo"] = this.repo.GetEquipoId(jug.IdEquipo);
            return View(jug);
        }
        public IActionResult CambiarContraseña(int id)
        {
            ViewData["Mensaje"] = "";
            return View(this.repo.GetJugadorId(id));
        }
        [HttpPost]
        public IActionResult CambiarContraseña(int id,String nueva,String copia)
        {
            if (nueva != copia)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return View();
            }
            this.repo.CambiarContraseña(id,nueva);
            return RedirectToAction("Perfil", "Jugadores");
        }

        public IActionResult ModificarJugador(int id)
        {
            Jugador jug = this.repo.GetJugadorId(id);
            ViewData["Equipo"] = this.repo.GetEquipoId(jug.IdEquipo);
            ViewData["Equipos"] = this.repo.GetEquipos();
            return View(jug);
        }
        [HttpPost]
        public async Task<IActionResult> ModificarJugador(int id,String nombre,String nick,String Funcion,int equipo,IFormFile fotojugador)
        {
            
            if (fotojugador != null)
            {
                String filename = Toolkit.FilenameNormalizer(fotojugador.FileName);
                String ruta = this.provider.MapPath(filename, Folders.Images);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await fotojugador.CopyToAsync(stream);


                }
                this.repo.ModificarJugadorFoto(id, nick, nombre, Funcion,filename, equipo);
            }
            else
            {
                this.repo.ModificarJugador(id, nick, nombre, Funcion, equipo);

            }
            return RedirectToAction("Index", "Jugadores");
        }

        public IActionResult EliminarJugador(int id)
        {
            return View(this.repo.GetJugadorId(id));
        }

        [HttpPost]
        public IActionResult EliminarJugador(int id,String accion)
        {
            if (accion == "Delete")
            {
                this.repo.EliminarJugador(id);
            }

            return RedirectToAction("Index", "Jugadores");
        }
    }
}
