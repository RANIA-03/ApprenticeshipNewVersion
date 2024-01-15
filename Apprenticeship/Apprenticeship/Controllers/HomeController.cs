using Apprenticeship.Data.Repository.AssignmentRepo;
using Apprenticeship.Data.Repository.CompanyRepo;
using Apprenticeship.Data.Repository.ReportRepo;
using Apprenticeship.Data.Repository.ReportsLogRepo;
using Apprenticeship.Data.Repository.SchoolSupervisorRepo;
using Apprenticeship.Data.Repository.StudentRepo;
using Apprenticeship.Data.Repository.TeamLeaderRepo;
using Apprenticeship.Data.Repository.TrainingRepo;
using Apprenticeship.Data;
using Apprenticeship.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Apprenticeship.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext context;
        ITrainingRepository trainingRepo;
        IStudentRepository studentRepo;
        ISchoolSupervisorRepository schoolSupervisorRepo;
        ITeamLeaderRepository teamLeaderRepo;
        IReportRepository reportRepo;
        IAssignmentRepository assignmentRepo;
        IReportsLogRepository reportsLogRepo;
        ICompanyRepository companyRepo;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context,
            ITrainingRepository trainingRepo,
            IStudentRepository studentRepo,
            ISchoolSupervisorRepository schoolSupervisorRepo,
            ITeamLeaderRepository teamLeaderRepo,
            IReportRepository reportRepo,
            IAssignmentRepository assignmentRepo,
            IReportsLogRepository reportsLogRepo,
            ICompanyRepository companyRepo)
        {
            _logger = logger;
            this.context = context;
            this.trainingRepo = trainingRepo;
            this.studentRepo = studentRepo;
            this.schoolSupervisorRepo = schoolSupervisorRepo;
            this.teamLeaderRepo = teamLeaderRepo;
            this.reportRepo = reportRepo;
            this.assignmentRepo = assignmentRepo;
            this.reportsLogRepo = reportsLogRepo;
            this.companyRepo = companyRepo;
        }
        public IActionResult Index()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Id != null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }
        [Authorize(Roles = "ADMIN, TEAMLEADER, SCHOOLSUPERVISOR, STUDENT")]
        public IActionResult Dashboard()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = context.Users.Where(user => user.Id == Id).SingleOrDefault();
            var role = context.Entry(user).Property("Discriminator").CurrentValue;
            switch (role)
            {
                case "Person":
                    return RedirectToAction("Dashboard", "Admin");
                case "Student":
                    return RedirectToAction("Dashboard", "Student");
                case "TeamLeader":
                    return RedirectToAction("Dashboard", "TeamLeader");
                case "SchoolSupervisor":
                    return RedirectToAction("Dashboard", "SchoolSupervisor");
            }
            return View();
        }
        public IActionResult Welcome()
        {
            return View();
        }
        [Authorize(Roles = "ADMIN, TEAMLEADER, SCHOOLSUPERVISOR, STUDENT")]
        public IActionResult Calendar()
        {
            return View();
        }
        [Authorize(Roles = "ADMIN, TEAMLEADER, SCHOOLSUPERVISOR, STUDENT")]
        public IActionResult Profile()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = context.Users.Where(user => user.Id == Id).SingleOrDefault();
            var role = context.Entry(user).Property("Discriminator").CurrentValue;
            ViewBag.role = role;
            return View(user);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
