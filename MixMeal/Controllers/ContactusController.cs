using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MixMeal.customAuth;
using MixMeal.Models;

namespace MixMeal.Controllers
{
    
    public class ContactusController : Controller
    {
        private readonly ModelContext _context;

        public ContactusController(ModelContext context)
        {
            _context = context;
        }
        [CustomAuthorize(1)] // Admin
        public async Task<IActionResult> ContactInbox()
        {

            ViewBag.EmailCount = _context.Contactus.Count();


            return _context.Contactus != null ?
                         View(await _context.Contactus.ToListAsync()) :
                         Problem("Entity set 'ModelContext.Contactus'  is null.");

            return View();
        }

        [CustomAuthorize(1)] // Admin
        // GET: Contactus/Details/5
        public async Task<IActionResult> EmailDetails(decimal? id)
        {
            if (id == null || _context.Contactus == null)
            {
                return NotFound();
            }

            var contactu = await _context.Contactus
                .FirstOrDefaultAsync(m => m.Contactusid == id);
            if (contactu == null)
            {
                return NotFound();
            }

            return View(contactu);
        }

        // GET: Contactus/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Phone = await _context.Contactuspagecontents.SingleOrDefaultAsync(c => c.Contenttype == "Phone");
            ViewBag.email =await _context.Contactuspagecontents.SingleOrDefaultAsync(c => c.Contenttype == "Email");
            ViewBag.Time = await _context.Contactuspagecontents.SingleOrDefaultAsync(c => c.Contenttype == "contact Time");
            ViewBag.message = await _context.Contactuspagecontents.SingleOrDefaultAsync(c => c.Contenttype == "message");
            return View();
        }

        // POST: Contactus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Custname,Subject,Custemail,Message,Contactdate")] Contactu contactu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactu);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index" ,"Home");
            }
            return View(contactu);
        }


       
        // GET: Contactus/Delete/5
        [CustomAuthorize(1)] // Admin
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Contactus == null)
            {
                return NotFound();
            }

            var contactu = await _context.Contactus
                .FirstOrDefaultAsync(m => m.Contactusid == id);
            if (contactu == null)
            {
                return NotFound();
            }

            return View(contactu);
        }

        // POST: Contactus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(1)] // Admin
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Contactus == null)
            {
                return Problem("Entity set 'ModelContext.Contactus'  is null.");
            }
            var contactu = await _context.Contactus.FindAsync(id);
            if (contactu != null)
            {
                _context.Contactus.Remove(contactu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactuExists(decimal id)
        {
          return (_context.Contactus?.Any(e => e.Contactusid == id)).GetValueOrDefault();
        }
    }
}
