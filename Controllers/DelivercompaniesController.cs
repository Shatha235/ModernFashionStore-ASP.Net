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
    public class DelivercompaniesController : Controller
    {
        private readonly ModelContext _context;

        public DelivercompaniesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Delivercompanies
        public async Task<IActionResult> Index()
        {
              return _context.Delivercompanies != null ? 
                          View(await _context.Delivercompanies.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Delivercompanies'  is null.");
        }

        // GET: Delivercompanies/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Delivercompanies == null)
            {
                return NotFound();
            }

            var delivercompany = await _context.Delivercompanies
                .FirstOrDefaultAsync(m => m.Delivercompanyid == id);
            if (delivercompany == null)
            {
                return NotFound();
            }

            return View(delivercompany);
        }

        // GET: Delivercompanies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Delivercompanies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Delivercompanyid,Delivercompanyname,Delivercompanyaddress")] Delivercompany delivercompany)
        {
            if (ModelState.IsValid)
            {
                _context.Add(delivercompany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(delivercompany);
        }

        // GET: Delivercompanies/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Delivercompanies == null)
            {
                return NotFound();
            }

            var delivercompany = await _context.Delivercompanies.FindAsync(id);
            if (delivercompany == null)
            {
                return NotFound();
            }
            return View(delivercompany);
        }

        // POST: Delivercompanies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Delivercompanyid,Delivercompanyname,Delivercompanyaddress")] Delivercompany delivercompany)
        {
            if (id != delivercompany.Delivercompanyid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(delivercompany);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DelivercompanyExists(delivercompany.Delivercompanyid))
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
            return View(delivercompany);
        }

        // GET: Delivercompanies/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Delivercompanies == null)
            {
                return NotFound();
            }

            var delivercompany = await _context.Delivercompanies
                .FirstOrDefaultAsync(m => m.Delivercompanyid == id);
            if (delivercompany == null)
            {
                return NotFound();
            }

            return View(delivercompany);
        }

        // POST: Delivercompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Delivercompanies == null)
            {
                return Problem("Entity set 'ModelContext.Delivercompanies'  is null.");
            }
            var delivercompany = await _context.Delivercompanies.FindAsync(id);
            if (delivercompany != null)
            {
                _context.Delivercompanies.Remove(delivercompany);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DelivercompanyExists(decimal id)
        {
          return (_context.Delivercompanies?.Any(e => e.Delivercompanyid == id)).GetValueOrDefault();
        }
    }
}
