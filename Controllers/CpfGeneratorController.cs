using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BruxoTools.Controllers
{
    public class CpfGeneratorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Generate()
        {
            string cpf = GenerateCpf();
            ViewBag.GeneratedCpf = cpf;
            return View("Index");
        }

        private string GenerateCpf()
        {
            Random rand = new Random();
            int[] cpfBase = new int[9];

            for (int i = 0; i < 9; i++)
                cpfBase[i] = rand.Next(0, 10);

            int firstDigit = CalculateCpfDigit(cpfBase);
            int secondDigit = CalculateCpfDigit(cpfBase.Concat(new int[] { firstDigit }).ToArray());

            string cpf = string.Join("", cpfBase) + firstDigit + secondDigit;
            return FormatCpf(cpf);
        }

        private int CalculateCpfDigit(int[] numbers)
        {
            int sum = numbers.Select((num, index) => num * (numbers.Length + 1 - index)).Sum();
            int remainder = sum % 11;
            return remainder < 2 ? 0 : 11 - remainder;
        }
        private string FormatCpf(string cpf)
        {
            return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
        }
    }
}
