using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Helpers;
using ProyectoFinal.Models;
using ProyectoFinal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public async Task<IActionResult> Login(String user,String password,String accion)
        {
            if (accion == "entrar")
            {
                Jugador jug = this.repo.ExisteJugador(user, password);
                if (jug == null)
                {
                    ViewData["Mensaje"] = "Usuario/Password Incorrectos";
                    return View();
                }
                else
                {
                    ClaimsIdentity identidad = new ClaimsIdentity(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        ClaimTypes.Name, ClaimTypes.Role
                        );
                    identidad.AddClaim(new Claim(ClaimTypes.NameIdentifier,jug.IdJugador.ToString()));
                    identidad.AddClaim(new Claim(ClaimTypes.Name, jug.Nick));
                    identidad.AddClaim(new Claim(ClaimTypes.Role, jug.Funcion));
                    ClaimsPrincipal principal = new ClaimsPrincipal(identidad);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        principal,
                        new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.Now.AddMinutes(30)
                    }) ;
                    //String action = TempData["action"].ToString();
                    //String controller = TempData["controller"].ToString();
                    return RedirectToAction("Index", "Home"); 
                }
            }
            else
            {
                return RedirectToAction("SignUp", "Usuarios");
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult SignUp()
        {
            List<Equipo>equipos=this.repo.GetEquipos();
            return View(equipos);
        }
        [HttpPost]
        // IdJugador,Nombre,Nick,Funcion,IdEquipo,Correo,Password,Foto
        public async Task<IActionResult> SignUp(String Nombre,String Nick,String Funcion, int IdEquipo,String correo,String password,IFormFile Foto)
        {
            
            if (Foto != null)
            {
                String filename = Toolkit.FilenameNormalizer(Foto.FileName);
                String path = await this.uploader.UploadFileAsync(Foto, Folders.Images);
                this.repo.CrearJugador(Nombre, Nick, Funcion, IdEquipo, correo, password, filename );
            }
            return RedirectToAction("Index","Home");
        }

        public IActionResult AccesoDenegado()
        {
            return View();
        }

    }
}
