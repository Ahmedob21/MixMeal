using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MixMeal.customAuth;
using MixMeal.EmailSender;
using MixMeal.Models;
using MixMeal.PDFGenerator;
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
         // Customer
        public async Task<IActionResult> MyPurchase()
        {
            var customerid = HttpContext.Session.GetInt32("CustomerSession");
            var mypurchase = await _context.Purchases
                .Include(user => user.Customer)
                .Include(recipe =>recipe.Recipe)
                .Include(category => category.Recipe.Category)
                .Include(chef =>chef.Recipe.Chef)
                .Where(userid => userid.Customerid == customerid).ToListAsync();
            return View(mypurchase);
        }

        [HttpGet]
        public async Task<IActionResult>  VisaCards(decimal id)
        {
            var customerid = HttpContext.Session.GetInt32("CustomerSession");
            if(customerid == null)
            {
                return RedirectToAction("Login", "Account");
            }
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

           

            return RedirectToAction("UpdatePdfOnPurchase","Customer" , new { id });
        }


        public async Task<IActionResult> UpdatePdfOnPurchase(decimal id)
        {
            var recipe = await _context.Recipes
                                 .Include(r => r.Category)
                                 .Include(r => r.Chef)
                                 .Include(r => r.Recipestatus)
                                 .SingleOrDefaultAsync(r => r.Recipeid == id);
            if (recipe == null)
            {
                return NotFound();
            }

            var pdfUpdater = new PDFG(_context, _webHostEnvironment);
            string filePath = pdfUpdater.UpdateRecipePdf(recipe);

            var CustId = HttpContext.Session.GetInt32("CustomerSession");
            var customer = await _context.Userlogins.Include(user => user.User)
                                        .Where(customers => customers.Userid == CustId).SingleOrDefaultAsync();
            // Send email with the PDF attachment
            string recipientEmail = customer.Email;
            string subject = "Your Recipe PDF";
            string body = "Dear Customer, please find attached your recipe PDF.";
            SendEmail send = new SendEmail();

            send.SendEmailWithPDF(recipientEmail, subject, body, filePath);




            return RedirectToAction("thanks", "Recipes");
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
