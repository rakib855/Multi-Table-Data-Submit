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
    public class Supplier : Controller
    {
        private readonly ApplicationDbContext _context;

        public Supplier(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {

            var suppliers = _context.Suppliers.Include(s => s.City).ThenInclude(s => s.Country);
            return View(await suppliers.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .Include(s => s.City).ThenInclude(s => s.Country)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
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
        public async Task<IActionResult> Create([Bind("ID,CompanyName,ContactName,ContactTitle,CityId,Phone,Fax")] Data.Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Country"] = new SelectList(_context.countries, "ID", "Name");
            ViewData["CityId"] = new SelectList(_context.Cities.Include(e => e.Country).ToList(), "ID", "Name", supplier.CityId, "Country.Name");
            return View(supplier);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            ViewData["Country"] = new SelectList(_context.countries, "ID", "Name");
            ViewData["CityId"] = new SelectList(_context.Cities.Include(e => e.Country).ToList(), "ID", "Name", supplier.CityId, "Country.Name");
            return View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,CompanyName,ContactName,ContactTitle,CityId,Phone,Fax")] Data.Supplier supplier)
        {
            if (id != supplier.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.ID))
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
            ViewData["Country"] = new SelectList(_context.countries, "ID", "Name");
            ViewData["CityId"] = new SelectList(_context.Cities.Include(e => e.Country).ToList(), "ID", "Name", supplier.CityId, "Country.Name");
            return View(supplier);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .Include(s => s.City).ThenInclude(s => s.Country)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public JsonResult GetCities(long countryId)
        {
            List<Data.City> cities = _context.Cities.Include(s => s.Country).Where(s => s.CountryId == countryId).ToList();
            return Json(new SelectList(cities, "ID", "Name", null, "Country.Name"));
        }

        private bool SupplierExists(long id)
        {
            return _context.Suppliers.Any(e => e.ID == id);
        }
    }
}
