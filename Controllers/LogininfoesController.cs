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
    public class LogininfoesController : Controller
    {
        private readonly ModelContext _context;

        public LogininfoesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Logininfoes
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Logininfos.Include(l => l.User).Include(l => l.Userrole);
            return View(await modelContext.ToListAsync());
        }

        // GET: Logininfoes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Logininfos == null)
            {
                return NotFound();
            }

            var logininfo = await _context.Logininfos
                .Include(l => l.User)
                .Include(l => l.Userrole)
                .FirstOrDefaultAsync(m => m.Loginid == id);
            if (logininfo == null)
            {
                return NotFound();
            }

            return View(logininfo);
        }

        // GET: Logininfoes/Create
        public IActionResult Create()
        {
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid");
            ViewData["Userroleid"] = new SelectList(_context.Roles, "Roleid", "Roleid");
            return View();
        }

        // POST: Logininfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Loginid,Userid,Email,Password,Userroleid")] Logininfo logininfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(logininfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", logininfo.Userid);
            ViewData["Userroleid"] = new SelectList(_context.Roles, "Roleid", "Roleid", logininfo.Userroleid);
            return View(logininfo);
        }

        // GET: Logininfoes/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Logininfos == null)
            {
                return NotFound();
            }

            var logininfo = await _context.Logininfos.FindAsync(id);
            if (logininfo == null)
            {
                return NotFound();
            }
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", logininfo.Userid);
            ViewData["Userroleid"] = new SelectList(_context.Roles, "Roleid", "Roleid", logininfo.Userroleid);
            return View(logininfo);
        }

        // POST: Logininfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Loginid,Userid,Email,Password,Userroleid")] Logininfo logininfo)
        {
            if (id != logininfo.Loginid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(logininfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LogininfoExists(logininfo.Loginid))
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
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", logininfo.Userid);
            ViewData["Userroleid"] = new SelectList(_context.Roles, "Roleid", "Roleid", logininfo.Userroleid);
            return View(logininfo);
        }

        // GET: Logininfoes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Logininfos == null)
            {
                return NotFound();
            }

            var logininfo = await _context.Logininfos
                .Include(l => l.User)
                .Include(l => l.Userrole)
                .FirstOrDefaultAsync(m => m.Loginid == id);
            if (logininfo == null)
            {
                return NotFound();
            }

            return View(logininfo);
        }

        // POST: Logininfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Logininfos == null)
            {
                return Problem("Entity set 'ModelContext.Logininfos'  is null.");
            }
            var logininfo = await _context.Logininfos.FindAsync(id);
            if (logininfo != null)
            {
                _context.Logininfos.Remove(logininfo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LogininfoExists(decimal id)
        {
          return (_context.Logininfos?.Any(e => e.Loginid == id)).GetValueOrDefault();
        }
    }
}
