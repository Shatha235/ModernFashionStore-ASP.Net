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
    public class ProducttypesController : Controller
    {
        private readonly ModelContext _context;

        public ProducttypesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Producttypes
        public async Task<IActionResult> Index()
        {
              return _context.Producttypes != null ? 
                          View(await _context.Producttypes.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Producttypes'  is null.");
        }

        // GET: Producttypes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Producttypes == null)
            {
                return NotFound();
            }

            var producttype = await _context.Producttypes
                .FirstOrDefaultAsync(m => m.Typeid == id);
            if (producttype == null)
            {
                return NotFound();
            }

            return View(producttype);
        }

        // GET: Producttypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Producttypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Typeid,Typename,Imagepath,Categorytypeid")] Producttype producttype)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producttype);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producttype);
        }

        // GET: Producttypes/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Producttypes == null)
            {
                return NotFound();
            }

            var producttype = await _context.Producttypes.FindAsync(id);
            if (producttype == null)
            {
                return NotFound();
            }
            return View(producttype);
        }

        // POST: Producttypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Typeid,Typename,Imagepath,Categorytypeid")] Producttype producttype)
        {
            if (id != producttype.Typeid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producttype);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProducttypeExists(producttype.Typeid))
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
            return View(producttype);
        }

        // GET: Producttypes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Producttypes == null)
            {
                return NotFound();
            }

            var producttype = await _context.Producttypes
                .FirstOrDefaultAsync(m => m.Typeid == id);
            if (producttype == null)
            {
                return NotFound();
            }

            return View(producttype);
        }

        // POST: Producttypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Producttypes == null)
            {
                return Problem("Entity set 'ModelContext.Producttypes'  is null.");
            }
            var producttype = await _context.Producttypes.FindAsync(id);
            if (producttype != null)
            {
                _context.Producttypes.Remove(producttype);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProducttypeExists(decimal id)
        {
          return (_context.Producttypes?.Any(e => e.Typeid == id)).GetValueOrDefault();
        }
    }
}
