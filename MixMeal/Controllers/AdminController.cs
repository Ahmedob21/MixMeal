using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MixMeal.Models;
using System.Linq;

namespace MixMeal.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {

            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.registeredCustomer =  _context.Users.Where(user => user.Role.Rolename == "Customer").Count();
            ViewBag.PendingCustomer = _context.Users.Where(user => user.Role.Rolename == "Customer" && user.Userstatus.Statusname == "Pending").Count();
            ViewBag.RejectedCustomer = _context.Users.Where(user => user.Role.Rolename == "Customer" && user.Userstatus.Statusname == "Rejected").Count();
            ViewBag.registeredChef = _context.Users.Where(user => user.Role.Rolename == "Chef" && user.Userstatus.Statusname == "Accepted").Count();
            ViewBag.PendingChef = _context.Users.Where(user => user.Role.Rolename == "Chef" && user.Userstatus.Statusname == "Pending").Count();
            ViewBag.RejectedChef = _context.Users.Where(user => user.Role.Rolename == "Chef" && user.Userstatus.Statusname == "Rejected").Count();
            ViewBag.registeredAdmin = _context.Users.Where(user => user.Role.Rolename == "Admin").Count();
            ViewBag.AcceptedRecipe = _context.Recipes.Where(recipe => recipe.Recipestatus.Statusname == "Accepted").Count();
            ViewBag.PendingRecipe = _context.Recipes.Where(recipe => recipe.Recipestatus.Statusname == "Pending").Count();
            ViewBag.RejectedRecipe = _context.Recipes.Where(recipe => recipe.Recipestatus.Statusname == "Rejected").Count();

            //ViewBag.CategoryName = await _context.Recipes.Include(category => category.Category).ToListAsync();


