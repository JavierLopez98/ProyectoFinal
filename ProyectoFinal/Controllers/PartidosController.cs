using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal.Controllers
{
    public class PartidosController : Controller
    {
        public IActionResult Listado()
        {
            return View();
        }

        public IActionResult NuevoPartido()
        {
            return View();
        }

        public IActionResult ModificarPartido()
        {
            return View();
        }
        public IActionResult EliminarPartido()
        {
            return View();
        }
    }
}
