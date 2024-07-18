using katalog.SbisData;
using katalog.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace katalog.Controllers
{
    public class SbisController : Controller
    {
        private readonly ISbisService _sbisService;
        public SbisController(ISbisService sbisService)
        {
            _sbisService = sbisService;
        }
        public async Task<IActionResult> Index()
        {
            await _sbisService.Auth();
            List<Product> products =  await _sbisService.GetCategories();
            foreach (var product in products)
            {
                Console.WriteLine(product.Name);
            }
            return View();
        }
    }
}
