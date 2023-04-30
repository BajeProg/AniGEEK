using AniGeekAPI;
using Microsoft.AspNetCore.Mvc;

namespace AniGEEK.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Catalog()
        {
            var client = new AniGeekClient("SecretToken");
            var products = await client.GetProductsAsync();
            if (products is null) throw new Exception("No Products");
            return View(products);
        }

        public IActionResult Main()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}