            return View();
        }



        public async Task<IActionResult> Report()
        {
          
            var report = await _context.Purchases.Include(user => user.Customer)
                .Include(recipe => recipe.Recipe)
                .Include(chef => chef.Recipe.Chef)
                .Include(category => category.Recipe.Category).ToListAsync();

            // Counting total purchases
            ViewBag.TotalPurchases = report.Count();

            // Calculating total earnings
            decimal totalEarnings = report.Sum(purchase => purchase.Earnings);
            ViewBag.TotalEarnings = totalEarnings;

            // Getting unique categories
            var uniqueCategories = report.Select(purchase => purchase.Recipe.Category.Categoryname).Distinct().ToList();
            ViewBag.UniqueCategories = uniqueCategories;
            return View(report);
        }


        public async Task<IActionResult> MonthlyReport(int year, int month)
        {
            var report = await _context.Purchases
                .Include(p => p.Customer)
                .Include(p => p.Recipe)
                    .ThenInclude(r => r.Category)
                .Include(p => p.Recipe.Chef)
                .Where(p => p.Purchasedate.Year == year && p.Purchasedate.Month == month)
                .ToListAsync();

            return View(report);
        }


        public async Task<IActionResult> AnnualReport(int year)
        {
            var report = await _context.Purchases
                .Include(p => p.Customer)
                .Include(p => p.Recipe)
                    .ThenInclude(r => r.Category)
                .Include(p => p.Recipe.Chef)
                .Where(p => p.Purchasedate.Year == year)
                .ToListAsync();

            return View(report);
        }




        public IActionResult AddAdmin()
        {
            ViewData["Genderid"] = new SelectList(_context.Genders, "Genderid", "Gendername");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddAdmin([Bind("Firstname,Lastname,Email,Username,Phone,Upassword,Bithdate,Genderid")] RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = new();
                user.Firstname = registerViewModel.Firstname;
                user.Lastname = registerViewModel.Lastname;
                user.Username = registerViewModel.Username;
                user.Phone = registerViewModel.Phone;
                user.Bithdate = registerViewModel.Bithdate;
                if (registerViewModel.ImageFile != null)
                {
                    string wwwrootPath = _webHostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + registerViewModel.ImageFile.FileName;
                    string fullPath = Path.Combine(wwwrootPath + "/Image/personalImage/", imageName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                       await registerViewModel.ImageFile.CopyToAsync(fileStream);
                    }
                    user.Imagepath = imageName;
                }
                else
                {
                    user.Imagepath = "default.jpg";
                }
                
                user.Roleid = 1;
                user.Userstatusid = 1;
                user.Genderid = registerViewModel.Genderid;
                await _context.AddAsync(user);
                await _context.SaveChangesAsync();


                Userlogin userlogin = new Userlogin();
                userlogin.Email = registerViewModel.Email;
                userlogin.Upassword = registerViewModel.Upassword;
                userlogin.Userid = user.Userid;

                await _context.AddAsync(userlogin);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Admin");
            }

            return View(registerViewModel);
        }

        public async Task<IActionResult> AllAdmin()
        {
            var admin = await _context.Userlogins
                .Include(user => user.User)
                .Include(role => role.User.Role)
                .Where(user=> user.User.Role.Rolename == "Admin").ToListAsync();
            return View(admin);
        }



        [HttpGet]
        public async Task<IActionResult> AllCustomer()
        {
            var customers = await _context.Userlogins
                .Include(customer => customer.User)
                .Include(role=>role.User.Role)
                .Where(userRole => userRole.User.Role.Roleid == 3).ToListAsync();

            return View(customers);
        }


       

        public async Task<IActionResult> CustomerDetails(decimal? userid)
        {
            if (userid == null || _context.Userlogins == null)
            {
                return NotFound();
            }

            var customer = await _context.Userlogins.Include(user => user.User)
                .FirstOrDefaultAsync(user => user.Userid == userid);



            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }









        //Chef Section
        [HttpGet]
        public async Task<IActionResult> AllChef()
        {
            var Chefs = await _context.Userlogins.Include(user => user.User).Include(role => role.User.Role)
                                                 .Include(u => u.User.Userstatus)
           .Where(chef => chef.User.Role.Rolename == "Chef" && chef.User.Userstatus.Statusname == "Accepted" || chef.User.Userstatus.Statusname == "Rejected")
           .ToListAsync();
            return View(Chefs);
        }



        [HttpGet]
        public async Task<IActionResult> ChefRequest()
        {
            var request = await _context.Userlogins.Include(chef => chef.User).Include(role=> role.User.Role)
                .Include(u => u.User.Userstatus)
                .Where(user => user.User.Userstatus.Statusname == "Pending" && user.User.Role.Rolename == "Chef").ToListAsync();

            return View(request);
        }
      


        [HttpPost]
        public async Task<IActionResult> AcceptChef(decimal id)
        {

            if (id == null || _context.Users == null)
            {
                return NotFound();
            }
            var chefs = await _context.Users.FindAsync(id);
            if (chefs != null)
            {
                chefs.Userstatusid = 1;
                _context.Users.Update(chefs);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(AllChef));
        }
        [HttpPost]
        public async Task<IActionResult> RejectChef(decimal? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }
            var chefs = await _context.Users.FindAsync(id);
            if (chefs != null)
            {
                 chefs.Userstatusid = 3;
                _context.Users.Update(chefs);
                await  _context.SaveChangesAsync();        
            }
           return RedirectToAction(nameof(AllChef));

        }


































        // Recipe Section
        public async Task<IActionResult> AccepttedRecipe()
        {

            var recipes = await _context.Recipes.Include(status => status.Recipestatus)
                .Include(chef => chef.Chef)
                .Include(category => category.Category)
                .Where(status => status.Recipestatus.Statusname == "Accepted").ToListAsync();
            return View(recipes);
        }
        public async Task<IActionResult> RejectedRecipe()
        {

            var recipes = await _context.Recipes.Include(status => status.Recipestatus)
                .Include(chef => chef.Chef)
                .Include(category => category.Category)
                .Include(u => u.Recipestatus).Where(status => status.Recipestatus.Statusname == "Rejected").ToListAsync();
            return View(recipes);
        }


        [HttpGet]
        public async Task<IActionResult> RecipeRequest()
        {
            var request = await _context.Recipes.Include(chef => chef.Chef)
                .Include(category => category.Category)
                .Include(u => u.Recipestatus)
                .Where(recipe => recipe.Recipestatus.Statusname == "Pending" ).ToListAsync();

            return View(request);
        }



        public async Task<IActionResult> RecipeDetails(decimal? id)
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


        // GET: Recipes/Delete/5
        public async Task<IActionResult> DeleteRecipe(decimal? id)
        {
            //var ChefId = HttpContext.Session.GetInt32("chefSession");

            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.Chef)
                .Include(r => r.Recipestatus)
                .FirstOrDefaultAsync(m => m.Recipeid == id);
            if (recipe == null )
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("DeleteRecipe")]
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


        public async Task<IActionResult> AcceptRecipe(decimal id)
        {

            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                recipe.Recipestatusid = 1;
                _context.Recipes.Update(recipe);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(RecipeRequest));
        }

        
        public async Task<IActionResult> RejectRecipe(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                recipe.Recipestatusid = 3;
                _context.Recipes.Update(recipe);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(RecipeRequest));

        }







        // Testimonials Section
        public async Task<IActionResult> Testimonials()
        {
            var testimonial = await _context.Testimonials.Include(user => user.Cust).Include(status => status.Testimonialstatus)
                .Where(status => status.Testimonialstatus.Statusname == "Accepted" || status.Testimonialstatus.Statusname == "Rejected").ToListAsync();

            return View(testimonial);
        }



        public async Task<IActionResult> PendingTestimonials()
        {
            var testimonial = await _context.Testimonials.Include(user => user.Cust).Include(status => status.Testimonialstatus)
                .Where(testimonials => testimonials.Testimonialstatus.Statusname == "Pending").ToListAsync();
                                                         
            return View(testimonial);
        }


        public async Task<IActionResult> AcceptTestimonials(decimal id)
        {

            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }
            var Testimonial = await _context.Testimonials.FindAsync(id);
            if (Testimonial != null)
            {
                Testimonial.Testimonialstatusid = 1;
                _context.Testimonials.Update(Testimonial);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(PendingTestimonials));
        }


        public async Task<IActionResult> RejectTestimonials(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }
            var Testimonial = await _context.Testimonials.FindAsync(id);
            if (Testimonial != null)
            {
                Testimonial.Testimonialstatusid = 3;
                _context.Testimonials.Update(Testimonial);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(PendingTestimonials));

        }





























            public IActionResult SearchByDate()
            {
            var result = _context.Recipes
                .Include(x => x.Category)
                .Include(x => x.Chef)
                .ToList();
            return View(result);
            }


        [HttpPost]
        public IActionResult SearchByDate(DateTime? startDate, DateTime? endDate)
        {
            var result = _context.Recipes.Include(x => x.Category).Include(x => x.Chef).Include(status => status.Recipestatus).ToList();

            if (startDate == null && endDate == null)
            {

                return View(result);
            }
            else if (startDate != null && endDate == null)
            {
                result = result.Where(x => x.Publishdate >= startDate).ToList();
                return View(result);
            }
            else if (startDate == null && endDate != null)
            {
                result = result.Where(x => x.Publishdate <= endDate).ToList();
                return View(result);
            }
            else
            {
                result = result.Where(x => x.Publishdate >= startDate && x.Publishdate <= endDate).ToList();
                return View(result);
            }
        }
        [HttpGet]
        [Route("admin/search-by-name")]
        public IActionResult SearchByName()
        {
            var result = _context.Recipes
                .Include(x => x.Category)
                .Include(x => x.Chef)
                .ToList();
            return View(result);
        }
        [HttpPost]
        [Route("admin/search-by-name")]
        public IActionResult SearchByName( string? RecipeName)
        {
            var result = _context.Recipes.Include(x => x.Category).Include(x => x.Chef).Include(status => status.Recipestatus).ToList();
            if (string.IsNullOrEmpty(RecipeName))
            {
                return View(result);
            }
            else
            {
                result = result
                    .Where(recipenames => recipenames.Recipename.ToLower().Contains(RecipeName.ToLower()))
                    .ToList();
                return View(result);
            }

        }









        public IActionResult empty()
        {
            return View();
        }
    }
}
