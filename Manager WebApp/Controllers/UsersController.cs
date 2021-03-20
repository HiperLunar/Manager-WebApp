using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Manager_WebApp.Data;
using Manager_WebApp.Models;
using Manager_WebApp.Models.UserViewModels;

namespace Manager_WebApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly ManagerContext _context;

        public UsersController(ManagerContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index(
            string sort,
            string currentFilter,
            string search,
            int? page)
        {
            ViewData["currentSort"] = sort;

            if (search != null) { page = 1; }
            else { search = currentFilter; }

            ViewData["currentFilter"] = search;

            var userList = from u in _context.User
                           select u;
            if (!String.IsNullOrEmpty(search))
            {
                userList = userList.Where(u => u.Name.Contains(search));
            }
            switch (sort)
            {
                case "Name":
                    userList = userList.OrderBy(u => u.Name);
                    break;
                case "Age":
                    userList = userList.OrderBy(u => u.Age);
                    break;
                case "Email":
                    userList = userList.OrderBy(u => u.Email);
                    break;
                case "Description":
                    userList = userList.OrderBy(s => s.Description);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<User>.CreateAsync(userList.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.Departments)
                .Include(u => u.Addresses)
                .ThenInclude(u => u.Address)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Age,Email,Description")] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            } catch (DbUpdateException ex)
            {
                if (ex.HResult == -2146233088)
                {
                    ModelState.AddModelError("", @"Unable to save changes.
                                                       User already exists");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.Addresses)
                .ThenInclude(u => u.Address)
                .AsNoTracking()
                .FirstOrDefaultAsync( u => u.ID == id);
            if (user == null)
            {
                return NotFound();
            }
            PopulateAssignedAddresses(user);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(Guid? id, string[] selectedAddresses)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.Addresses)
                .ThenInclude(u => u.Address)
                .FirstOrDefaultAsync(u => u.ID == id);
            if (await TryUpdateModelAsync<User>(
                user,
                "",
                u => u.Name, u => u.Age, u => u.Email, u => u.Description))
            {
                UpdateUserAddresses(selectedAddresses, user);
                try
                {
                    var query = await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = user.ID });
                }
                catch (DbUpdateException ex)
                {
                    if (ex.HResult == -2146233088)
                    {
                        ModelState.AddModelError("", @"Unable to save changes.
                                                       User already exists");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.");
                    }
                }
            }
            UpdateUserAddresses(selectedAddresses, user);
            PopulateAssignedAddresses(user);
            return View(user);
        }

        private void PopulateAssignedAddresses(User user)
        {
            var addresses = _context.Address;
            var userAddressIDs = new HashSet<Guid>(user.Addresses.Select(u => u.AddressID));
            var viewModel = new List<AssignedAddress>();
            foreach (var address in addresses)
            {
                viewModel.Add(new AssignedAddress
                { 
                    AddressID = address.ID,
                    Name = address.Information,
                    Assigned = userAddressIDs.Contains(address.ID)
                });
            }
            ViewBag.Addresses = viewModel;
        }

        private void UpdateUserAddresses(string[] addresses, User user)
        {
            if (addresses == null)
            {
                user.Addresses = new List<UserAddress>();
                return;
            }

            var addressesHS = new HashSet<string>(addresses);
            var userAddresses = new HashSet<Guid>
                (user.Addresses.Select(c => c.AddressID));
            foreach (var address in _context.Address)
            {
                if (addressesHS.Contains(address.ID.ToString()))
                {
                    if (!userAddresses.Contains(address.ID))
                    {
                        user.Addresses.Add(new UserAddress { UserID = user.ID, AddressID = address.ID });
                    }
                }
                else
                {
                    if (userAddresses.Contains(address.ID))
                    {
                        UserAddress addressToRemove = user.Addresses.FirstOrDefault<UserAddress>(u => u.AddressID == address.ID);
                        user.Addresses.Remove(addressToRemove);
                    }
                }
            }
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var user = await _context.User.FindAsync(id);
                _context.Entry(user).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool UserExists(Guid id)
        {
            return _context.User.Any(e => e.ID == id);
        }
    }
}
