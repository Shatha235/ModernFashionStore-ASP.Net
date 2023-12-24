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
    public class ProductsizesController : Controller
    {
        private readonly ModelContext _context;

        public ProductsizesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Productsizes
        public async Task<IActionResult> Index()
        {
              return _context.Productsizes != null ? 
                          View(await _context.Productsizes.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Productsizes'  is null.");
        }

        // GET: Productsizes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Productsizes == null)
            {
                return NotFound();
            }

            var productsize = await _context.Productsizes
                .FirstOrDefaultAsync(m => m.Sizeid == id);
            if (productsize == null)
            {
                return NotFound();
            }

            return View(productsize);
        }

        // GET: Productsizes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Productsizes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Sizeid,Sizename")] Productsize productsize)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productsize);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productsize);
        }

        // GET: Productsizes/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Productsizes == null)
            {
                return NotFound();
            }

            var productsize = await _context.Productsizes.FindAsync(id);
            if (productsize == null)
            {
                return NotFound();
            }
            return View(productsize);
        }

        // POST: Productsizes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Sizeid,Sizename")] Productsize productsize)
        {
            if (id != productsize.Sizeid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productsize);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsizeExists(productsize.Sizeid))
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
            return View(productsize);
        }

        // GET: Productsizes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Productsizes == null)
            {
                return NotFound();
            }

            var productsize = await _context.Productsizes
                .FirstOrDefaultAsync(m => m.Sizeid == id);
            if (productsize == null)
            {
                return NotFound();
            }

            return View(productsize);
        }

        // POST: Productsizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Productsizes == null)
            {
                return Problem("Entity set 'ModelContext.Productsizes'  is null.");
            }
            var productsize = await _context.Productsizes.FindAsync(id);
            if (productsize != null)
            {
                _context.Productsizes.Remove(productsize);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsizeExists(decimal id)
        {
          return (_context.Productsizes?.Any(e => e.Sizeid == id)).GetValueOrDefault();
        }
    }
}
