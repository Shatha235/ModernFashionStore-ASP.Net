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
    public class ProductcolorsController : Controller
    {
        private readonly ModelContext _context;

        public ProductcolorsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Productcolors
        public async Task<IActionResult> Index()
        {
              return _context.Productcolors != null ? 
                          View(await _context.Productcolors.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Productcolors'  is null.");
        }

        // GET: Productcolors/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Productcolors == null)
            {
                return NotFound();
            }

            var productcolor = await _context.Productcolors
                .FirstOrDefaultAsync(m => m.Colorid == id);
            if (productcolor == null)
            {
                return NotFound();
            }

            return View(productcolor);
        }

        // GET: Productcolors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Productcolors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Colorid,Colorname")] Productcolor productcolor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productcolor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productcolor);
        }

        // GET: Productcolors/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Productcolors == null)
            {
                return NotFound();
            }

            var productcolor = await _context.Productcolors.FindAsync(id);
            if (productcolor == null)
            {
                return NotFound();
            }
            return View(productcolor);
        }

        // POST: Productcolors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Colorid,Colorname")] Productcolor productcolor)
        {
            if (id != productcolor.Colorid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productcolor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductcolorExists(productcolor.Colorid))
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
            return View(productcolor);
        }

        // GET: Productcolors/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Productcolors == null)
            {
                return NotFound();
            }

            var productcolor = await _context.Productcolors
                .FirstOrDefaultAsync(m => m.Colorid == id);
            if (productcolor == null)
            {
                return NotFound();
            }

            return View(productcolor);
        }

        // POST: Productcolors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Productcolors == null)
            {
                return Problem("Entity set 'ModelContext.Productcolors'  is null.");
            }
            var productcolor = await _context.Productcolors.FindAsync(id);
            if (productcolor != null)
            {
                _context.Productcolors.Remove(productcolor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductcolorExists(decimal id)
        {
          return (_context.Productcolors?.Any(e => e.Colorid == id)).GetValueOrDefault();
        }
    }
}
