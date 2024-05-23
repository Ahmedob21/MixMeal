using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MixMeal.Models;
using System.Diagnostics;

namespace MixMeal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


    

        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(ModelContext context, IWebHostEnvironment webHostEnvironment , ILogger<HomeController> logger)
        {

            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async  Task<IActionResult> Index()
        {
            var category = _context.Categories.ToList();

            //ViewBag.commonrecipes = await _context.Purchases.Include(recipe => recipe.Recipe).Include(category => category.Recipe.Category)
            //    .Include(chef => chef.Recipe.Chef) .Where(commonrecipe => commonrecipe)




            return View(category);
        }
        public async Task<IActionResult> AllCategories()
        {
            var AllCategory = await _context.Categories.ToListAsync();
            return View(AllCategory);
        }

        public async Task<IActionResult> AllRecipes()
        {
            var recipes = await _context.Recipes.Include(status =>status.Recipestatus)
                .Where(recipe => recipe.Recipestatus.Statusname =="Accepted").ToListAsync();

            ViewBag.RecipeCount =  _context.Recipes.Include(status => status.Recipestatus)
                .Where(recipe => recipe.Recipestatus.Statusname == "Accepted").Count();
            return View(recipes);
        }
        public async Task<IActionResult> RecipesByCategory(decimal id)
        {
            
            var Recipes = await _context.Recipes.Include(category => category.Category)
                .Where(Recipe => Recipe.Categoryid ==  id).ToListAsync();

            if (Recipes.Count == 0) { return RedirectToAction(nameof(EmptyByCategory)); }

            return View(Recipes);
           
        }
        public IActionResult EmptyByCategory()
        {
            return View();
        }

        public async Task<IActionResult> RecipesByChef(decimal id)
        {

            var Recipes = await _context.Recipes.Include(chef => chef.Chef)
                .Where(Recipe => Recipe.Chefid == id).ToListAsync();

            if (Recipes.Count == 0) { return RedirectToAction(nameof(EmptyByChef)); }

            return View(Recipes);

        }

      
        public IActionResult EmptyByChef()
        {
            return View();
        }


        public async Task<IActionResult> RecipeDetails(decimal id)
        {
            var recipeDetails = await _context.Recipes.Include(chef => chef.Chef)
                .Include(category => category.Category)
                .SingleOrDefaultAsync(recipe => recipe.Recipeid == id);
            return View(recipeDetails);
        }



        public async Task<IActionResult> Chefs()
        {
            var chefs = await _context.Users.Include(u => u.Role)
                .Include(status=> status.Userstatus)
                .Where(x => x.Role.Rolename == "Chef" && x.Userstatus.Statusname == "Accepted").ToListAsync();

            return View(chefs);
        }

        public async Task<IActionResult> Testimonial()
        {
            var testimonial = await _context.Testimonials.Include(user => user.Cust).Include(role =>role.Cust.Role)
                .Include(status => status.Testimonialstatus)
                .Where(statusName => statusName.Testimonialstatus.Statusname == "Accepted").ToListAsync();
            return View(testimonial);
        }

        public async Task<IActionResult> ContactUs()
        {
            return View();
        }





        [HttpGet]
        [Route("Home/search-by-name")]
        public IActionResult SearchByName()
        {
            var result = _context.Recipes
                .Include(x => x.Category)
                .Include(x => x.Chef)
                .ToList();
            return View(result);
        }
        [HttpPost]
        [Route("Home/search-by-name")]
        public IActionResult SearchByName(string? RecipeName)
        {
            var result = _context.Recipes.Include(x => x.Category).Include(x => x.Chef).Include(status => status.Recipestatus).Where(status=> status.Recipestatus.Statusname =="Accepted").ToList();
            if (string.IsNullOrEmpty(RecipeName))
            {
                return View(result);
            }
            else
            {
                result = result
                    .Where(recipenames => recipenames.Recipename.ToLower().Contains(RecipeName.ToLower()) && recipenames.Recipestatus.Statusname == "Accepted")
                    .ToList();
                return View(result);
            }

        }







        public IActionResult Privacy()
        {
            return View();
        }



        public IActionResult AccessDenied()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
