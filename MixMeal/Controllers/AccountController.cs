using Humanizer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MixMeal.Models;
using NuGet.Protocol.Plugins;
using System.Security.Cryptography;




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
            ViewData["Genderid"] = new SelectList(_context.Genders, "Genderid", "Gendername");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChefSignUp([Bind("Firstname,Lastname,Email,Username,Phone,Upassword,Bithdate,Genderid")] RegisterViewModel registerViewModel)
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
                      await  registerViewModel.ImageFile.CopyToAsync(fileStream);
                    }
                    user.Imagepath = imageName;
                }
                else
                {
                    user.Imagepath = "default.jpg";
                }
                user.Roleid = 2;
                user.Userstatusid = 2;
                user.Genderid = registerViewModel.Genderid;

                await _context.AddAsync(user);
                await _context.SaveChangesAsync();

                Userlogin userlogin = new();
                userlogin.Email = registerViewModel.Email.ToLower();
               
                userlogin.Upassword = registerViewModel.Upassword;
                userlogin.Userid = user.Userid;
                
                await _context.AddAsync(userlogin);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            ViewData["Genderid"] = new SelectList(_context.Genders, "Genderid", "Gendername");


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp([Bind("Firstname,Lastname,Username,Phone,Email,Upassword,Bithdate,ImageFile,Genderid")] RegisterViewModel registerViewModel)
        {
            
            if (ModelState.IsValid) {
               
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
                      await  registerViewModel.ImageFile.CopyToAsync(fileStream);
                    }
                    user.Imagepath = imageName;
                }
                else
                {
                    user.Imagepath = "default.jpg";
                }
                user.Roleid = 3;
                user.Userstatusid = 1;
                user.Genderid = registerViewModel.Genderid;

                await _context.AddAsync(user);
                await _context.SaveChangesAsync();

                Userlogin userlogin = new();
                 userlogin.Email = registerViewModel.Email.ToLower();
                 userlogin.Upassword = registerViewModel.Upassword;
                 userlogin.Userid = user.Userid;
               
                
                
                await  _context.AddAsync(userlogin);
                await  _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            ViewBag.gender = new SelectList(_context.Genders, "GenderId", "GenderId", registerViewModel.Genderid);
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
                .Include(u => u.User).Include(role =>role.User.Role)
                .SingleOrDefaultAsync(u => u.Email == userlogin.Email && u.Upassword == (userlogin.Upassword));

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(userlogin);
            }

            if (user.User == null || user.User.Role == null)
            {
                ModelState.AddModelError("", "User or role information is incomplete.");
                return View(userlogin);
            }

            HttpContext.Session.SetInt32("userSession",(int) user.Userid);
            HttpContext.Session.SetInt32("roleSession", (int)user.User.Roleid);



          

            

            switch (user.User.Roleid)
            {
                case 1: // Admin
                    HttpContext.Session.SetInt32("adminSession", (int)user.Userid);
                    ViewBag.adminid = user.Userid;
                    return RedirectToAction("Index", "Admin");
                case 2: // Chef
                    HttpContext.Session.SetInt32("chefSession", (int)user.Userid);
                    ViewBag.chefid = user.Userid;
                    return RedirectToAction("Index", "Chef");
                case 3: // Customer
                    HttpContext.Session.SetInt32("CustomerSession", (int)user.Userid);
                    ViewBag.Customerid = user.Userid;
                    return RedirectToAction("Index", "Customer");
                default:
                    ModelState.AddModelError("", "Unknown role.");
                    return View(userlogin);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Profile(decimal id)
        {
            var userId = HttpContext.Session.GetInt32("userSession");
            var roleId = HttpContext.Session.GetInt32("roleSession");

            if (userId == null || roleId == null)
            {
                return RedirectToAction("Login");
            }

            if (id != userId)
            {
                return RedirectToAction("Index", "Home");
            }

            var profile = await _context.Userlogins
                .Include(x => x.User)
                .Include(user => user.User.Role)
                .Include(user => user.User.Userstatus)
                .SingleOrDefaultAsync(u => u.Userid == userId);

            if (profile == null)
            {
                return NotFound();
            }

            switch (roleId)
            {
                case 1: // Admin
                    return View("AdminProfile", profile);
                case 2: // Chef
                    return View("ChefProfile", profile);
                case 3: // Customer
                    return View("CustomerProfile", profile);
                default:
                    return RedirectToAction("Login");
            }
        }










        [HttpGet]
        public async Task<IActionResult> UpdateMyProfile(decimal id)
        {
            var userId = HttpContext.Session.GetInt32("userSession");
            var roleId = HttpContext.Session.GetInt32("roleSession");

            if (userId == null || roleId == null)
            {
                return RedirectToAction("Login");
            }

            if (id != userId)
            {
                return Unauthorized();
            }

            var profile = await _context.Users
                .Include(user => user.Role)
                .Include(user => user.Userstatus)
                .SingleOrDefaultAsync(u => u.Userid == userId);

            if (profile == null)
            {
                return NotFound();
            }

            ViewData["Genderid"] = new SelectList(_context.Genders, "Genderid", "Gendername");

            switch (profile.Roleid)
            {
                case 1: // Admin
                    return View("AdminEditProfile", profile);
                case 2: // Chef
                    return View("ChefEditProfile", profile);
                case 3: // Customer
                    return View("CustomerEditProfile", profile);
                default:
                    return View("Login");
            }
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMyProfile([Bind("Firstname,Lastname,Username,Phone,Bithdate,ImageFile,Genderid")] EditProfile updatedProfile)
        {
            var userId = HttpContext.Session.GetInt32("userSession");

            if (userId == null)
            {
                return Unauthorized();
            }
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Userid == userId);

            if (ModelState.IsValid)
            {
               
                try
                {
                    // Update the existing user with the new data, except Userid and Roleid
                    existingUser.Firstname = updatedProfile.Firstname;
                    existingUser.Lastname = updatedProfile.Lastname;
                    existingUser.Username = updatedProfile.Username;
                    existingUser.Phone = updatedProfile.Phone;
                    existingUser.Bithdate = updatedProfile.Bithdate;
                    existingUser.ImageFile = updatedProfile.ImageFile;
                    existingUser.Genderid = updatedProfile.Genderid;
                    
                    if (updatedProfile.ImageFile != null)
                    {
                        string wwwrootPath = _webHostEnvironment.WebRootPath;
                        string imageName = Guid.NewGuid().ToString() + "_" + updatedProfile.ImageFile.FileName;
                        string fullPath = Path.Combine(wwwrootPath + "/Image/personalImage/", imageName);
                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            await updatedProfile.ImageFile.CopyToAsync(fileStream);
                        }
                        existingUser.Imagepath = imageName;
                    }
                    else
                    {
                        existingUser.Imagepath = "default.jpg";
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return View("ConcurrencyError");
                }

                ViewData["Genders"] = new SelectList(_context.Genders, "Genderid", "Gendername", updatedProfile.Genderid);
                switch (existingUser.Roleid)
                {
                    case 1: // Admin
                        return RedirectToAction("Profile", new { id = existingUser.Userid });
                    case 2: // Chef
                        return RedirectToAction("Profile", new { id = existingUser.Userid });
                    case 3: // Customer
                        return RedirectToAction("Profile", new { id = existingUser.Userid });
                    default:
                        return RedirectToAction("Login");
                }
            }

          

            ViewData["Genders"] = new SelectList(_context.Genders, "Genderid", "Gendername");
            switch (existingUser.Roleid)
            {
                case 1: // Admin
                    return View("AdminEditProfile", updatedProfile);
                case 2: // Chef
                    return View("ChefEditProfile", updatedProfile);
                case 3: // Customer
                    return View("CustomerEditProfile", updatedProfile);
                default:
                    return RedirectToAction("Login");
            }




        }
        private bool UserExists(decimal id)
        {
            return _context.Userlogins.Any(e => e.Userid == id);
        }


        [HttpGet]
        public async Task<IActionResult> UpdateAccount(decimal id)
        {
            var userId = HttpContext.Session.GetInt32("userSession");
            var roleId = HttpContext.Session.GetInt32("roleSession");

            if (userId == null || roleId == null)
            {
                return RedirectToAction("Login");
            }

            if (id != userId)
            {
                return Unauthorized();
            }

            var profile = await _context.Userlogins.Include(user => user.User)
                .Include(user => user.User.Role)
                .Include(user => user.User.Userstatus)
                .SingleOrDefaultAsync(u => u.Userid == userId);

            if (profile == null)
            {
                return NotFound();
            }

           

            switch (profile.User.Roleid)
            {
                case 1: // Admin
                    return View("UpdateAdminAccount");
                case 2: // Chef   
                    return View("UpdateChefAccount");
                case 3: // Customer    
                    return View("UpdateCustomerAccount");
                default:
                    return View("Login");
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAccount([Bind("Email,OldPassword,Upassword,ConfirmPassword")] EditAccount editAccount)
        {
            var userId = HttpContext.Session.GetInt32("userSession");

            if (userId == null)
            {
                return Unauthorized();
            }
            var existingUser = await _context.Userlogins.Include(user => user.User)
                .SingleOrDefaultAsync(u => u.Userid == userId);

            if (ModelState.IsValid)
            {
                if(!verifyPassword(editAccount.OldPassword , existingUser.Upassword))
                {
                    ModelState.AddModelError("OldPassword", "The old password is incorrect.");
                    switch (existingUser.User.Roleid)
                    {
                        case 1: // Admin
                            return View("UpdateAdminAccount", editAccount);
                        case 2: // Chef
                            return View("UpdateChefAccount", editAccount);
                        case 3: // Customer
                            return View("UpdateCustomerAccount", editAccount);
                        default:
                            return RedirectToAction("Login");
                    }
                }

                try
                {
                    
                    existingUser.Email = editAccount.Email;
                    existingUser.Upassword = editAccount.Upassword;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return View("ConcurrencyError");
                }

               
                switch (existingUser.User.Roleid)
                {
                    case 1: // Admin
                        return RedirectToAction("Profile", new { id = existingUser.Userid });
                    case 2: // Chef
                        return RedirectToAction("Profile", new { id = existingUser.Userid });
                    case 3: // Customer
                        return RedirectToAction("Profile", new { id = existingUser.Userid });
                    default:
                        return RedirectToAction("Login");
                }
            }


            switch (existingUser.User.Roleid)
            {
                case 1: // Admin
                    return View("UpdateAdminAccount", editAccount);
                case 2: // Chef
                    return View("UpdateChefAccount", editAccount);
                case 3: // Customer
                    return View("UpdateCustomerAccount", editAccount);
                default:
                    return RedirectToAction("Login");
            }

        }


        private bool verifyPassword(string EnteredPassword , string storedPassword)
        {
            if (EnteredPassword == storedPassword) { return true; }
            else { return false; }
        } 


        public IActionResult Logout()
        {
            HttpContext.Session.Remove("adminSession");
            HttpContext.Session.Remove("chefSession");
            HttpContext.Session.Remove("userSession");
            return View(nameof(Login));
        }

        public IActionResult ErrorPage()
        {
            return View();
        }

        public IActionResult accessPermissions()
        {
            return View();
        }


    }
}
