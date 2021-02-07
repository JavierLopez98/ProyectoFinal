using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Data;
using ProyectoFinal.Models;
using ProyectoFinal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ProyectoFinal.Controllers
{
    public class EquipoController : Controller
    {
        RepositoryJugadores repo;

        public EquipoController(RepositoryJugadores repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            
            return View();
        }
         
        
    }
}
