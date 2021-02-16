using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Models;
using ProyectoFinal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal.Controllers
{
    public class LigasController : Controller
    {
        RepositoryJugadores repo;

        public LigasController(RepositoryJugadores repo)
        {
            this.repo = repo;
        }

        public IActionResult Index(String nombre)
        {
            if (nombre != null)
            {
                return View(this.repo.GetLigasNombre(nombre));
            }
            return View(this.repo.GetLigas());
        }

        public IActionResult ModificarLiga(int id)
        {

            return View(this.repo.GetLigaId(id));
        }
        [HttpPost]
        public IActionResult ModificarLiga(Liga lig)
        {
            this.repo.ModificarLiga(lig.IdLiga, lig.Nombre, lig.Descripcion);
            return RedirectToAction("Index", "Ligas");
        }
    }
}
