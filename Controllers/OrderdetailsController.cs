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
    public class OrderdetailsController : Controller
    {
        private readonly ModelContext _context;

        public OrderdetailsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Orderdetails
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Orderdetails.Include(o => o.Product);
            return View(await modelContext.ToListAsync());
        }

        // GET: Orderdetails/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Orderdetails == null)
            {
                return NotFound();
            }

            var orderdetail = await _context.Orderdetails
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Orderdetailid == id);
            if (orderdetail == null)
            {
                return NotFound();
            }

            return View(orderdetail);
        }

        // GET: Orderdetails/Create
        public IActionResult Create()
        {
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid");
            return View();
        }

        // POST: Orderdetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Orderdetailid,Orderid,Productid,Quantity")] Orderdetail orderdetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderdetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid", orderdetail.Productid);
            return View(orderdetail);
        }

        // GET: Orderdetails/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Orderdetails == null)
            {
                return NotFound();
            }

            var orderdetail = await _context.Orderdetails.FindAsync(id);
            if (orderdetail == null)
            {
                return NotFound();
            }
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid", orderdetail.Productid);
            return View(orderdetail);
        }

        // POST: Orderdetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Orderdetailid,Orderid,Productid,Quantity")] Orderdetail orderdetail)
        {
            if (id != orderdetail.Orderdetailid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderdetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderdetailExists(orderdetail.Orderdetailid))
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
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid", orderdetail.Productid);
            return View(orderdetail);
        }

        // GET: Orderdetails/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Orderdetails == null)
            {
                return NotFound();
            }

            var orderdetail = await _context.Orderdetails
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Orderdetailid == id);
            if (orderdetail == null)
            {
                return NotFound();
            }

            return View(orderdetail);
        }

        // POST: Orderdetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Orderdetails == null)
            {
                return Problem("Entity set 'ModelContext.Orderdetails'  is null.");
            }
            var orderdetail = await _context.Orderdetails.FindAsync(id);
            if (orderdetail != null)
            {
                _context.Orderdetails.Remove(orderdetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderdetailExists(decimal id)
        {
          return (_context.Orderdetails?.Any(e => e.Orderdetailid == id)).GetValueOrDefault();
        }
    }
}
