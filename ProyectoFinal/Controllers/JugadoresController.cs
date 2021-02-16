using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Filters;
using ProyectoFinal.Models;
using ProyectoFinal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProyectoFinal.Controllers
{
    public class JugadoresController : Controller
    {
        RepositoryJugadores repo;
        public JugadoresController(RepositoryJugadores repo)
        {
            this.repo = repo;
        }
        public IActionResult Index(int? equipo)
        {
            ViewData["Equipos"] = this.repo.GetEquipos();
            if (equipo != null)
            {
                return View(this.repo.GetJugadoresEquipo(equipo.Value));
            }
            else
            {
                return View(this.repo.GetJugadores());
            }
            
        }
        [HttpPost]
        public IActionResult Index(String nick,int?equipo)
        {
            ViewData["Equipo"] = new Equipo();
            ViewData["Equipos"] = this.repo.GetEquipos();
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
                    jugadores = this.repo.GetJugadores();
                        if (jugadores.Count() == 0) ViewData["Mensaje"] = "No se han encontrado jugadores";
                }
                
            }
            return View(jugadores);
        }
       
        public IActionResult Detalles(int id)
        {
            return View(this.repo.GetJugadorId(id));
        }
        [AuthorizeUsuario]
        public IActionResult Perfil()
        {
            String dato = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            int id = int.Parse(dato);
            Jugador jug = this.repo.GetJugadorId(id);
            return View(jug);
        }

        public IActionResult ModificarJugador(int id)
        {
            Jugador jug = this.repo.GetJugadorId(id);
            ViewData["Equipo"] = this.repo.GetEquipoId(jug.IdEquipo);
            ViewData["Equipos"] = this.repo.GetEquipos();
            return View(jug);
        }
        [HttpPost]
        public IActionResult ModificarJugador()
        {
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
