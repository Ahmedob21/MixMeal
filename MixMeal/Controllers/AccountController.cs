using Humanizer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MixMeal.Models;
using NuGet.Protocol.Plugins;




namespace MixMeal.Controllers
{
    public class AccountController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AccountController(ModelContext context , IWebHostEnvironment webHostEnvironment)
        {
            
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChefSignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChefSignUp([Bind("Firstname,Lastname,Email,Username,Phone,Upassword,Bithdate")] RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = new();
                user.Firstname = registerViewModel.Firstname;
                user.Lastname = registerViewModel.Lastname;
                user.Username = registerViewModel.Username;
                user.Phone = registerViewModel.Phone;
                user.Bithdate = registerViewModel.Bithdate;
                user.Roleid = 2;
                user.Userstatusid = 2;
                

                await _context.AddAsync(user);
                await _context.SaveChangesAsync();

                Userlogin userlogin = new();
                userlogin.Email = registerViewModel.Email;
               
                userlogin.Upassword = registerViewModel.Upassword;
                userlogin.Userid = user.Userid;
                
                await _context.AddAsync(userlogin);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp([Bind("Firstname,Lastname,Email,Username,Phone,Upassword,Bithdate")] RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid) { 
                User user = new();
                user.Firstname = registerViewModel.Firstname;
                user.Lastname = registerViewModel.Lastname;
                user.Username = registerViewModel.Username;
                user.Phone = registerViewModel.Phone;
                user.Bithdate = registerViewModel.Bithdate;
                user.Roleid = 3;
                user.Userstatusid = 1;
                
                await _context.AddAsync(user);
                await _context.SaveChangesAsync();

                Userlogin userlogin = new();
                 userlogin.Email = registerViewModel.Email;
                 userlogin.Upassword = registerViewModel.Upassword;
                 userlogin.Userid = user.Userid;
               
                
                
                await  _context.AddAsync(userlogin);
                await  _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
           return View();
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Upassword")] LoginViewModel userlogin)
        {
            if (!ModelState.IsValid)
            {
                return View(userlogin);
            }

            var user = await _context.Userlogins
                .Include(u => u.User)
                .SingleOrDefaultAsync(u => u.Email == userlogin.Email && u.Upassword == (userlogin.Upassword));



            var userId = user.Userid;
            var roleId = user.User.Roleid;

           

            if (user == null)
            {
                ModelState.AddModelError("", "invalid email or password");
                return View(userlogin);
            }

            

            switch (roleId)
            {
                case 1: // Admin
                    HttpContext.Session.SetInt32("adminSession", (int)userId);
                    return RedirectToAction("Index", "Admin");
                case 2: // User
                    HttpContext.Session.SetInt32("chefSession", (int)userId);
                    return RedirectToAction("Index", "Chef");
                case 3: // Chef
                    HttpContext.Session.SetInt32("userSession", (int)userId);
                    return RedirectToAction("Index", "Customer");
                default:
                    ModelState.AddModelError("", "Something Error");
                    return View(userlogin);
            }






        }


        public IActionResult Logout()
        {
         
            HttpContext.Session.Remove("chefSession");
            return View(nameof(Login));
        }




    }
}
