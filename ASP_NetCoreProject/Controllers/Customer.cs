using ASP_NetCoreProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_NetCoreProject.Controllers
{
    public class Customer : Controller
    {
        private readonly ApplicationDbContext _context;

        public Customer(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {

            var customers = _context.customers.Include(c => c.City).ThenInclude(e => e.Country);
            return View(await customers.ToListAsync());
        }


        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.customers
                .Include(c => c.City).ThenInclude(c => c.Country)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Country"] = new SelectList(_context.countries, "ID", "Name");
            ViewData["CityId"] = new SelectList(_context.Cities.Include(e => e.Country).ToList(), "ID", "Name", null, "Country.Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,CityId,Phone")] Data.Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Country"] = new SelectList(_context.countries, "ID", "Name");
            ViewData["CityId"] = new SelectList(_context.Cities, "ID", "Name", customer.CityId);
            return View(customer);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["Country"] = new SelectList(_context.countries, "ID", "Name");
            ViewData["CityId"] = new SelectList(_context.Cities.Include(e => e.Country).ToList(), "ID", "Name", customer.CityId, "Country.Name");
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,FirstName,LastName,CityId,Phone")] Data.Customer customer)
        {
            if (id != customer.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.ID))
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
            ViewData["Country"] = new SelectList(_context.countries, "ID", "Name", customer.City.CountryId);
            ViewData["CityId"] = new SelectList(_context.Cities.Include(e => e.Country).ToList(), "ID", "Name", customer.CityId, "Country.Name");
            return View(customer);
        }

        private bool CustomerExists(long id)
        {
            return _context.customers.Any(e => e.ID == id);
        }



        [HttpGet]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.customers
                .Include(c => c.City).ThenInclude(c => c.Country)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var customer = await _context.customers.FindAsync(id);
            _context.customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public JsonResult GetCities(long countryId)
        {
            List<Data.City> cities = _context.Cities.Include(e => e.Country).Where(e => e.CountryId == countryId).ToList();
            return Json(new SelectList(cities, "ID", "Name", null, "Country.Name"));
        }
    }
}
