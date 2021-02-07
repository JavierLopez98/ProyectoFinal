using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Models;
using ProyectoFinal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public IActionResult Index()
        {
            List<Jugador> jugadores = this.repo.GetJugadores();
            return View(jugadores);
        }
        [HttpPost]
        public IActionResult Index(String nick)
        {
         
            List<Jugador> jugadores = this.repo.buscarJugadorNick(nick);
            if (jugadores.Count() == 0) ViewData["Mensaje"] = "No se han encontrado jugadores";
            return View(jugadores);
        }
    }
}
