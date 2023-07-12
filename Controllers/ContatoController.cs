using Microsoft.AspNetCore.Mvc;

namespace Site.Controllers
{
    public class ContatoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
