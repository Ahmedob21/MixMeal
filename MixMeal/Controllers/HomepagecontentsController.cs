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
    public class HomepagecontentsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomepagecontentsController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Homepagecontents
        public async Task<IActionResult> Index()
        {
              return _context.Homepagecontents != null ? 
                          View(await _context.Homepagecontents.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Homepagecontents'  is null.");
        }

        // GET: Homepagecontents/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Homepagecontents == null)
            {
                return NotFound();
            }

            var homepagecontent = await _context.Homepagecontents
                .FirstOrDefaultAsync(m => m.Homepagecontentid == id);
            if (homepagecontent == null)
            {
                return NotFound();
            }

            return View(homepagecontent);
        }

        // GET: Homepagecontents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Homepagecontents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Homepagecontentid,Contenttype,Content,ImageFile")] Homepagecontent homepagecontent)
        {
            if (ModelState.IsValid)
            {
                if (homepagecontent.ImageFile != null)
                {
                    string wwwrootPath = _webHostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + homepagecontent.ImageFile.FileName;
                    string fullPath = Path.Combine(wwwrootPath + "/Image/HomePageImage/", imageName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await homepagecontent.ImageFile.CopyToAsync(fileStream);
                    }
                    homepagecontent.Imagepath = imageName;
                }
                else
                {
                    homepagecontent.Imagepath = null;
                }
                homepagecontent.Position = 1;
                _context.Add(homepagecontent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homepagecontent);
        }

        // GET: Homepagecontents/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Homepagecontents == null)
            {
                return NotFound();
            }

            var homepagecontent = await _context.Homepagecontents.FindAsync(id);
            ViewBag.image = homepagecontent;
            if (homepagecontent == null)
            {
                return NotFound();
            }
            return View(homepagecontent);
        }

        // POST: Homepagecontents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Homepagecontentid,Contenttype,Content,ImageFile")] Homepagecontent homepagecontent, string? imagepath)
        {
            //var contentid = _context.Homepagecontents.Find(id);
            
            if (id != homepagecontent.Homepagecontentid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (homepagecontent.ImageFile != null)
                    {
                        string wwwrootPath = _webHostEnvironment.WebRootPath;
                        string imageName = Guid.NewGuid().ToString() + "_" + homepagecontent.ImageFile.FileName;
                        string fullPath = Path.Combine(wwwrootPath + "/Image/AboutPageImage/", imageName);
                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            await homepagecontent.ImageFile.CopyToAsync(fileStream);
                        }
                        homepagecontent.Imagepath = imageName;
                    }
                    else
                    {
                        homepagecontent.Imagepath = imagepath;
                    }
                    homepagecontent.Position = 1;
                    _context.Update(homepagecontent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomepagecontentExists(homepagecontent.Homepagecontentid))
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
            return View(homepagecontent);
        }

        // GET: Homepagecontents/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Homepagecontents == null)
            {
                return NotFound();
            }

            var homepagecontent = await _context.Homepagecontents
                .FirstOrDefaultAsync(m => m.Homepagecontentid == id);
            if (homepagecontent == null)
            {
                return NotFound();
            }

            return View(homepagecontent);
        }

        // POST: Homepagecontents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Homepagecontents == null)
            {
                return Problem("Entity set 'ModelContext.Homepagecontents'  is null.");
            }
            var homepagecontent = await _context.Homepagecontents.FindAsync(id);
            if (homepagecontent != null)
            {
                _context.Homepagecontents.Remove(homepagecontent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomepagecontentExists(decimal id)
        {
          return (_context.Homepagecontents?.Any(e => e.Homepagecontentid == id)).GetValueOrDefault();
        }
    }
}
