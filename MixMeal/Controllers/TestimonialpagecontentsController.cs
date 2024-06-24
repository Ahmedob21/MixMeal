using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MixMeal.customAuth;
using MixMeal.Models;

namespace MixMeal.Controllers
{
    [CustomAuthorize(1)] // Admin
    public class TestimonialpagecontentsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TestimonialpagecontentsController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Testimonialpagecontents
        public async Task<IActionResult> Index()
        {
              return _context.Testimonialpagecontents != null ? 
                          View(await _context.Testimonialpagecontents.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Testimonialpagecontents'  is null.");
        }

        // GET: Testimonialpagecontents/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Testimonialpagecontents == null)
            {
                return NotFound();
            }

            var testimonialpagecontent = await _context.Testimonialpagecontents
                .FirstOrDefaultAsync(m => m.Testimonialpagecontentid == id);
            if (testimonialpagecontent == null)
            {
                return NotFound();
            }

            return View(testimonialpagecontent);
        }

        // GET: Testimonialpagecontents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Testimonialpagecontents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Testimonialpagecontentid,Contenttype,Content,Position,ImageFile")] Testimonialpagecontent testimonialpagecontent)
        {
            if (ModelState.IsValid)
            {
                if (testimonialpagecontent.ImageFile != null)
                {
                    string wwwrootPath = _webHostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + testimonialpagecontent.ImageFile.FileName;
                    string fullPath = Path.Combine(wwwrootPath + "/Image/TestimonialPageImage/", imageName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await testimonialpagecontent.ImageFile.CopyToAsync(fileStream);
                    }
                    testimonialpagecontent.Imagepath = imageName;
                }
                else
                {
                    testimonialpagecontent.Imagepath = null;
                }

                testimonialpagecontent.Position = 1;
                _context.Add(testimonialpagecontent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testimonialpagecontent);
        }

        // GET: Testimonialpagecontents/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Testimonialpagecontents == null)
            {
                return NotFound();
            }

            var testimonialpagecontent = await _context.Testimonialpagecontents.FindAsync(id);
            ViewBag.Testimonial=testimonialpagecontent;
            if (testimonialpagecontent == null)
            {
                return NotFound();
            }
            return View(testimonialpagecontent);
        }

        // POST: Testimonialpagecontents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Testimonialpagecontentid,Contenttype,Content,ImageFile")] Testimonialpagecontent testimonialpagecontent,string? imagepath)
        {
            if (id != testimonialpagecontent.Testimonialpagecontentid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                
                try
                {
                    if (testimonialpagecontent.ImageFile != null)
                    {
                        string wwwrootPath = _webHostEnvironment.WebRootPath;
                        string imageName = Guid.NewGuid().ToString() + "_" + testimonialpagecontent.ImageFile.FileName;
                        string fullPath = Path.Combine(wwwrootPath + "/Image/TestimonialPageImage/", imageName);
                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            await testimonialpagecontent.ImageFile.CopyToAsync(fileStream);
                        }
                        testimonialpagecontent.Imagepath = imageName;
                    }
                    else
                    {
                        testimonialpagecontent.Imagepath = imagepath;
                    }

                    testimonialpagecontent.Position = 1;
                    _context.Update(testimonialpagecontent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialpagecontentExists(testimonialpagecontent.Testimonialpagecontentid))
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
            return View(testimonialpagecontent);
        }

        // GET: Testimonialpagecontents/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Testimonialpagecontents == null)
            {
                return NotFound();
            }

            var testimonialpagecontent = await _context.Testimonialpagecontents
                .FirstOrDefaultAsync(m => m.Testimonialpagecontentid == id);
            if (testimonialpagecontent == null)
            {
                return NotFound();
            }

            return View(testimonialpagecontent);
        }

        // POST: Testimonialpagecontents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Testimonialpagecontents == null)
            {
                return Problem("Entity set 'ModelContext.Testimonialpagecontents'  is null.");
            }
            var testimonialpagecontent = await _context.Testimonialpagecontents.FindAsync(id);
            if (testimonialpagecontent != null)
            {
                _context.Testimonialpagecontents.Remove(testimonialpagecontent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestimonialpagecontentExists(decimal id)
        {
          return (_context.Testimonialpagecontents?.Any(e => e.Testimonialpagecontentid == id)).GetValueOrDefault();
        }
    }
}
