using Apprenticeship.Data.Entities;
using Apprenticeship.Data.Repository.AssignmentRepo;
using Apprenticeship.Data.Repository.CompanyRepo;
using Apprenticeship.Data.Repository.DocumentRepo;
using Apprenticeship.Data.Repository.ReportRepo;
using Apprenticeship.Data.Repository.ReportsLogRepo;
using Apprenticeship.Data.Repository.ReportStatusRepo;
using Apprenticeship.Data.Repository.SchoolRepo;
using Apprenticeship.Data.Repository.StudentRepo;
using Apprenticeship.Data.Repository.TeamLeaderRepo;
using Apprenticeship.Data.Repository.TrainingRepo;
using Apprenticeship.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Apprenticeship.Controllers
{
    public class TeamLeaderController : Controller
    {
        ITeamLeaderRepository teamLeaderRepo;
		ICompanyRepository companyRepo;
		IAssignmentRepository assignmentsRepo;
		IReportRepository reportRepo;
		ITrainingRepository trainingRepo;
		IReportStatusRepository reportStatusRepo;
		IDocumentRepository documentRepo;
        IAssignmentRepository assignmentRepo;
        IReportsLogRepository reportsLogRepo;
        ISchoolRepository schoolRepo;
        IStudentRepository studentRepo;
        public TeamLeaderController(IAssignmentRepository assignmentRepo, ISchoolRepository schoolRepo, IStudentRepository studentRepo,

		IReportsLogRepository reportsLogRepo,ITeamLeaderRepository teamLeaderRepo, IDocumentRepository documentRepo, ICompanyRepository companyRepo,IReportStatusRepository reportStatusRepo ,IAssignmentRepository assignmentsRepo, IReportRepository reportRepo, ITrainingRepository trainingRepo)
		{
            this.assignmentRepo = assignmentRepo;
            this.reportsLogRepo = reportsLogRepo;
			this.teamLeaderRepo = teamLeaderRepo;
			this.companyRepo = companyRepo;
			this.assignmentsRepo = assignmentsRepo;
			this.trainingRepo = trainingRepo;
			this.documentRepo = documentRepo;
			this.reportRepo = reportRepo;
			this.reportStatusRepo = reportStatusRepo;
            this.schoolRepo = schoolRepo;
            this.studentRepo = studentRepo;
		}
        //Read From Database
        [Authorize(Roles = "ADMIN")]
        public IActionResult Index()
        {
            ViewBag.teamLeaders = teamLeaderRepo.GetAllTeamLeaders();
            return View();
        }
        [Authorize(Roles = "ADMIN")]
        public IActionResult Add() {
            ViewBag.companies = companyRepo.GetAllCompanies();
            return View();
        }
        //Create TeamLeader Row in Database
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Insert(TeamLeaderDTO teamLeader)
		{
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                var teamLeader_ = new TeamLeader();
                teamLeader_.fristName = teamLeader.fristName;
                teamLeader_.secondName = teamLeader.secondName;
                teamLeader_.thirdName = teamLeader.thirdName;
                teamLeader_.lastName = teamLeader.lastName;
                teamLeader_.PhoneNumber = teamLeader.PhoneNumber;
                teamLeader_.Email = teamLeader.Email;
                teamLeader_.NormalizedEmail = teamLeader.Email.ToUpper();
                teamLeader_.UserName = teamLeader.Email;
                teamLeader_.NormalizedUserName = teamLeader.Email.ToUpper();
                teamLeader_.companyId = teamLeader.companyId;
                await teamLeaderRepo.AddTeamLeader(teamLeader_, teamLeader.Password);
                return RedirectToAction("Index");
            }
            ViewBag.companies = companyRepo.GetAllCompanies();
            return View("Add", teamLeader);
		}
        [Authorize(Roles = "ADMIN")]

        public IActionResult Edit(string Id)
		{
            ViewBag.companies = companyRepo.GetAllCompanies();
            var teamLeader = teamLeaderRepo.GetTeamLeader(Id);
            var teamLeader_ = new TeamLeaderDTO();
            teamLeader_.Id = teamLeader.Id;
            teamLeader_.fristName = teamLeader.fristName;
            teamLeader_.secondName = teamLeader.secondName;
            teamLeader_.thirdName = teamLeader.thirdName;
            teamLeader_.lastName = teamLeader.lastName;
            teamLeader_.Email = teamLeader.Email;
            teamLeader_.PhoneNumber = teamLeader.PhoneNumber;
            teamLeader_.companyId = teamLeader.companyId;
            return View(teamLeader_);
		}
        //Edit TeamLeader Row in Database
        [Authorize(Roles = "ADMIN")]

        public IActionResult Edited(TeamLeaderDTO teamLeader)
		{
            ModelState.Remove("Id");
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                var teamLeader_ = new TeamLeader();
                teamLeader_.Id = teamLeader.Id;
                teamLeader_.fristName = teamLeader.fristName;
                teamLeader_.secondName = teamLeader.secondName;
                teamLeader_.thirdName = teamLeader.thirdName;
                teamLeader_.lastName = teamLeader.lastName;
                teamLeader_.Email = teamLeader.Email;
                teamLeader_.NormalizedEmail = teamLeader.Email.ToUpper();
                teamLeader_.UserName = teamLeader.Email;
                teamLeader_.NormalizedUserName = teamLeader.Email.ToUpper();
                teamLeader_.PhoneNumber = teamLeader.PhoneNumber;
                teamLeader_.companyId = teamLeader.companyId;
                teamLeaderRepo.EditTeamLeader(teamLeader_);
                return RedirectToAction("Index");
            }
            ViewBag.companies = companyRepo.GetAllCompanies();
            return View("Edit", teamLeader);
        }
        //Delete TeamLeader Row in Database
        [Authorize(Roles = "ADMIN")]

        public IActionResult Delete(string Id)
		{
			teamLeaderRepo.DeleteTeamLeader(Id);
			return RedirectToAction("Index");
		}
        [Authorize(Roles = "TEAMLEADER")]

        public IActionResult Trainings()
		{
			var teamLeaderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			ViewBag.trainings=trainingRepo.GetAllTrainings().Where(t => t.teamLeaderId == teamLeaderId).ToList();
			var teamLeader = teamLeaderRepo.GetTeamLeader(teamLeaderId);
			return View(teamLeader);
		}
        [Authorize(Roles = "TEAMLEADER")]
        public IActionResult Assignments(int trainingId)
		{
			ViewBag.assignments = assignmentsRepo.GetAllAssignments().Where(t => t.trainingId == trainingId).ToList();
			ViewBag.documents = documentRepo.GetAllDocuments();

			return View(trainingId);
		}
        [Authorize(Roles = "TEAMLEADER")]
        public IActionResult Reports(int assignmentId)
		{
			ViewBag.reports = reportRepo.GetAllReports().Where(r => r.assignmentId == assignmentId).ToList();
			ViewBag.reportStatuses = reportStatusRepo.GetAllReportStatuses();
            ViewBag.documents = documentRepo.GetAllDocuments();
            return View(assignmentId);
		}
        [Authorize(Roles = "TEAMLEADER")]
        public IActionResult Dashboard()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.trainings = trainingRepo.GetAllTrainings().Where(t => t.teamLeaderId == Id).ToList();
            ViewBag.students = trainingRepo.GetAllTrainings().Where(t => t.teamLeaderId == Id).ToList();
            ViewBag.schoolSupervisors = trainingRepo.GetAllTrainings().Where(t => t.teamLeaderId == Id).ToList();
            var teamLeader = teamLeaderRepo.GetTeamLeader(Id);
            ViewBag.teamLeaders = teamLeaderRepo.GetAllTeamLeaders().Where(t => t.companyId == teamLeader.companyId).ToList();
            ViewBag.reports = reportRepo.GetAllReports().Where(r => r.assignment.training.teamLeaderId == Id).ToList();
            ViewBag.assignments = assignmentRepo.GetAllAssignments().Where(a => a.training.teamLeaderId == Id).ToList();
            ViewBag.reportslog = reportsLogRepo.GetAllReportsLogs().Where(rl => rl.report.assignment.training.teamLeaderId == Id).ToList();
            ViewBag.ApprovedCount = reportRepo.GetAllReports().Count(r => r.assignment.training.teamLeaderId == Id && r.reportStatusId == 1);
            ViewBag.RejectedCount = reportRepo.GetAllReports().Count(r => r.assignment.training.teamLeaderId == Id && r.reportStatusId == 2);
            ViewBag.PendingCount = reportRepo.GetAllReports().Count(r => r.assignment.training.teamLeaderId == Id && r.reportStatusId == 3);
			var schools = schoolRepo.GetAllSchools();
			var studentsInSchools = schools.Select(school =>
				new
				{
					SchoolName = school.shortName,
					StudentCount = studentRepo.GetAllStudents().Count(student => student.schoolId == school.schoolId)
				}).ToList();
			ViewBag.SchoolsAndStudentCounts = studentsInSchools;
            ViewBag.lastThreeReports = reportRepo.GetAllReports()
                .Where(r => r.assignment.training.teamLeaderId == Id)
                .OrderByDescending(r => r.reportId)
                .Take(3)
                .ToList();
            return View(teamLeader);
        }
        [Authorize(Roles = "TEAMLEADER")]
        public IActionResult AllAssignments()
        {
			var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			ViewBag.assignments = assignmentRepo.GetAllAssignments().Where(r => r.training.teamLeaderId == Id).ToList();
			ViewBag.documents = documentRepo.GetAllDocuments();
			return View();
		}
        [Authorize(Roles = "TEAMLEADER")]
        public IActionResult AllReports()
        {
			var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			ViewBag.reports = reportRepo.GetAllReports().Where(r => r.assignment.training.teamLeaderId == Id).ToList();
			ViewBag.documents = documentRepo.GetAllDocuments();
			return View();
        }
    }
}
