using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Data;
using ProyectoFinal.Helpers;
using ProyectoFinal.Models;
using ProyectoFinal.Repositories;
using ProyectoFinal.Services;
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
        
        ServiceStorageFile storageFile;
        ServiceEquipos service;
        public EquipoController(RepositoryJugadores repo,ServiceStorageFile storageFile,ServiceEquipos service)
        {
            this.repo = repo;
            
            this.service = service;
            this.storageFile = storageFile;
        }
        public async Task<IActionResult> Index(int? liga)
        {
            ViewData["Ligas"] = await this.service.GetLigasAsync();
            ViewData["Liga"] = new Liga();
            if (liga != null)
            {
                ViewData["Liga"] = await this.service.BuscarLigaAsync(liga.Value);
                return View(await this.service.GetEquiposAsync());
            }
            else
            {
                return View(this.repo.GetEquipos());
            }
            
        }

        public async Task<IActionResult> NuevoEquipo()
        {
            List<Liga> ligas = await this.service.GetLigasAsync();
            ViewData["Ligas"] = ligas;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NuevoEquipo(String Nombre,int liga,IFormFile fotoequipo)
        {
            
            String filename = Toolkit.FilenameNormalizer(fotoequipo.FileName);
            
            using (var stream = fotoequipo.OpenReadStream())
            {
                await this.storageFile.UploadFile(fotoequipo, Nombre);
            }
            await this.service.InsertarEquipo(Nombre,liga, Nombre+filename);
            return RedirectToAction("Index","Equipo");
        }


        public async Task<IActionResult> Detalles(int id)
        {
            ViewData["Jugadores"] = await this.service.GetJugadoresEquipoAsync(id);
            return View(await this.service.BuscarEquipoAsync(id));
        }
        public async Task<IActionResult> ModificarEquipo(int id)
        {
            Equipo eq = await this.service.BuscarEquipoAsync(id);
            ViewData["Liga"] = await this.service.BuscarLigaAsync(eq.Liga);
            ViewData["Ligas"] = await this.service.GetLigasAsync();
            return View(eq);
        }

        [HttpPost]
        public async Task<IActionResult> ModificarEquipo(int id,String Nombre,int liga, IFormFile? fotoequipo)
        {
            String filename = fotoequipo.Name;
            if (this.storageFile.GetFile(filename) != null)
            {
                await this.storageFile.DeleteFile(filename);
            }            
            using (var stream = fotoequipo.OpenReadStream())
            {
                await this.storageFile.UploadFile(fotoequipo, filename);
            }
            await this.service.ModificarEquipo(id, Nombre, liga, filename);
            return RedirectToAction("Index","Equipo");
        }

        public async Task<IActionResult> EliminarEquipos(int id)
        {
            
            return View(await this.service.BuscarEquipoAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> EliminarEquipos(int id,String accion)
        {
            if (accion == "Delete")
            {
                await this.service.EliminarEquipoAsync(id);
            }
            
            return RedirectToAction("Index", "Equipo");
        }


    }
}
