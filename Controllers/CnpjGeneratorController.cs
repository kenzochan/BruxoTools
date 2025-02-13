using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BruxoTools.Controllers
{
    public class CnpjGeneratorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Generate()
        {
            string cnpj = GenerateCnpj();
            ViewBag.GeneratedCnpj = cnpj;
            return View("Index");
        }

        private string GenerateCnpj()
        {
            Random rand = new Random();
            int[] cnpjBase = new int[12];

            for (int i = 0; i < 8; i++)
                cnpjBase[i] = rand.Next(0, 10);

            // Definir os quatro últimos dígitos fixos para empresas "matriz"
            cnpjBase[8] = 0;
            cnpjBase[9] = 0;
            cnpjBase[10] = 0;
            cnpjBase[11] = 1;

            int firstDigit = CalculateCnpjDigit(cnpjBase);
            int secondDigit = CalculateCnpjDigit(cnpjBase.Concat(new int[] { firstDigit }).ToArray());

            string cnpj = string.Join("", cnpjBase) + firstDigit + secondDigit;
            return FormatCnpj(cnpj);
        }

        private int CalculateCnpjDigit(int[] numbers)
        {
            int[] multipliers = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 6, 5, 4, 3, 2 };
            int sum = numbers.Select((num, index) => num * multipliers[index + (multipliers.Length - numbers.Length)]).Sum();
            int remainder = sum % 11;
            return remainder < 2 ? 0 : 11 - remainder;
        }

        private string FormatCnpj(string cnpj)
        {
            return Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
        }
    }
}
