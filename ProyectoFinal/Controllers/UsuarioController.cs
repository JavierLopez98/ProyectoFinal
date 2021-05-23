using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Helpers;
using ProyectoFinal.Models;
using ProyectoFinal.Repositories;
using ProyectoFinal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProyectoFinal.Controllers
{
    public class UsuarioController : Controller
    {
        RepositoryJugadores repo;
        FileUploader uploader;
        ServiceEquipos service;
        ServiceStorageFile storageFile;

        public UsuarioController(RepositoryJugadores repo, FileUploader uploader, ServiceEquipos service, ServiceStorageFile storageFile)
        {
            this.repo = repo;
            this.uploader = uploader;
            this.service = service;
            this.storageFile = storageFile;
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(String user,String password,String accion)
        {
            if (accion == "entrar")
            {
                Jugador jug = await this.service.ExisteJugador(user, password);
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
                    identidad.AddClaim(new Claim(ClaimTypes.Surname, jug.Foto));
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
                return RedirectToAction("SignUp", "Usuario");
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> SignUp()
        {
            List<Equipo>equipos=await this.service.GetEquiposAsync();
            return View(equipos);
        }
        [HttpPost]
        
        public async Task<IActionResult> SignUp(String Nombre,String Nick, int IdEquipo,String correo,String password,IFormFile Foto)
        {
            
           
                String filename = Toolkit.FilenameNormalizer(Foto.FileName);
                //String path = await this.uploader.UploadFileAsync(Foto, Folders.Images);
                using (var stream = Foto.OpenReadStream())
                {
                    await this.storageFile.UploadFile(Foto, Nombre);
                }
                await this.service.InsertarJugador(Nombre, Nick, IdEquipo, correo, password, Nombre+filename);
                this.repo.CrearJugador(Nombre, Nick, IdEquipo, correo, password, "defaultuserimage.png");
            
            Jugador jug = this.repo.ExisteJugador(Nick, password);
            ClaimsIdentity identidad = new ClaimsIdentity(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        ClaimTypes.Name, ClaimTypes.Role
                        );
            identidad.AddClaim(new Claim(ClaimTypes.NameIdentifier, jug.IdJugador.ToString()));
            identidad.AddClaim(new Claim(ClaimTypes.Name, jug.Nick));
            identidad.AddClaim(new Claim(ClaimTypes.Role, jug.Funcion));
            identidad.AddClaim(new Claim(ClaimTypes.Surname, jug.Foto));
            ClaimsPrincipal principal = new ClaimsPrincipal(identidad);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.Now.AddMinutes(30)
                });

            return RedirectToAction("Index","Home");
        }

        

    }
}
