using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ProjectOnBoarding.Data;
using ProjectOnBoarding.Models;
using ProjectOnBoarding.Services.Email;
using ProjectOnBoarding.ViewModels;
using Document = ProjectOnBoarding.Models.Document;
using Project = ProjectOnBoarding.Models.Project;

namespace ProjectOnBoarding.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectDBContext _dbcontext;
        private readonly ILogger<CompanyController> _logger;        
        private readonly IEmailService _emailService;
        private readonly UserManager<ProjectDBUser> _userManager;
        public ProjectController(ProjectDBContext dBContext, ILogger<CompanyController> logger
            , IEmailService emailService, UserManager<ProjectDBUser> userManager)
        {
            _dbcontext = dBContext;            
            _logger = logger;
            _emailService = emailService;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Add()
        {
            var companies = _dbcontext.Companies.ToList();
            var divisions = new List<Division>();
            var brands = new List<Brand>();

            ViewBag.Companies = new SelectList(companies, "Id", "CompanyName");
            ViewBag.Divisions = new SelectList(divisions, "Id", "Name");
            ViewBag.Brands = new SelectList(brands, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProjectViewModel viewmodel)
        {
            var project = new Project()
            {
                Name = viewmodel.Name,
                Description = viewmodel.Description,
                CompanyId = viewmodel.CompanyId,
                DivisionId = viewmodel.DivisionId,
                BrandId = viewmodel.BrandId,
                Brief = viewmodel.Brief,
                References = viewmodel.References,
                IsCompleted = viewmodel.IsCompleted,                
                Company = GetCompanyName(viewmodel.CompanyId),
                Division = GetDivisionName(viewmodel.DivisionId),
                Brand = GetBrandName(viewmodel.BrandId),

                Code = "", // Updating the code after insert
                CreatedBy = User.Identity is not null ? User.Identity.Name : "Not Available",
                CreateDate = DateTime.Now,
                
            };
            await _dbcontext.Projects.AddAsync(project);
            await _dbcontext.SaveChangesAsync();
            int id = (int)project.Id;
            UpdateProjectCode(id);
            _logger.LogInformation("[APPEVENT] New project [ Code={0},Name={1} ] added by {2}", project.Code, project.Name, User.Identity.Name);
            return RedirectToAction("Index", "Project");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var projects = await _dbcontext.Projects.Where(p => p.IsCompleted == false).OrderByDescending(p => p.CreateDate).ToListAsync();
            return View(projects);
        }

        [HttpGet]
        public async Task<IActionResult> Completed()
        {
            var projects = await _dbcontext.Projects.Where(p => p.IsCompleted == true).OrderByDescending(p => p.CreateDate).ToListAsync();
            return View(projects);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var companies = _dbcontext.Companies.ToList();
            var divisions = _dbcontext.Divisions.ToList();
            var brands = _dbcontext.Brands.ToList();

            ViewBag.Companies = new SelectList(companies, "Id", "CompanyName");
            ViewBag.Divisions = new SelectList(divisions, "Id", "Name");
            ViewBag.Brands = new SelectList(brands, "Id", "Name");

            var project = await _dbcontext.Projects.FindAsync(id);

            ProjectViewModel projectvm = new ProjectViewModel();

            if (project != null)
            {
                projectvm.Id = project.Id;
                projectvm.Name = project.Name;
                projectvm.Description = project.Description;
                projectvm.CompanyId = project.CompanyId;
                projectvm.Company = project.Company;
                projectvm.DivisionId = project.DivisionId;
                projectvm.Division = project.Division;
                projectvm.BrandId = project.BrandId;
                projectvm.Brand = project.Brand;
                projectvm.Brief = project.Brief;
                projectvm.References = project.References;                
                projectvm.Code = project.Code;
                projectvm.IsCompleted = project.IsCompleted;
                projectvm.CreateDate = project.CreateDate;
                projectvm.CreatedBy = project.CreatedBy;
            }

            return View(projectvm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var companies = _dbcontext.Companies.ToList();
            var divisions = _dbcontext.Divisions.ToList();
            var brands = _dbcontext.Brands.ToList();

            ViewBag.Companies = new SelectList(companies, "Id", "CompanyName");
            ViewBag.Divisions = new SelectList(divisions, "Id", "Name");
            ViewBag.Brands = new SelectList(brands, "Id", "Name");

            var project = await _dbcontext.Projects.FindAsync(id);

            ProjectViewModel projectvm = new ProjectViewModel();
            if (project != null)
            {
                projectvm.Id = project.Id;
                projectvm.Name = project.Name;
                projectvm.Description = project.Description;
                projectvm.CompanyId = project.CompanyId;
                projectvm.Company = project.Company;
                projectvm.DivisionId = project.DivisionId;
                projectvm.Division = project.Division;
                projectvm.BrandId = project.BrandId;
                projectvm.Brand = project.Brand;
                projectvm.Brief = project.Brief;
                projectvm.References = project.References;
                projectvm.IsCompleted = project.IsCompleted;
                
                projectvm.Code = project.Code;
                projectvm.CreateDate = project.CreateDate;
                projectvm.CreatedBy = project.CreatedBy;
            }

            return View(projectvm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProjectViewModel viewmodel)
        {
            var project = await _dbcontext.Projects.FindAsync(viewmodel.Id);           

            if (project is not null) 
            { 
                project.Name = viewmodel.Name;
                project.Description = viewmodel.Description;
                project.CompanyId = viewmodel.CompanyId;
                project.DivisionId = viewmodel.DivisionId;
                project.BrandId = viewmodel.BrandId;
                project.Brief = viewmodel.Brief;
                project.References = viewmodel.References;
                project.Code = viewmodel.Code;
                project.IsCompleted = viewmodel.IsCompleted;
                project.CreateDate = viewmodel.CreateDate;
                project.CreatedBy   = viewmodel.CreatedBy;
                
                project.Company = GetCompanyName(viewmodel.CompanyId);
                project.Division = GetDivisionName(viewmodel.DivisionId);
                project.Brand = GetBrandName(viewmodel.BrandId);

                await _dbcontext.SaveChangesAsync();
                _logger.LogInformation("[APPEVENT] Project [ Code={0},Name={1} ] added by {2}", project.Code, project.Name, User.Identity.Name);
            }

            return RedirectToAction("Index","Project");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _dbcontext.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _dbcontext.Projects.FindAsync(id);
            if (project != null)
            {
                _dbcontext.Projects.Remove(project);
                await _dbcontext.SaveChangesAsync();
                _logger.LogInformation("[APPEVENT] Project [ Code={0},Name={1} ] deleted by {2}", project.Code, project.Name, User.Identity.Name);
            }

            
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Email()
        {
            //var users = _userManager.Users.ToList();
            ViewData["Projects"] = new SelectList(_dbcontext.Projects, "Id", "Name");
            ViewData["Users"] = new SelectList(_userManager.Users, "Email","FirstName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Email(EmailViewModel viewModel)
        {
            int projectId = viewModel.ProjectId;
            string emailTo = viewModel.EmailId;

            string emailBody = await FormatEmailBody(projectId);

            List<Document> documents = await _dbcontext.Documents.Where(d => d.ProjectId == projectId).ToListAsync();

            _emailService.SendEmail(new EmailDto()
            {
                To = emailTo, 
                Subject = "Project details",
                Body = emailBody,
                Attachments = documents,
            });

            ViewData["Projects"] = new SelectList(_dbcontext.Projects, "Id", "Name");
            ViewData["Users"] = new SelectList(_userManager.Users, "Email", "FirstName");

            return View();
        }
        public async Task<IActionResult> DocumentListPartial(int projectId)
        {               
            return PartialView("_FileListPartial", await _dbcontext.Documents.Where(p => p.ProjectId == projectId).ToListAsync());
        }
        public JsonResult GetDivisionByCompanyId(int compamyId) {
            return Json(_dbcontext.Divisions.Where(d => d.CompanyId == compamyId).ToList());
        }

        public JsonResult GetBrandByDivisionId(int divisionId)
        {
            JsonResult result = Json(_dbcontext.Brands.Where(d => d.DivisionId == divisionId).ToList());
            return result;
        }

        private async Task<string> FormatEmailBody(int projectId)
        {
            string emailBody = string.Empty;

            Project project = await _dbcontext.Projects.FindAsync(projectId);

            emailBody = "<html><body>";

            if (project != null)
            {
                emailBody = "<table>";
                emailBody = "<html><body><table>";
                emailBody += "<tr><td><b>Code</b></td><td>" + project.Code + "</td></tr>";
                emailBody += "<tr><td><b>Name</b></td><td>" + project.Name + "</td></tr>";
                emailBody += "<tr><td><b>Description</b></td><td>" + project.Description + "</td></tr>";

                emailBody += "<tr><td><b>Company</b></td><td>" + GetCompanyName(project.CompanyId) + "</td></tr>";
                emailBody += "<tr><td><b>Division</b></td><td>" + GetDivisionName(project.DivisionId) + "</td></tr>";
                emailBody += "<tr><td><b>Brand</b></td><td>" + GetBrandName(project.BrandId) + "</td></tr>";

                emailBody += "<tr><td><b>Brief</b></td><td>" + project.Brief + "</td></tr>";
                emailBody += "<tr><td><b>Reference</b></td><td>" + project.References + "</td></tr>";
                emailBody += "</table>";
            }

            List<ProjectTask> tasks = await _dbcontext.ProjectTasks.Where(t => t.ProjectId == projectId).ToListAsync();
            int taskCount = 0;
            if (tasks is not null)
            {
                if (tasks.Count > 0)
                {
                    emailBody += "<h4>Below are the project tasks</h4>";

                    foreach (var task in tasks)
                    {
                        taskCount++;
                        emailBody += "<table>";
                        emailBody += "<tr><td><b>Task #</b></td><td><b>" + taskCount + "</b></td></tr>";
                        emailBody += "<tr><td>Name</td><td>" + task.Name + "</td></tr>";
                        emailBody += "<tr><td>Task Description</td><td>" + task.Description + "</td></tr>";
                        emailBody += "<tr><td>Category</td><td>" + task.Category + "</td></tr>";
                        emailBody += "<tr><td>Size</td><td>" + task.Size + "</td></tr>";
                        emailBody += "<table> <p>";
                    }
                }
                else
                {
                    emailBody += "<h4>No tasks added for the project</h4>";
                }
            }

            emailBody += "</body></html>";
            return emailBody;
        }
        private string GetCompanyName(int? companyId) {
            return _dbcontext.Companies.Where(c => c.Id == companyId).Select(c => c.CompanyName).SingleOrDefault().ToString();
        }
        
        private string GetDivisionName(int? divisionId) {
            return _dbcontext.Divisions.Where(d => d.Id == divisionId).Select(d => d.Name).SingleOrDefault().ToString();
        }

        private string GetBrandName(int? brandId)
        {
            return _dbcontext.Brands.Where(d => d.Id == brandId).Select(d => d.Name).SingleOrDefault().ToString();
        }

        private string GenerateProjectCode(int projectId)
        {
            return "AUG" + "-" + DateTime.Now.Year.ToString() + "-" + projectId.ToString();
        }

        private void UpdateProjectCode(int projectId)
        {
            var project =  _dbcontext.Projects.Find(projectId);
            if (project is not null)
            {
                project.Code = GenerateProjectCode(project.Id);
                _dbcontext.SaveChanges();
            }
        }
    }
}
