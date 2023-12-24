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
    public class ProductattributesController : Controller
    {
        private readonly ModelContext _context;

        public ProductattributesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Productattributes
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Productattributes.Include(p => p.Color).Include(p => p.Product).Include(p => p.Size);
            return View(await modelContext.ToListAsync());
        }

        // GET: Productattributes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Productattributes == null)
            {
                return NotFound();
            }

            var productattribute = await _context.Productattributes
                .Include(p => p.Color)
                .Include(p => p.Product)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.Productattributeid == id);
            if (productattribute == null)
            {
                return NotFound();
            }

            return View(productattribute);
        }

        // GET: Productattributes/Create
        public IActionResult Create()
        {
            ViewData["Colorid"] = new SelectList(_context.Productcolors, "Colorid", "Colorid");
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid");
            ViewData["Sizeid"] = new SelectList(_context.Productsizes, "Sizeid", "Sizeid");
            return View();
        }

        // POST: Productattributes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Productattributeid,Productid,Colorid,Sizeid,Quantity")] Productattribute productattribute)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productattribute);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Colorid"] = new SelectList(_context.Productcolors, "Colorid", "Colorid", productattribute.Colorid);
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid", productattribute.Productid);
            ViewData["Sizeid"] = new SelectList(_context.Productsizes, "Sizeid", "Sizeid", productattribute.Sizeid);
            return View(productattribute);
        }

        // GET: Productattributes/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Productattributes == null)
            {
                return NotFound();
            }

            var productattribute = await _context.Productattributes.FindAsync(id);
            if (productattribute == null)
            {
                return NotFound();
            }
            ViewData["Colorid"] = new SelectList(_context.Productcolors, "Colorid", "Colorid", productattribute.Colorid);
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid", productattribute.Productid);
            ViewData["Sizeid"] = new SelectList(_context.Productsizes, "Sizeid", "Sizeid", productattribute.Sizeid);
            return View(productattribute);
        }

        // POST: Productattributes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Productattributeid,Productid,Colorid,Sizeid,Quantity")] Productattribute productattribute)
        {
            if (id != productattribute.Productattributeid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productattribute);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductattributeExists(productattribute.Productattributeid))
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
            ViewData["Colorid"] = new SelectList(_context.Productcolors, "Colorid", "Colorid", productattribute.Colorid);
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid", productattribute.Productid);
            ViewData["Sizeid"] = new SelectList(_context.Productsizes, "Sizeid", "Sizeid", productattribute.Sizeid);
            return View(productattribute);
        }

        // GET: Productattributes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Productattributes == null)
            {
                return NotFound();
            }

            var productattribute = await _context.Productattributes
                .Include(p => p.Color)
                .Include(p => p.Product)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.Productattributeid == id);
            if (productattribute == null)
            {
                return NotFound();
            }

            return View(productattribute);
        }

        // POST: Productattributes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Productattributes == null)
            {
                return Problem("Entity set 'ModelContext.Productattributes'  is null.");
            }
            var productattribute = await _context.Productattributes.FindAsync(id);
            if (productattribute != null)
            {
                _context.Productattributes.Remove(productattribute);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductattributeExists(decimal id)
        {
          return (_context.Productattributes?.Any(e => e.Productattributeid == id)).GetValueOrDefault();
        }
    }
}
