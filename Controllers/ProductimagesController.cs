using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class ProductimagesController : Controller
    {
        private readonly ModelContext _context;

        public ProductimagesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Productimages
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Productimages.Include(p => p.Product);
            return View(await modelContext.ToListAsync());
        }

        // GET: Productimages/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Productimages == null)
            {
                return NotFound();
            }

            var productimage = await _context.Productimages
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Imageid == id);
            if (productimage == null)
            {
                return NotFound();
            }

            return View(productimage);
        }

        // GET: Productimages/Create
        public IActionResult Create()
        {
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid");
            return View();
        }

        // POST: Productimages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Imageid,Productid,Imagepath")] Productimage productimage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productimage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid", productimage.Productid);
            return View(productimage);
        }

        // GET: Productimages/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Productimages == null)
            {
                return NotFound();
            }

            var productimage = await _context.Productimages.FindAsync(id);
            if (productimage == null)
            {
                return NotFound();
            }
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid", productimage.Productid);
            return View(productimage);
        }

        // POST: Productimages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Imageid,Productid,Imagepath")] Productimage productimage)
        {
            if (id != productimage.Imageid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productimage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductimageExists(productimage.Imageid))
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
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid", productimage.Productid);
            return View(productimage);
        }

        // GET: Productimages/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Productimages == null)
            {
                return NotFound();
            }

            var productimage = await _context.Productimages
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Imageid == id);
            if (productimage == null)
            {
                return NotFound();
            }

            return View(productimage);
        }

        // POST: Productimages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Productimages == null)
            {
                return Problem("Entity set 'ModelContext.Productimages'  is null.");
            }
            var productimage = await _context.Productimages.FindAsync(id);
            if (productimage != null)
            {
                _context.Productimages.Remove(productimage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductimageExists(decimal id)
        {
          return (_context.Productimages?.Any(e => e.Imageid == id)).GetValueOrDefault();
        }
    }
}
