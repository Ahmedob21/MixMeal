using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MixMeal.Models;
using MixMeal.PDFGenerator;
using MixMeal.EmailSender;
using MixMeal.customAuth;

namespace MixMeal.Controllers
{
    
    public class RecipesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RecipesController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Recipes
        
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Recipes.Include(r => r.Category).Include(r => r.Chef).Include(r => r.Recipestatus)
                .Where(display => display.Chef.Userstatus.Statusname == "Accepted" && display.Recipestatus.Statusname == "Accepted");
            return View(await modelContext.ToListAsync());
        }

        // GET: Recipes/Details/5
       
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.Chef)
                .Include(r => r.Recipestatus)
                .FirstOrDefaultAsync(m => m.Recipeid == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: Recipes/Create
        
        public async Task<IActionResult> Create()
        {
            var ChefId = HttpContext.Session.GetInt32("chefSession");
            var chef =await _context.Users.Include(status => status.Userstatus)
                .SingleOrDefaultAsync(Chef => Chef.Userid ==  ChefId);
            if (chef == null || chef.Userstatus == null)
            {
                return RedirectToAction("Login" , "Account");
            }
            if (chef.Userstatus.Statusname == "Accepted")
            {

                ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "Categoryname");
                ViewData["Chefid"] = new SelectList(_context.Users, "Userid", "Userid");
                ViewData["Recipestatusid"] = new SelectList(_context.Statuses, "Statusid", "Statusid");
                return View();
            }
            else if (chef.Userstatus.Statusname == "Pending")
            {

                return RedirectToAction("PendigChef" , "Chef");
            }
            else { return RedirectToAction("RejectedChef" , "Chef"); }
                
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create([Bind("Recipeid,Recipename,ImageFile,Publishdate,Price,Categoryid,Chefid,Recipestatusid,Recipedescription,Ingredientname,Instructions")] Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                var chefId = (decimal)  HttpContext.Session.GetInt32("chefSession");

                if (recipe.ImageFile != null)
                {
                    string wwwrootPath = _webHostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + recipe.ImageFile.FileName;
                    string fullPath = Path.Combine(wwwrootPath + "/Image/RecipeImage/", imageName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await recipe.ImageFile.CopyToAsync(fileStream);
                    }
                    recipe.Imagepath = imageName;
                }
                else
                {
                    recipe.Imagepath = "default.jpg";
                }
                    recipe.Recipestatusid = 2;
                    recipe.Chefid = chefId;
                    _context.Add(recipe);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("MyRecipes", "Chef");
                
            }
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "Categoryid", recipe.Categoryid);
            ViewData["Chefid"] = new SelectList(_context.Users, "Userid", "Userid", recipe.Chefid);
            ViewData["Recipestatusid"] = new SelectList(_context.Statuses, "Statusid", "Statusid", recipe.Recipestatusid);
            return View(recipe);
        }














        // GET: Recipes/Edit/5
        
        public async Task<IActionResult> Edit(decimal? id)
        {
            var ChefId = HttpContext.Session.GetInt32("chefSession");
             
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            ViewBag.image = recipe;

            if (recipe == null || recipe.Chefid != ChefId)
            {
                return NotFound();
            }
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "Categoryname");
            ViewData["Chefid"] = new SelectList(_context.Users, "Userid", "Userid", recipe.Chefid);
            ViewData["Recipestatusid"] = new SelectList(_context.Statuses, "Statusid", "Statusid", recipe.Recipestatusid);
            return View(recipe);
        }

        // POST: Recipes/Edit/5
      
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Edit(decimal id, [Bind("Recipename,Publishdate,ImageFile,Price,Categoryid,Recipedescription,Ingredientname,Instructions")] RecipeViewModel recipeViewModel , string? imagepath)
        {
            var existingRecipe = await _context.Recipes.FindAsync(id);
            if (existingRecipe == null)
            {
                return NotFound();
            }
            if (id != existingRecipe.Recipeid)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    if (recipeViewModel.ImageFile != null)
                    {
                        string wwwrootPath = _webHostEnvironment.WebRootPath;
                        string imageName = Guid.NewGuid().ToString() + "_" + recipeViewModel.ImageFile.FileName;
                        string fullPath = Path.Combine(wwwrootPath + "/Image/RecipeImage/", imageName);
                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            await recipeViewModel.ImageFile.CopyToAsync(fileStream);
                        }
                        existingRecipe.Imagepath = imageName;
                    }
                    else
                    {
                        existingRecipe.Imagepath = imagepath;
                    }
                    existingRecipe.Recipename = recipeViewModel.Recipename;
                   
                    existingRecipe.Price = recipeViewModel.Price;
                    existingRecipe.Categoryid = recipeViewModel.Categoryid;
                    existingRecipe.Recipestatusid = 2;
                    existingRecipe.Recipedescription = recipeViewModel.Recipedescription;
                    existingRecipe.Ingredientname = recipeViewModel.Ingredientname;
                    existingRecipe.Instructions = recipeViewModel.Instructions;

                    _context.Update(existingRecipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(existingRecipe.Recipeid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("MyRecipes", "Chef");
            }
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "Categoryname");
            ViewData["Chefid"] = new SelectList(_context.Users, "Userid", "Userid", existingRecipe.Chefid);
            ViewData["Recipestatusid"] = new SelectList(_context.Statuses, "Statusid", "Statusid", existingRecipe.Recipestatusid);
            return View(existingRecipe);
        }













































        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            var ChefId = HttpContext.Session.GetInt32("chefSession");

            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.Chef)
                .Include(r => r.Recipestatus)
                .FirstOrDefaultAsync(m => m.Recipeid == id);
            if (recipe == null || recipe.Chefid != ChefId)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Recipes == null)
            {
                return Problem("Entity set 'ModelContext.Recipes'  is null.");
            }
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(decimal id)
        {
          return (_context.Recipes?.Any(e => e.Recipeid == id)).GetValueOrDefault();
        }



        


        public IActionResult thanks()
        {
            return View();
        }






    }
}
