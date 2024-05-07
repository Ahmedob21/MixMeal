using Microsoft.AspNetCore.Mvc;
using MixMeal.Models;

namespace MixMeal.Controllers
{
    public class ChefController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ChefController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {

            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateRecipe()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRecipe(Recipe recipe)
        {
            var chefid = (decimal) HttpContext.Session.GetInt32("chefSession");

            if(chefid == null)
            {
                return View();

            }

            if(ModelState.IsValid) { 
               
                 recipe.Chefid = chefid;
                 await _context.AddAsync(recipe);
                 await _context.SaveChangesAsync();



                return RedirectToAction("index" , "Chef");
            }

                return View(recipe);
        }
    }
}
