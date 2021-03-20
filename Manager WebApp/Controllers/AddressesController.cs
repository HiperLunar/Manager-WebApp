using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Manager_WebApp.Data;
using Manager_WebApp.Models;

namespace Manager_WebApp.Controllers
{
    public class AddressesController : Controller
    {
        private readonly ManagerContext _context;

        public AddressesController(ManagerContext context)
        {
            _context = context;
        }

        // GET: Addresses
        public async Task<IActionResult> Index(
            string currentFilter,
            string search,
            int? page)
        {
            if (search != null) { page = 1; }
            else { search = currentFilter; }

            ViewData["currentFilter"] = search;

            var addressList = from a in _context.Address
                           select a;
            if (!String.IsNullOrEmpty(search))
            {
                addressList = addressList.Where(u => u.Information.Contains(search));
            }
            addressList = addressList.OrderBy(a => a.Information);

            int pageSize = 10;
            return View(await PaginatedList<Address>.CreateAsync(addressList.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Addresses/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Address
                .Include(a => a.Users)
                .ThenInclude(ua => ua.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // GET: Addresses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Information,IsCommercial")] Address address)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(address);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                } catch (DbUpdateException ex)
                {
                    if (ex.HResult == -2146233088)
                    {
                        ModelState.AddModelError("", @"Unable to save changes.
                                                       Address already exists");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.");
                    }
                }
            }
            return View(address);
        }

        // GET: Addresses/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Address.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Information,IsCommercial")] Address address)
        {
            if (id != address.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.HResult == -2146233088)
                    {
                        ModelState.AddModelError("", @"Unable to save changes.
                                                       Name already exists");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.");
                    }
                }
            }
            return View(address);
        }

        // GET: Addresses/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Address
                .FirstOrDefaultAsync(m => m.ID == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var address = await _context.Address.FindAsync(id);
            _context.Entry(address).State =  EntityState.Deleted;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressExists(Guid id)
        {
            return _context.Address.Any(e => e.ID == id);
        }
    }
}
