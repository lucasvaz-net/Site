using Microsoft.AspNetCore.Mvc;

namespace Site.Controllers
{
    public class EstudosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
