using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System;
using System.IO;

namespace BruxoTools.Controllers
{
    public class QrCodeGeneratorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Generate(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                ViewBag.Error = "Digite um texto para gerar o QR Code.";
                return View("Index");
            }

            string qrCodeImage = GenerateQrCode(text);
            ViewBag.QrCodeImage = qrCodeImage;

            return View("Index");
        }

        private string GenerateQrCode(string text)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
                PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
                byte[] qrCodeBytes = qrCode.GetGraphic(20);

                return "data:image/png;base64," + Convert.ToBase64String(qrCodeBytes);
            }
        }

        [HttpGet]
        public IActionResult DownloadQrCode(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return BadRequest("Texto inválido para gerar o QR Code.");
            }

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
                PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
                byte[] qrCodeBytes = qrCode.GetGraphic(20);

                return File(qrCodeBytes, "image/png", "qrcode.png");
            }
        }
    }
}