using ParsePdfApp.Services;
using Microsoft.AspNetCore.Mvc;
using ParsePdfApp.Dtos;
using ParsePdfApp.Utils;
using ParsePdfApp.Models;

namespace ParsePdfApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly CategoryService _categoryService;

        public HomeController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet()]
        public IActionResult CreatePurchaseGet()
        {
            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string downloadsPath = Path.Combine(userProfile, "Downloads", "Purchase.pdf");

            string text = PdfParserPdfPig.ParsePdf(downloadsPath);
            List<Product> products = PdfParserPdfPig.ParseProducts(text);

            foreach (Product product in products)
            {
                Console.WriteLine(product.Code);
            }

            return Ok("Get PDF parsed successfully.");
        }

        [HttpPost()]
        public IActionResult CreatePurchasePost([FromBody] FileDto fileDto)
        {
            string text = PdfParserPdfPig.DecodePDF(fileDto.File);
            List<Product> products = PdfParserPdfPig.ParseProducts(text);

            foreach (Product product in products)
            {
                Console.WriteLine(product.Code);
            }

            return Ok("Post PDF parsed successfully.");
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { Message = "Test call successfull" });
        }

        [HttpGet("category")]
        public ActionResult<List<Category>> GetCategories()
        {
            List<Category> categories = _categoryService.getCategoryList();
            return Ok(categories);
        }
    }
}
