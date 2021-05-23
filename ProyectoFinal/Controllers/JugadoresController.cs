using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Filters;
using ProyectoFinal.Helpers;
using ProyectoFinal.Models;
using ProyectoFinal.Repositories;
using ProyectoFinal.Services;
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
        ServiceEquipos service;
        ServiceStorageFile storageFile;
        public JugadoresController(RepositoryJugadores repo, PathProvider provider, ServiceEquipos service, ServiceStorageFile storageFile)
        {
            this.service = service;
            this.storageFile = storageFile;
            this.repo = repo;
            this.provider = provider;
        }
        public async Task<IActionResult> Index(int? pagina)
        {

            if (pagina == null)
            {
                pagina = 1;
            }
            List<Jugador> registro= await this.service.GetJugadoresAsync();
            ViewData["Equipos"] = await this.service.GetEquiposAsync();
            ViewData["registros"] = registro.Count();
            return View(await this.service.PaginarJugadores(pagina.Value-1));
            
            
        }
        [HttpPost]
        public async Task<IActionResult> Index(String nick,int?equipo)
        {
            ViewData["Equipo"] = new Equipo();
            ViewData["Equipos"] =await this.service.GetEquiposAsync();
            ViewData["registros"] = await this.service.GetJugadoresAsync();
            List<Jugador> jugadores;
           
            if (equipo != null)
            {
                ViewData["Equipo"] = await this.service.BuscarEquipoAsync(equipo.Value);
                if (nick != null)
                {
                    jugadores = await this.service.GetJugadoresEquipoAsync(nick, equipo.Value);
                }
                else
                {
                    jugadores = await this.service.GetJugadoresEquipoAsync(equipo.Value);
                }
            }
            else
            {
                if (nick != null)
                {
                    jugadores = await this.service.BuscarJugadoresNickAsync(nick);
                    
                }
                else
                {
                    jugadores = await this.service.PaginarJugadores(0);
                        if (jugadores.Count() == 0) ViewData["Mensaje"] = "No se han encontrado jugadores";
                }
                
            }
            return View(jugadores);
        }
       
        public async Task<IActionResult> Detalles(int id)
        {
            Jugador jug = await this.service.BuscarJugadorAsync(id);
            ViewData["Equipo"] = await this.service.BuscarEquipoAsync(jug.IdEquipo);
            return View(jug);
        }
        [AuthorizeUsuario]
        public async Task<IActionResult> Perfil()
        {
            String dato = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            int id = int.Parse(dato);
            Jugador jug = await this.service.BuscarJugadorAsync(id);
            ViewData["Equipo"] = await this.service.BuscarEquipoAsync(jug.IdEquipo);
            return View(jug);
        }
        public async Task<IActionResult> CambiarContraseña(int id)
        {
            ViewData["Mensaje"] = "";
            return View(await this.service.BuscarJugadorAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> CambiarContraseña(int id,String nueva,String copia)
        {
            if (nueva != copia)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return View();
            }
            //await this.service.ModificarJugador(id,nombre,nick,idequipo,correo,copia,foto);
            this.repo.CambiarContraseña(id,nueva);
            return RedirectToAction("Perfil", "Jugadores");
        }

        public async Task<IActionResult> ModificarJugador(int id)
        {
            Jugador jug = await this.service.BuscarJugadorAsync(id);
            ViewData["Equipo"] = await this.service.BuscarEquipoAsync(jug.IdEquipo);
            ViewData["Equipos"] = await this.service.GetEquiposAsync();
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

        public async Task<IActionResult> EliminarJugador(int id)
        {
            return View(await this.service.BuscarJugadorAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> EliminarJugador(int id,String accion)
        {
            if (accion == "Delete")
            {
                await this.service.EliminarEquipoAsync(id);
            }

            return RedirectToAction("Index", "Jugadores");
        }
    }
}
