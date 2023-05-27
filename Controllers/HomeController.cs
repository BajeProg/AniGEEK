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

        public IActionResult Contact() => View();

        public IActionResult QnA() => View();

        public IActionResult Profile()
        {
            int? d = HttpContext.Session.GetInt32("userid");
            if (HttpContext.Session.GetInt32("userid") == null)
                return Redirect("~/Home/Login");
            return View();
        }
    }
}
