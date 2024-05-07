using Microsoft.AspNetCore.Mvc;

namespace MixMeal.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
