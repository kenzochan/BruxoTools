using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using Tesseract;

namespace BruxoTools.Controllers
{
    public class ImageToTextController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Convert(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                ViewBag.Error = "Por favor, selecione uma imagem.";
                return View("Index");
            }

            string extractedText = ExtractTextFromImage(image);
            ViewBag.ExtractedText = extractedText;

            return View("Index");
        }

        private string ExtractTextFromImage(IFormFile image)
        {
            string extractedText = "";

            try
            {
                // Salvar a imagem temporariamente
                var filePath = Path.GetTempFileName();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                // Caminho do diretório de idiomas do Tesseract
                string tessDataPath = "C:/Program Files/Tesseract-OCR/tessdata"; // Linux
                // Se estiver no Windows, pode ser algo como: "C:\\Program Files\\Tesseract-OCR\\tessdata"

                // Instanciar o Tesseract OCR
                using (var engine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(filePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            extractedText = page.GetText();
                        }
                    }
                }

                // Remover o arquivo temporário
                System.IO.File.Delete(filePath);
            }
            catch (Exception ex)
            {
                extractedText = "Erro ao processar a imagem: " + ex.Message;
            }

            return extractedText;
        }
    }
}
