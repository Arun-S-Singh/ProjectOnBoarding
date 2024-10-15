
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectOnBoarding.Data;
using ProjectOnBoarding.Models;
using ProjectOnBoarding.ViewModels;


namespace ProjectOnBoarding.Controllers
{
    public class DocumentController : Controller
    {
        private readonly ProjectDBContext _context;
        private readonly ILogger<CompanyController> _logger;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public DocumentController(ProjectDBContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment
            , ILogger<CompanyController> logger)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        // GET: Document
        public async Task<IActionResult> Index()
        {
            var projectDBContext = _context.Documents.Include(d => d.Project);
            return View(await projectDBContext.ToListAsync());
        }

        // GET: Document/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Document/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
            return View();
        }

        // POST: Document/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentViewModel viewModel)
        {
            string filename = string.Empty;
            string uniqueFilename = string.Empty;

            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "documents");

            if (viewModel.File != null)
            {
                filename = viewModel.File.FileName;
                uniqueFilename = Guid.NewGuid().ToString() + "_" + filename;
                string filePath = Path.Combine(uploadsFolder, uniqueFilename);

                try
                {
                    viewModel.File.CopyTo(new FileStream(filePath, FileMode.Create));

                    var document = new Document()
                    {
                        Type = viewModel.Type,
                        FileName = filename,
                        UniqueFileName = uniqueFilename,
                        ProjectId = viewModel.ProjectId,
                    };
                    await _context.Documents.AddAsync(document);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("[APPEVENT] New document [ Id={0},FileName={1} ] added by {2} for project [{3}]",
                        viewModel.Id, uniqueFilename, User.Identity.Name, viewModel.ProjectId);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Unable to upload document for project [{0}]", viewModel.ProjectId);
                    _logger.LogError(ex.Message);
                }


            }
            return RedirectToAction("Index", "Document");
        }

        // GET: Document/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            DocumentViewModel viewModel = new DocumentViewModel()
            {
                Id = document.Id,
                Type = document.Type,
                FileName = document.FileName,
                UniqueFileName = document.UniqueFileName,
                ProjectId = document.ProjectId,
            };
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", document.Project);
            return View(viewModel);
        }

        // POST: Document/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DocumentViewModel viewModel)
        {
            string filename = string.Empty;
            string uniqueFilename = string.Empty;

            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "documents");

            var document = await _context.Documents.FindAsync(id);

            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (viewModel.File != null)
            {
                filename = viewModel.File.FileName;
                uniqueFilename = Guid.NewGuid().ToString() + "_" + filename;

                string filePath = Path.Combine(uploadsFolder, uniqueFilename);
                string deleteFilePath = Path.Combine(uploadsFolder, document.UniqueFileName);

                document.FileName = filename;
                document.UniqueFileName = uniqueFilename;

                if (System.IO.File.Exists(deleteFilePath))
                {
                    System.IO.File.Delete(deleteFilePath);
                }

                viewModel.File.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            document.Type = viewModel.Type;
            document.ProjectId = viewModel.ProjectId;

            try
            {               
                _context.Update(document);
                await _context.SaveChangesAsync();

                _logger.LogInformation("[APPEVENT] Document [Id={0}, File={1}] edited by {2}",
                    viewModel.Id, uniqueFilename, User.Identity.Name);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentExists(viewModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", viewModel.ProjectId);
            return View(viewModel);
        }

        // GET: Document/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Document/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "documents");

            var document = await _context.Documents.FindAsync(id);
            string filePath = Path.Combine(uploadsFolder, document.UniqueFileName);
            if (document != null)
            {
                try
                {
                    _context.Documents.Remove(document);
                    await _context.SaveChangesAsync();

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    _logger.LogInformation("[APPEVENT] Document [ Id={0},FileName={1} ] deleted by {2} for project [{3}]",
                           document.Id, document.UniqueFileName, User.Identity.Name, document.ProjectId);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Unable to delete file {0} for project {1}", document.UniqueFileName, document.Project);
                    _logger.LogError(ex.Message);
                }

            }
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.Id == id);
        }
    }
}
