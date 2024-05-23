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
    public class PurchasesController : Controller
    {
        private readonly ModelContext _context;

        public PurchasesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Purchases
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Purchases.Include(p => p.Customer).Include(p => p.Recipe).Include(chef => chef.Recipe.Chef);
            return View(await modelContext.ToListAsync());
        }

        // GET: Purchases/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Purchases == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases
                .Include(p => p.Customer)
                .Include(p => p.Recipe)
                .Include(chef => chef.Recipe.Chef)
                .FirstOrDefaultAsync(m => m.Purchaseid == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // GET: Purchases/Create
        public IActionResult Create()
        {
            ViewData["Customerid"] = new SelectList(_context.Users, "Userid", "Userid");
            ViewData["Recipeid"] = new SelectList(_context.Recipes, "Recipeid", "Recipeid");
            return View();
        }

        // POST: Purchases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Purchaseid,Purchasedate,Customerid,Recipeid,Earnings")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Customerid"] = new SelectList(_context.Users, "Userid", "Userid", purchase.Customerid);
            ViewData["Recipeid"] = new SelectList(_context.Recipes, "Recipeid", "Recipeid", purchase.Recipeid);
            return View(purchase);
        }

        // GET: Purchases/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Purchases == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }
            ViewData["Customerid"] = new SelectList(_context.Users, "Userid", "Userid", purchase.Customerid);
            ViewData["Recipeid"] = new SelectList(_context.Recipes, "Recipeid", "Recipeid", purchase.Recipeid);
            return View(purchase);
        }

        // POST: Purchases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Purchaseid,Purchasedate,Customerid,Recipeid,Earnings")] Purchase purchase)
        {
            if (id != purchase.Purchaseid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseExists(purchase.Purchaseid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Customerid"] = new SelectList(_context.Users, "Userid", "Userid", purchase.Customerid);
            ViewData["Recipeid"] = new SelectList(_context.Recipes, "Recipeid", "Recipeid", purchase.Recipeid);
            return View(purchase);
        }

        // GET: Purchases/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Purchases == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases
                .Include(p => p.Customer)
                .Include(p => p.Recipe)
                .FirstOrDefaultAsync(m => m.Purchaseid == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Purchases == null)
            {
                return Problem("Entity set 'ModelContext.Purchases'  is null.");
            }
            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase != null)
            {
                _context.Purchases.Remove(purchase);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseExists(decimal id)
        {
          return (_context.Purchases?.Any(e => e.Purchaseid == id)).GetValueOrDefault();
        }
    }
}
