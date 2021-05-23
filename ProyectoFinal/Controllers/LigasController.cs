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
    public class LigasController : Controller
    {
        RepositoryJugadores repo;
        ServiceEquipos service;
        

        public LigasController(RepositoryJugadores repo,ServiceEquipos service)
        {
            this.repo = repo;
            this.service = service;
        }

        public async Task<IActionResult> Index(String nombre)
        {
            if (nombre != null)
            {
                return View(await this.service.BuscarLigaNombreAsync(nombre));
            }
            return View(await this.service.GetLigasAsync());
        }

        public async Task<IActionResult> Detalles(int id)
        {
            return View(await this.service.BuscarLigaAsync(id));
        }

        public async Task<IActionResult> ModificarLiga(int id)
        {

            return View(await this.service.BuscarLigaAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> ModificarLiga(Liga lig)
        {
            await this.service.ModificarLigaAsync(lig.IdLiga, lig.Nombre, lig.Descripcion);
            
            return RedirectToAction("Index", "Ligas");
        }
        public async Task<IActionResult> NuevaLiga()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NuevaLiga(Liga lig)
        {
            await this.service.InsertarLigaAsync(lig.Nombre, lig.Descripcion);
            return RedirectToAction("Index", "Ligas");
        }
        public async Task<IActionResult> EliminarLiga(int id)
        {
            return View(await this.service.BuscarLigaAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> EliminarLiga(int id,String accion)
        {
            if (accion == "Delete")
            {
                await this.service.EliminarLigaAsync(id);
            }
            return RedirectToAction("Index", "Ligas");
        }
    }
}
