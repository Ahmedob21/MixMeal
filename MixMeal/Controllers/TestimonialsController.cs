using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MixMeal.Models;

namespace MixMeal.Controllers
{
    public class TestimonialsController : Controller
    {
        private readonly ModelContext _context;

        public TestimonialsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Testimonials
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Testimonials.Include(t => t.Cust).Include(t => t.Testimonialstatus);
            return View(await modelContext.ToListAsync());
        }

        // GET: Testimonials/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.Cust)
                .Include(t => t.Testimonialstatus)
                .FirstOrDefaultAsync(m => m.Testumonialid == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // GET: Testimonials/Create
        public IActionResult Create()
        {
            var cust = HttpContext.Session.GetInt32("CustomerSession");
            if (cust != null)
            {
                ViewData["Custid"] = new SelectList(_context.Users, "Userid", "Userid");
                ViewData["Testimonialstatusid"] = new SelectList(_context.Statuses, "Statusid", "Statusid");
                return View();

            }

            return RedirectToAction("Login", "Account");
        }

        // POST: Testimonials/Create
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ucomment")] AddTestimonial addTestimonial)
        {
            var cust =  HttpContext.Session.GetInt32("CustomerSession");
            var customer = await _context.Users.SingleOrDefaultAsync(customers => customers.Userid == cust);
            

            if (ModelState.IsValid)
            {
                    Testimonial testimonial = new Testimonial();

                    testimonial.Ucomment = addTestimonial.Ucomment;
                    testimonial.Custid = (decimal)cust;
                    testimonial.Testimonialstatusid = 2;
                    _context.Add(testimonial);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index" , "Home");
            }
                
                return View(addTestimonial);



        }

        

        // GET: Testimonials/Delete/5   
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.Cust)
                .Include(t => t.Testimonialstatus)
                .FirstOrDefaultAsync(m => m.Testumonialid == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // POST: Testimonials
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Testimonials == null)
            {
                return Problem("Entity set 'ModelContext.Testimonials'  is null.");
            }
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                _context.Testimonials.Remove(testimonial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestimonialExists(decimal id)
        {
          return (_context.Testimonials?.Any(e => e.Testumonialid == id)).GetValueOrDefault();
        }
    }
}
