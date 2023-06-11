using AniGeekAPI;
using Microsoft.AspNetCore.Mvc;

namespace AniGEEK.Controllers
{
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Profile(string login, string pass)
        {
            var user = await client.LoginAsync(login, pass, HttpContext.Session.Id);
            if (user is null) throw new Exception("Неправильный логин или пароль");
            HttpContext.Session.SetInt32("userid", (int)user.ID);
            return Redirect("~/Home/Profile");
        }

        public async Task<IActionResult> Item(int id)
        {
            var product = await client.GetProductAsync(id);
            if (product is null) throw new Exception("No product");
            return View(product);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("`/Home/Login");
        }
    }
}
