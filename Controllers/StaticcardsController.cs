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
    public class StaticcardsController : Controller
    {
        private readonly ModelContext _context;

        public StaticcardsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Staticcards
        public async Task<IActionResult> Index()
        {
              return _context.Staticcards != null ? 
                          View(await _context.Staticcards.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Staticcards'  is null.");
        }

        // GET: Staticcards/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Staticcards == null)
            {
                return NotFound();
            }

            var staticcard = await _context.Staticcards
                .FirstOrDefaultAsync(m => m.Cardid == id);
            if (staticcard == null)
            {
                return NotFound();
            }

            return View(staticcard);
        }

        // GET: Staticcards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Staticcards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cardid,Cardnumber,Expirydate,Cvv,Balance")] Staticcard staticcard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(staticcard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(staticcard);
        }

        // GET: Staticcards/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Staticcards == null)
            {
                return NotFound();
            }

            var staticcard = await _context.Staticcards.FindAsync(id);
            if (staticcard == null)
            {
                return NotFound();
            }
            return View(staticcard);
        }

        // POST: Staticcards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Cardid,Cardnumber,Expirydate,Cvv,Balance")] Staticcard staticcard)
        {
            if (id != staticcard.Cardid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staticcard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaticcardExists(staticcard.Cardid))
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
            return View(staticcard);
        }

        // GET: Staticcards/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Staticcards == null)
            {
                return NotFound();
            }

            var staticcard = await _context.Staticcards
                .FirstOrDefaultAsync(m => m.Cardid == id);
            if (staticcard == null)
            {
                return NotFound();
            }

            return View(staticcard);
        }

        // POST: Staticcards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Staticcards == null)
            {
                return Problem("Entity set 'ModelContext.Staticcards'  is null.");
            }
            var staticcard = await _context.Staticcards.FindAsync(id);
            if (staticcard != null)
            {
                _context.Staticcards.Remove(staticcard);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaticcardExists(decimal id)
        {
          return (_context.Staticcards?.Any(e => e.Cardid == id)).GetValueOrDefault();
        }
    }
}
