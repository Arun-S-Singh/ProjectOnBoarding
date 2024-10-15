
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectOnBoarding.Data;
using ProjectOnBoarding.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ProjectOnBoarding.Controllers
{
    public class BrandController : Controller
    {
        private readonly ProjectDBContext _context;
        private readonly ILogger<CompanyController> _logger;

        public BrandController(ProjectDBContext context, ILogger<CompanyController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Brand
        public async Task<IActionResult> Index()
        {
            var projectDBContext = _context.Brands.Include(b => b.Division);
            return View(await projectDBContext.ToListAsync());
        }

        // GET: Brand/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .Include(b => b.Division)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // GET: Brand/Create
        public IActionResult Create()
        {
            ViewData["DivisionId"] = new SelectList(_context.Divisions, "Id", "Name");
            return View();
        }

        // POST: Brand/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DivisionId")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brand);
                await _context.SaveChangesAsync();
                _logger.LogInformation("[APPEVENT] New Brand [ Id={0},Name={1} ] added by {2}", brand.Id, brand.Name, User.Identity.Name);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DivisionId"] = new SelectList(_context.Divisions, "Id", "Name", brand.DivisionId);
            return View(brand);
        }

        // GET: Brand/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            ViewData["DivisionId"] = new SelectList(_context.Divisions, "Id", "Name", brand.DivisionId);
            return View(brand);
        }

        // POST: Brand/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DivisionId")] Brand brand)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brand);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("[APPEVENT] Brand [ Id={0},Name={1} ] edited by {2}", brand.Id, brand.Name, User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.Id))
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
            ViewData["DivisionId"] = new SelectList(_context.Divisions, "Id", "Name", brand.DivisionId);
            return View(brand);
        }

        // GET: Brand/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .Include(b => b.Division)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Brand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("[APPEVENT] Brand [ Id={0},Name={1} ] deleted by {2}", brand.Id, brand.Name, User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        private bool BrandExists(int id)
        {
            return _context.Brands.Any(e => e.Id == id);
        }
    }
}
