using AniGeekAPI;
using AniGeekAPI.Classes;
using Microsoft.AspNetCore.Mvc;

namespace AniGEEK.Controllers
{
    public static class Info
    {
        public static User? user = null;
    }
    public class HomeController : Controller
    {
        private AniGeekClient client = new AniGeekClient("SecretToken");

        #region non_logic
        public IActionResult Index() => View();

        public IActionResult Main() => View();

        public IActionResult Login() => View();

        public IActionResult Contact() => View();

        public IActionResult QnA() => View();
        #endregion

        public async Task<IActionResult> Catalog()
        {
            var products = await client.GetProductsAsync();
            if (products is null) throw new Exception("No Products");
            return View(products);
        }

        public IActionResult Profile()
        {
            int? d = HttpContext.Session.GetInt32("userid");
            if (HttpContext.Session.GetInt32("userid") == null)
                return Redirect("~/Home/Login");
            return View(Info.user);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(string login, string pass)
        {
            Info.user = await client.LoginAsync(login, pass, HttpContext.Session.Id);
            if (Info.user is null) throw new Exception("Неправильный логин или пароль");
            HttpContext.Session.SetInt32("userid", (int)Info.user.ID);
            return Redirect("~/Home/Profile");
        }

        public async Task<IActionResult> Item(int id)
        {
            var product = await client.GetProductAsync(id);
            if (product is null) throw new Exception("No product");
            return View(product);
        }

        public async Task<IActionResult> Order(int id)
        {
            var product = await client.GetProductAsync(id);
            if (product is null) throw new Exception("No product");
            if (Info.user is null) Redirect("~/Hpme/Login");
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Order(string address, int id)
        {
            var product = await client.GetProductAsync(id);
            if (product is null) throw new Exception("No product");
            if (Info.user is null) Redirect("~/Hpme/Login");
            await Info.user!.MakeOrderAsync(product, address);
            return Redirect("~/Home/Orders");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Info.user = null;
            return Redirect("~/Home/Login");
        }

        public async Task<IActionResult> Cart()
        {
            if(Info.user is null) return Redirect("~/Home/Login");
            var products = await Info.user.GetProductsFromCartAsync();
            if (products is null) throw new Exception("No Products");
            return View(products);
        }

        public async Task<IActionResult> Orders()
        {
            if (Info.user is null) return Redirect("~/Home/Login");
            var orders = await Info.user.GetOrdersAsync();
            if (orders is null) throw new Exception("No Products");
            return View(orders);
        }

        public async Task<IActionResult> Favorites()
        {
            if (Info.user is null) return Redirect("~/Home/Login");
            var products = await Info.user.GetProductsFromFavoriteAsync();
            if (products is null) throw new Exception("No Products");
            return View(products);
        }
    }
}
