using Microsoft.AspNetCore.Mvc;

namespace Site.Controllers
{
    public class ProjetosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
