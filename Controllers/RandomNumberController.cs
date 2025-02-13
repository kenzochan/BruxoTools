using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BruxoTools.Controllers
{
    public class RandomNumberController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Generate(int quantity, int minValue, int maxValue)
        {
            if (quantity < 1)
            {
                ViewBag.Error = "A quantidade deve ser pelo menos 1.";
                return View("Index");
            }

            if (minValue >= maxValue)
            {
                ViewBag.Error = "O valor mínimo deve ser menor que o valor máximo.";
                return View("Index");
            }

            List<int> numbers = GenerateRandomNumbers(quantity, minValue, maxValue);
            ViewBag.GeneratedNumbers = numbers;
            return View("Index");
        }

        private List<int> GenerateRandomNumbers(int quantity, int minValue, int maxValue)
        {
            Random rand = new Random();
            List<int> numbers = new List<int>();

            for (int i = 0; i < quantity; i++)
            {
                numbers.Add(rand.Next(minValue, maxValue + 1)); // Gera dentro do intervalo
            }

            return numbers;
        }
    }
}
