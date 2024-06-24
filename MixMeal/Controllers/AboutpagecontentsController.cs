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
    public class AboutpagecontentsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AboutpagecontentsController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Aboutpagecontents
        public async Task<IActionResult> Index()
        {
              return _context.Aboutpagecontents != null ? 
                          View(await _context.Aboutpagecontents.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Aboutpagecontents'  is null.");
        }

        // GET: Aboutpagecontents/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Aboutpagecontents == null)
            {
                return NotFound();
            }

            var aboutpagecontent = await _context.Aboutpagecontents
                .FirstOrDefaultAsync(m => m.Aboutpagecontentid == id);
            if (aboutpagecontent == null)
            {
                return NotFound();
            }

            return View(aboutpagecontent);
        }

        // GET: Aboutpagecontents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aboutpagecontents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Aboutpagecontentid,Contenttype,Content,Position,ImageFile")] Aboutpagecontent aboutpagecontent)
        {
            if (ModelState.IsValid)
            {
                if (aboutpagecontent.ImageFile != null)
                {
                    string wwwrootPath = _webHostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + aboutpagecontent.ImageFile.FileName;
                    string fullPath = Path.Combine(wwwrootPath + "/Image/AboutPageImage/", imageName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await aboutpagecontent.ImageFile.CopyToAsync(fileStream);
                    }
                    aboutpagecontent.Imagepath = imageName;
                }
                else
                {
                    aboutpagecontent.Imagepath = null;
                }
                aboutpagecontent.Position = 1;
                _context.Add(aboutpagecontent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aboutpagecontent);
        }

        // GET: Aboutpagecontents/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Aboutpagecontents == null)
            {
                return NotFound();
            }

            var aboutpagecontent = await _context.Aboutpagecontents.FindAsync(id);
            ViewBag.image = aboutpagecontent;
            if (aboutpagecontent == null)
            {
                return NotFound();
            }
            return View(aboutpagecontent);
        }

        // POST: Aboutpagecontents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Aboutpagecontentid,Contenttype,Content,ImageFile")] Aboutpagecontent aboutpagecontent , string? imagepath)
        {
            if (id != aboutpagecontent.Aboutpagecontentid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (aboutpagecontent.ImageFile != null)
                    {
                        string wwwrootPath = _webHostEnvironment.WebRootPath;
                        string imageName = Guid.NewGuid().ToString() + "_" + aboutpagecontent.ImageFile.FileName;
                        string fullPath = Path.Combine(wwwrootPath + "/Image/AboutPageImage/", imageName);
                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            await aboutpagecontent.ImageFile.CopyToAsync(fileStream);
                        }
                        aboutpagecontent.Imagepath = imageName;
                    }
                    else
                    {
                        aboutpagecontent.Imagepath = imagepath;
                    }
                    aboutpagecontent.Position = 1;
                    _context.Update(aboutpagecontent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutpagecontentExists(aboutpagecontent.Aboutpagecontentid))
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
            return View(aboutpagecontent);
        }

        // GET: Aboutpagecontents/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Aboutpagecontents == null)
            {
                return NotFound();
            }

            var aboutpagecontent = await _context.Aboutpagecontents
                .FirstOrDefaultAsync(m => m.Aboutpagecontentid == id);
            if (aboutpagecontent == null)
            {
                return NotFound();
            }

            return View(aboutpagecontent);
        }

        // POST: Aboutpagecontents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Aboutpagecontents == null)
            {
                return Problem("Entity set 'ModelContext.Aboutpagecontents'  is null.");
            }
            var aboutpagecontent = await _context.Aboutpagecontents.FindAsync(id);
            if (aboutpagecontent != null)
            {
                _context.Aboutpagecontents.Remove(aboutpagecontent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutpagecontentExists(decimal id)
        {
          return (_context.Aboutpagecontents?.Any(e => e.Aboutpagecontentid == id)).GetValueOrDefault();
        }
    }
}
