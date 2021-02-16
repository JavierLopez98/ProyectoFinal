using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Models;
using ProyectoFinal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal.Controllers
{
    public class PartidosController : Controller
    {
        RepositoryJugadores repo;
        public PartidosController(RepositoryJugadores repo)
        {
            this.repo = repo;
        }
        public IActionResult Listado()
        {
            ViewData["Equipos"] = this.repo.GetEquipos();
            return View(this.repo.GetPartidos());
        }

        public IActionResult NuevoPartido()
        {
            ViewData["equipos"] = this.repo.GetEquipos();
            return View();
        }
        [HttpPost]
        public IActionResult NuevoPartido(int Equipo1,int Equipo2,int ResultadoEquipo1,int ResultadoEquipo2,DateTime fecha)
        {
            if (Equipo1 == Equipo2)
            {
                ViewData["Mensaje"] = "Un equipo no puede enfrentarse a si mismo";
                return View();
            }
            String fechapartido = fecha.ToShortDateString();
            this.repo.NuevoPartido(Equipo1, Equipo2, ResultadoEquipo1, ResultadoEquipo2, fechapartido);
            return RedirectToAction("Listado", "Partidos");
        }

        public IActionResult ModificarPartidos(int id)
        {
            ViewData["equipos"] = this.repo.GetEquipos();
            return View(this.repo.GetPartidoId(id));
        }
        [HttpPost]
        public IActionResult ModificarPartidos(int id,int Equipo1, int Equipo2, int ResultadoEquipo1, int ResultadoEquipo2, DateTime fecha)
        {
            String fechapartido = fecha.ToShortDateString();
            this.repo.ModificarPartido(id,Equipo1, Equipo2, ResultadoEquipo1, ResultadoEquipo2, fechapartido);
            return RedirectToAction("Listado", "Partidos");
        }
    }
}
