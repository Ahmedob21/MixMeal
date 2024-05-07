using Microsoft.AspNetCore.Mvc;

namespace MixMeal.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
