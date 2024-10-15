
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectOnBoarding.Data;
using ProjectOnBoarding.Models;


namespace ProjectOnBoarding.Controllers
{
    public class DivisionController : Controller
    {
        private readonly ProjectDBContext _context;
        private readonly ILogger<CompanyController> _logger;
        public DivisionController(ProjectDBContext context, ILogger<CompanyController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Division
        public async Task<IActionResult> Index()
        {
            var projectDBContext = _context.Divisions.Include(d => d.Company);
            return View(await projectDBContext.ToListAsync());
        }

        // GET: Division/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var division = await _context.Divisions
                .Include(d => d.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (division == null)
            {
                return NotFound();
            }

            return View(division);
        }

        // GET: Division/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "CompanyName");
            return View();
        }

        // POST: Division/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CompanyId")] Division division)
        {
            if (ModelState.IsValid)
            {
                _context.Add(division);
                await _context.SaveChangesAsync();
                _logger.LogInformation("[APPEVENT] New Division [ Id={0},Name={1} ] added by {2}", division.Id, division.Name, User.Identity.Name);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "CompanyName", division.CompanyId);
            return View(division);
        }

        // GET: Division/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var division = await _context.Divisions.FindAsync(id);
            if (division == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "CompanyName", division.CompanyId);
            return View(division);
        }

        // POST: Division/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CompanyId")] Division division)
        {
            if (id != division.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(division);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("[APPEVENT] Division [ Id={0},Name={1} ] edited by {2}", division.Id, division.Name, User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DivisionExists(division.Id))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "CompanyName", division.CompanyId);
            return View(division);
        }

        // GET: Division/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var division = await _context.Divisions
                .Include(d => d.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (division == null)
            {
                return NotFound();
            }

            return View(division);
        }

        // POST: Division/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var division = await _context.Divisions.FindAsync(id);
            if (division != null)
            {
                _context.Divisions.Remove(division);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("[APPEVENT] Division [ Id={0},Name={1} ] deleted by {2}", division.Id, division.Name, User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        private bool DivisionExists(int id)
        {
            return _context.Divisions.Any(e => e.Id == id);
        }
    }
}
