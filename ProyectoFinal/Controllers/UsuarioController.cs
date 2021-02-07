using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Helpers;
using ProyectoFinal.Models;
using ProyectoFinal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal.Controllers
{
    public class UsuarioController : Controller
    {
        RepositoryJugadores repo;
        FileUploader uploader;

        public UsuarioController(RepositoryJugadores repo,FileUploader uploader)
        {
            this.repo = repo;
            this.uploader = uploader;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(String user,String password,String accion)
        {
            if (accion == "entrar")
            {
                return RedirectToAction("index","Home");
            }else if(accion== "Registrarse")
            {
                return RedirectToAction("SingUp");
            }
            return View();
        }

        public IActionResult SingUp()
        {
            List<Equipo>equipos=this.repo.GetEquipos();
            return View(equipos);
        }
        [HttpPost]
        // IdJugador,Nombre,Nick,Funcion,IdEquipo,Correo,Password,Foto
        public async Task<IActionResult> SingUp(String Nombre,String Nick,String Funcion, int IdEquipo,String correo,String password,IFormFile Foto)
        {
            
            if (Foto != null)
            {
                String filename = Toolkit.FilenameNormalizer(Foto.FileName);
                String path = await this.uploader.UploadFileAsync(Foto, Folders.Images);
                this.repo.CrearJugador(Nombre, Nick, Funcion, IdEquipo, correo, password, filename );
            }
            return RedirectToAction("Index","Home");
        }

    }
}
