using Microsoft.AspNetCore.Mvc;

namespace Site.Controllers
{
    public class SobreMimController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
