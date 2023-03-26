using Microsoft.AspNetCore.Mvc;

namespace GWTTask.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
