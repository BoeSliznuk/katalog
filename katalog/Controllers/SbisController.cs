using katalog.Models;
using katalog.SbisData;
using katalog.Services.Interfaces;
using katalog.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var cart = string.IsNullOrEmpty(HttpContext.Session.GetString("cart")) ? new Cart() : JsonSerializer.Deserialize<Cart>(HttpContext.Session.GetString("cart"));
            if(cart.Products.Where(x=> x.Id == productId).Count() > 0)
            {
                cart.Products.First(x => x.Id == productId).ProdCount += quantity;
            }
            else
            {
                List<Product>? products = await _sbisService.GetProducts();
                var addedProduct = products.Where(x => x.Id == productId).First();
                addedProduct.ProdCount = quantity;
                cart.Products.Add(addedProduct);
            }
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart));
            return RedirectToAction("Cart");
        }
        [HttpGet("/Сart")]
        public async Task<IActionResult> Cart()
        {
            var cart = string.IsNullOrEmpty(HttpContext.Session.GetString("cart")) ? new Cart() : JsonSerializer.Deserialize<Cart>(HttpContext.Session.GetString("cart"));
            return View(cart);
        }
        [HttpPost]
        public async Task<IActionResult> Order(string name, string phone)
        {
            var cart = string.IsNullOrEmpty(HttpContext.Session.GetString("cart")) ? new Cart() : JsonSerializer.Deserialize<Cart>(HttpContext.Session.GetString("cart"));
            var Order = new Order()
            {
                Name = name,
                Phone = phone,
                Cart = cart
            };
            string output = JsonSerializer.Serialize(Order);
            var rand = new Random();
            System.IO.File.WriteAllText(Directory.GetCurrentDirectory() + "/Orders/"+ "order" + rand.Next() + ".json", output);
            cart = new Cart();
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart));
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteFromCart(int productId)
        {
            Console.WriteLine(productId);
            var cart = string.IsNullOrEmpty(HttpContext.Session.GetString("cart")) ? new Cart() : JsonSerializer.Deserialize<Cart>(HttpContext.Session.GetString("cart"));
            cart.Products = cart.Products.Where(x => x.Id != productId).ToList();
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart));
            return RedirectToAction("Cart");
        }
    }
}
