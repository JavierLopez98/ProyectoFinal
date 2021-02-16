using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Data;
using ProyectoFinal.Helpers;
using ProyectoFinal.Models;
using ProyectoFinal.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace ProyectoFinal.Controllers
{
    public class EquipoController : Controller
    {
        RepositoryJugadores repo;
        PathProvider provider;
        public EquipoController(RepositoryJugadores repo,PathProvider provider)
        {
            this.repo = repo;
            this.provider = provider;
        }
        public IActionResult Index(int? liga)
        {
            ViewData["Ligas"] = this.repo.GetLigas();
            ViewData["Liga"] = new Liga();
            if (liga != null)
            {
                ViewData["Liga"] = this.repo.GetLigaId(liga.Value);
                return View(this.repo.GetEquiposLiga(liga.Value));
            }
            else
            {
                return View(this.repo.GetEquipos());
            }
            
        }

        public IActionResult NuevoEquipo()
        {
            List<Liga> ligas = this.repo.GetLigas();
            ViewData["Ligas"] = ligas;
            return View();
        }
        [HttpPost]
        public async  Task<IActionResult> NuevoEquipo(String Nombre,int liga,IFormFile fotoequipo)
        {
            String filename = fotoequipo.FileName;
            String ruta = this.provider.MapPath(filename, Folders.Images);
            using (var stream = new FileStream(ruta, FileMode.Create))
            {
                await fotoequipo.CopyToAsync(stream);
            }
            this.repo.CrearEquipo(Nombre,liga, filename);
            return RedirectToAction("Index","Equipo");
        }

        public IActionResult Detalles(int id)
        {
            ViewData["Jugadores"] = this.repo.GetJugadoresEquipo(id);
            return View(this.repo.GetEquipoId(id));
        }
        public IActionResult ModificarEquipo(int id)
        {
            Equipo eq = this.repo.GetEquipoId(id);
            ViewData["Liga"] = this.repo.GetLigaId(eq.Liga);
            ViewData["Ligas"] = this.repo.GetLigas();
            return View(eq);
        }

        [HttpPost]
        public async Task<IActionResult> ModificarEquipo(int id,String Nombre,int liga, IFormFile? fotoequipo)
        {
            String filename = "";
            if (fotoequipo != null)
            {
                filename = fotoequipo.FileName;
                String ruta = this.provider.MapPath(filename, Folders.Images);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await fotoequipo.CopyToAsync(stream);


                }
                this.repo.ModificarEquipoFoto(id, Nombre, liga, filename);
            }
                
            else
                {
                    filename = "defaultuserimage.png";
                this.repo.ModificarEquipo(id, Nombre, liga);
                }

            
            return RedirectToAction("Index","Equipo");
        }

        public IActionResult EliminarEquipos(int id)
        {
            
            return View(this.repo.GetEquipoId(id));
        }

        [HttpPost]
        public IActionResult EliminarEquipos(int id,String accion)
        {
            if (accion == "Delete")
            {
                this.repo.EliminarEquipos(id);   
            }
            
            return RedirectToAction("Index", "Equipo");
        }


    }
}
