using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Models;
using ProyectoFinal.Repositories;
using ProyectoFinal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal.Controllers
{
    public class PartidosController : Controller
    {
        RepositoryJugadores repo;
        ServiceEquipos service;
        public PartidosController(RepositoryJugadores repo,ServiceEquipos service)
        {
            this.repo = repo;
            this.service = service;
        }
        public async Task<IActionResult> Listado(int? equipo, int? pagina)
        {
            Equipo eq = new Equipo();
            List<Partidos> partidos = new List<Partidos>();
            if (pagina == null)
            {
                pagina = 1;
            }

            if (equipo != null)
            {
                eq = await this.service.BuscarEquipoAsync(equipo.Value);
                partidos = await this.service.BuscarPartidosEquiposAsync(equipo.Value);
            }
            else
            {
                partidos = await this.service.PaginarPartidosAsync(pagina.Value-1);
            }
            List<Partidos> registros= await this.service.GetPartidosAsync();
            ViewData["registros"] = registros.Count;
            ViewData["Equipo"] = eq;
            ViewData["Equipos"] = await this.service.GetEquiposAsync();
            return View(partidos);
        }

        public async Task<IActionResult> NuevoPartido()
        {
            ViewData["equipos"] = await this.service.GetEquiposAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NuevoPartido(int Equipo1,int Equipo2,int ResultadoEquipo1,int ResultadoEquipo2,DateTime fecha)
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

        public async Task<IActionResult> ModificarPartidos(int id)
        {
            ViewData["equipos"] = await this.service.GetEquiposAsync();
            return View(this.service.BuscarPartidosAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> ModificarPartidos(int id,int Equipo1, int Equipo2, int ResultadoEquipo1, int ResultadoEquipo2, DateTime fecha)
        {
            String fechapartido = fecha.ToShortDateString();
            await this.service.ModificarPartidos(id,Equipo1, Equipo2, ResultadoEquipo1, ResultadoEquipo2, fechapartido);
            return RedirectToAction("Listado", "Partidos");
        }
    }
}
