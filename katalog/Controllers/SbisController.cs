using katalog.SbisData;
using katalog.Services.Interfaces;
using katalog.ViewModels;
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
        public async Task<IActionResult> Index(string search)
        {
            await _sbisService.Auth();
            List<Product> categories =  await _sbisService.GetCategories();
            List<Product>? products = String.IsNullOrEmpty(search) ? products = await _sbisService.GetProducts() : await _sbisService.GetProductsSearched(search);
            List<Balances> balances = await _sbisService.GetRemains(products.Select(x => x.Id).ToList());
            foreach (Product product in products)
            {
                if (balances.Where(x => x.Nomenclature == product.Id).FirstOrDefault() != null)
                {
                    product.ProdCount = balances.Where(x => x.Nomenclature == product.Id).First().Balance;
                }
            }
            var vm = new CatalogViewModel() { Categories = categories, Products = products };
            return View(vm);
        }
        [HttpGet("/{parentId}")]
        public async Task<IActionResult> Index(int parentId)
        {
            await _sbisService.Auth();
            List<Product>? categories = await _sbisService.GetCategories();
            List<Product>? products = await _sbisService.GetProducts(parentId);
            List<Balances> balances = await _sbisService.GetRemains(products.Select(x => x.Id).ToList());
            foreach (Product product in products)
            {
                if (balances.Where(x => x.Nomenclature == product.Id).FirstOrDefault() != null)
                {
                    product.ProdCount = balances.Where(x => x.Nomenclature == product.Id).First().Balance;
                }
            }
            var vm = new CatalogViewModel() { Categories = categories, Products = products };
            return View(vm);
        }
        [HttpGet("/Image")]
        public async Task<IActionResult> Image(string url)
        {
            await _sbisService.Auth();
            var response = await _sbisService.GetImage(url);
            return File(response, "image/png");
        }
    }
}
