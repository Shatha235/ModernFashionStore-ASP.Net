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
    public class CategorytypesController : Controller
    {
        private readonly ModelContext _context;

        public CategorytypesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Categorytypes
        public async Task<IActionResult> Index()
        {
              return _context.Categorytypes != null ? 
                          View(await _context.Categorytypes.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Categorytypes'  is null.");
        }

        // GET: Categorytypes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Categorytypes == null)
            {
                return NotFound();
            }

            var categorytype = await _context.Categorytypes
                .FirstOrDefaultAsync(m => m.Categorytypeid == id);
            if (categorytype == null)
            {
                return NotFound();
            }

            return View(categorytype);
        }

        // GET: Categorytypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorytypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Categorytypeid,Categorytypename")] Categorytype categorytype)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categorytype);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categorytype);
        }

        // GET: Categorytypes/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Categorytypes == null)
            {
                return NotFound();
            }

            var categorytype = await _context.Categorytypes.FindAsync(id);
            if (categorytype == null)
            {
                return NotFound();
            }
            return View(categorytype);
        }

        // POST: Categorytypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Categorytypeid,Categorytypename")] Categorytype categorytype)
        {
            if (id != categorytype.Categorytypeid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categorytype);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategorytypeExists(categorytype.Categorytypeid))
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
            return View(categorytype);
        }

        // GET: Categorytypes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Categorytypes == null)
            {
                return NotFound();
            }

            var categorytype = await _context.Categorytypes
                .FirstOrDefaultAsync(m => m.Categorytypeid == id);
            if (categorytype == null)
            {
                return NotFound();
            }

            return View(categorytype);
        }

        // POST: Categorytypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Categorytypes == null)
            {
                return Problem("Entity set 'ModelContext.Categorytypes'  is null.");
            }
            var categorytype = await _context.Categorytypes.FindAsync(id);
            if (categorytype != null)
            {
                _context.Categorytypes.Remove(categorytype);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategorytypeExists(decimal id)
        {
          return (_context.Categorytypes?.Any(e => e.Categorytypeid == id)).GetValueOrDefault();
        }
    }
}
