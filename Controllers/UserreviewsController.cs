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
    public class UserreviewsController : Controller
    {
        private readonly ModelContext _context;

        public UserreviewsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Userreviews
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Userreviews.Include(u => u.Product).Include(u => u.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Userreviews/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Userreviews == null)
            {
                return NotFound();
            }

            var userreview = await _context.Userreviews
                .Include(u => u.Product)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Reviewid == id);
            if (userreview == null)
            {
                return NotFound();
            }

            return View(userreview);
        }

        // GET: Userreviews/Create
        public IActionResult Create()
        {
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid");
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid");
            return View();
        }

        // POST: Userreviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Reviewid,Userid,Productid,Comments")] Userreview userreview)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userreview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid", userreview.Productid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", userreview.Userid);
            return View(userreview);
        }

        // GET: Userreviews/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Userreviews == null)
            {
                return NotFound();
            }

            var userreview = await _context.Userreviews.FindAsync(id);
            if (userreview == null)
            {
                return NotFound();
            }
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid", userreview.Productid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", userreview.Userid);
            return View(userreview);
        }

        // POST: Userreviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Reviewid,Userid,Productid,Comments")] Userreview userreview)
        {
            if (id != userreview.Reviewid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userreview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserreviewExists(userreview.Reviewid))
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
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid", userreview.Productid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", userreview.Userid);
            return View(userreview);
        }

        // GET: Userreviews/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Userreviews == null)
            {
                return NotFound();
            }

            var userreview = await _context.Userreviews
                .Include(u => u.Product)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Reviewid == id);
            if (userreview == null)
            {
                return NotFound();
            }

            return View(userreview);
        }

        // POST: Userreviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Userreviews == null)
            {
                return Problem("Entity set 'ModelContext.Userreviews'  is null.");
            }
            var userreview = await _context.Userreviews.FindAsync(id);
            if (userreview != null)
            {
                _context.Userreviews.Remove(userreview);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserreviewExists(decimal id)
        {
          return (_context.Userreviews?.Any(e => e.Reviewid == id)).GetValueOrDefault();
        }
    }
}
