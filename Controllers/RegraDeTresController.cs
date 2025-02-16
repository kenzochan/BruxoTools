using System;
using Microsoft.AspNetCore.Mvc;

namespace SeuProjeto.Controllers
{
    public class RegraDeTresController : Controller
    {
        // GET: RegraDeTres
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Calcular(double valor1, double valor2, double valor3)
        {
            double resultado = (valor2 * valor3) / valor1;
            ViewBag.Resultado = resultado;
            return View("Index");
        }
    }
}
