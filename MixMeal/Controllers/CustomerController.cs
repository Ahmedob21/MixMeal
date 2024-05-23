using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MixMeal.Models;
using Org.BouncyCastle.Bcpg;
using System.Text.RegularExpressions;

namespace MixMeal.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CustomerController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {

            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult>  VisaCards(decimal id)
        {
            var recipe =  _context.Recipes.SingleOrDefaultAsync(recipe => recipe.Recipeid == id);
            ViewBag.recipe = id;
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> VisaCards(decimal id , VisaCard visa )
        {
            var customerid = HttpContext.Session.GetInt32("CustomerSession");
            var recipe = await _context.Recipes.SingleOrDefaultAsync(recipe => recipe.Recipeid == id);
            ViewBag.recipe = recipe.Recipeid;
           
            if (customerid == null)
            {
                RedirectToAction("Login", "Account");
            }
            if (!IsValidVisaCard(visa)) 
            {
                ModelState.AddModelError("", "This card is invalid");
                return View(visa);
            }
            if (!DoesVisaCardExist(visa))
            {
                ModelState.AddModelError("", "This card is invalid");
                return View(visa);
            }
            var card = await _context.Paymentcards.SingleOrDefaultAsync(x=> x.Cardnumber == visa.Cardnumber );

            if (card.Balance < recipe.Price + Convert.ToDecimal((double)recipe.Price * 0.2))
            {
                ModelState.AddModelError("", "The payment transaction was rejected");
                return View(visa);
            }
            Purchase purchase=new Purchase();
            purchase.Recipeid = recipe.Recipeid;
            purchase.Customerid = (decimal)customerid;
            decimal totalPrice= Convert.ToDecimal((recipe.Price * Convert.ToDecimal(0.2))+recipe.Price);
            purchase.Earnings = Convert.ToDecimal((double)recipe.Price * 0.2);
            _context.Add(purchase);
            await _context.SaveChangesAsync();
            card.Balance-=(decimal)totalPrice;
            _context.Update(card);
            await _context.SaveChangesAsync();

           

            return RedirectToAction("UpdatePdfOnPurchase", "Recipes" , new { id });
        }

      

        private bool IsValidVisaCard(VisaCard payment)
        {
       

            // Validate expiration date
            if (DateTime.Parse($"{payment.Expiredate.Year}-{payment.Expiredate.Month}-01") < DateTime.Now)
            {
                return false;
            }

            // Validate CVV
            if (payment.Cvv.Length < 3)
            {
                return false;
            }

            return true;
        }
        private bool DoesVisaCardExist(VisaCard payment)
        {
            var card = _context.Paymentcards
                .Any(card => card.Cardnumber == payment.Cardnumber && card.Cardname == payment.Cardname && card.Expiredate == payment.Expiredate && card.Cvv == payment.Cvv);
            
            if(card == null)
            {
                return false ;
            }
            
            return true;
        }
       

    }
}
