using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BruxoTools.Controllers
{
    public class CurrencyConverterController : Controller
    {
        private const string API_URL = "https://api.exchangerate-api.com/v4/latest/";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConvertCurrency(string fromCurrency, string toCurrency, decimal amount)
        {
            if (string.IsNullOrEmpty(fromCurrency) || string.IsNullOrEmpty(toCurrency) || amount <= 0)
            {
                ViewBag.Error = "Por favor, preencha todos os campos corretamente.";
                return View("Index");
            }

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(API_URL + fromCurrency.ToUpper());
                var rates = JObject.Parse(response)["rates"];

                if (rates != null && rates[toCurrency.ToUpper()] != null)
                {
                    decimal rate = rates[toCurrency.ToUpper()].Value<decimal>();
                    decimal convertedAmount = amount * rate;
                    ViewBag.ConvertedAmount = $"{amount} {fromCurrency.ToUpper()} = {convertedAmount:F2} {toCurrency.ToUpper()}";
                }
                else
                {
                    ViewBag.Error = "Erro ao obter taxa de câmbio.";
                }
            }

            return View("Index");
        }
    }
}
