using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MixMeal.customAuth;
using MixMeal.Models;

namespace MixMeal.Controllers
{
    [CustomAuthorize(1)] // Admin
    public class ContactuspagecontentsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ContactuspagecontentsController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Contactuspagecontents
        public async Task<IActionResult> Index()
        {
              return _context.Contactuspagecontents != null ? 
                          View(await _context.Contactuspagecontents.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Contactuspagecontents'  is null.");
        }

        // GET: Contactuspagecontents/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Contactuspagecontents == null)
            {
                return NotFound();
            }

            var contactuspagecontent = await _context.Contactuspagecontents
                .FirstOrDefaultAsync(m => m.Contactuspagecontentid == id);
            if (contactuspagecontent == null)
            {
                return NotFound();
            }

            return View(contactuspagecontent);
        }

        // GET: Contactuspagecontents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contactuspagecontents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Contactuspagecontentid,Contenttype,Content,Position,ImageFile")] Contactuspagecontent contactuspagecontent)
        {
            if (ModelState.IsValid)
            {
                if (contactuspagecontent.ImageFile != null)
                {
                    string wwwrootPath = _webHostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + contactuspagecontent.ImageFile.FileName;
                    string fullPath = Path.Combine(wwwrootPath + "/Image/ContactImage/", imageName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await contactuspagecontent.ImageFile.CopyToAsync(fileStream);
                    }
                    contactuspagecontent.Imagepath = imageName;
                }
                else
                {
                    contactuspagecontent.Imagepath = null;
                }

                contactuspagecontent.Position = 1;
                _context.Add(contactuspagecontent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactuspagecontent);
        }

        // GET: Contactuspagecontents/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Contactuspagecontents == null)
            {
                return NotFound();
            }

            var contactuspagecontent = await _context.Contactuspagecontents.FindAsync(id);
            ViewBag.image = contactuspagecontent;
            if (contactuspagecontent == null)
            {
                return NotFound();
            }
            return View(contactuspagecontent);
        }

        // POST: Contactuspagecontents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Contactuspagecontentid,Contenttype,Content,ImageFile")] Contactuspagecontent contactuspagecontent, string? imagepath)
        {
            if (id != contactuspagecontent.Contactuspagecontentid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (contactuspagecontent.ImageFile != null)
                    {
                        string wwwrootPath = _webHostEnvironment.WebRootPath;
                        string imageName = Guid.NewGuid().ToString() + "_" + contactuspagecontent.ImageFile.FileName;
                        string fullPath = Path.Combine(wwwrootPath + "/Image/ContactImage/", imageName);
                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            await contactuspagecontent.ImageFile.CopyToAsync(fileStream);
                        }
                        contactuspagecontent.Imagepath = imageName;
                    }
                    else
                    {
                        contactuspagecontent.Imagepath = imagepath;
                    }
                    contactuspagecontent.Position = 1;
                    _context.Update(contactuspagecontent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactuspagecontentExists(contactuspagecontent.Contactuspagecontentid))
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
            return View(contactuspagecontent);
        }

        // GET: Contactuspagecontents/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Contactuspagecontents == null)
            {
                return NotFound();
            }

            var contactuspagecontent = await _context.Contactuspagecontents
                .FirstOrDefaultAsync(m => m.Contactuspagecontentid == id);
            if (contactuspagecontent == null)
            {
                return NotFound();
            }

            return View(contactuspagecontent);
        }

        // POST: Contactuspagecontents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Contactuspagecontents == null)
            {
                return Problem("Entity set 'ModelContext.Contactuspagecontents'  is null.");
            }
            var contactuspagecontent = await _context.Contactuspagecontents.FindAsync(id);
            if (contactuspagecontent != null)
            {
                _context.Contactuspagecontents.Remove(contactuspagecontent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactuspagecontentExists(decimal id)
        {
          return (_context.Contactuspagecontents?.Any(e => e.Contactuspagecontentid == id)).GetValueOrDefault();
        }
    }
}
