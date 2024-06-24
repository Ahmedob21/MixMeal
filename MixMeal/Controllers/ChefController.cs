using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MixMeal.customAuth;
using MixMeal.Models;

namespace MixMeal.Controllers
{
    [CustomAuthorize(2)] // Chef
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

        
        public async Task<IActionResult> MyRecipes()
        {
            var ChefId = HttpContext.Session.GetInt32("chefSession");
            if (ChefId == null)
            {
                return NotFound();
            }
            var recipes = await _context.Recipes
                .Include(chef => chef.Chef)
                .Include(category => category.Category)
                .Include(status => status.Recipestatus)
                .Where(recipe => recipe.Chefid == ChefId).ToListAsync();

            if (recipes == null)
            {
                return RedirectToAction(nameof(Empty));
            }

            return View(recipes);
        }


        [HttpGet]
        [Route("Chef/search-by-name")]
        public IActionResult SearchByName()
        {
            var result = GetRecipesByChef();
            return View(result);
        }

        [HttpPost]
        [Route("Chef/search-by-name")]
        public async Task<IActionResult> SearchByName(string? RecipeName)
        {
            var result = GetRecipesByChef();

            if (!string.IsNullOrEmpty(RecipeName))
            {
                RecipeName = RecipeName.ToLower();
                result = _context.Recipes
                    .Where(recipe => recipe.Recipename.ToLower().Contains(RecipeName))
                    .ToListAsync();
            }

            return View(await result);
        }

        private   async Task<List<Recipe>> GetRecipesByChef()
        {
            var chefid = HttpContext.Session.GetInt32("chefSession");
            if (chefid == null)
            {
                return new List<Recipe>(); 
            }

            return await _context.Recipes
                .Include(x => x.Category)
                .Include(x => x.Recipestatus)
                .Include(x => x.Chef)
                .Where(recipe => recipe.Chefid == chefid)
                .ToListAsync();
        }

        public async Task<IActionResult> MySales(decimal id)
        {
            var chefid = HttpContext.Session.GetInt32("chefSession");
            if (chefid == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if(chefid != id)
            {
                return RedirectToAction("AccessDenied" , "Account");
            }


            var MySales = await _context.Purchases
                .Include(user => user.Customer)
                .Include(recipe => recipe.Recipe)
                .Include(chef => chef.Recipe.Chef)
                .Include(category=> category.Recipe.Category)
                .Where(purchases => purchases.Recipe.Chefid == chefid).OrderBy(date =>date.Purchasedate).ToListAsync();
                
            return View(MySales);
        }


        public IActionResult Empty()
        {
            return View();
        }


        public IActionResult PendigChef() { return View(); }




        public IActionResult RejectedChef() { return View(); }



    }
}
