using Apprenticeship.Data.Entities;
using Apprenticeship.Data.Repository.AssignmentRepo;
using Apprenticeship.Data.Repository.DocumentRepo;
using Apprenticeship.Data.Repository.ReportRepo;
using Apprenticeship.Data.Repository.ReportsLogRepo;
using Apprenticeship.Data.Repository.ReportStatusRepo;
using Apprenticeship.Data.Repository.SchoolRepo;
using Apprenticeship.Data.Repository.SchoolSupervisorRepo;
using Apprenticeship.Data.Repository.StudentRepo;
using Apprenticeship.Data.Repository.TrainingRepo;
using Apprenticeship.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Apprenticeship.Controllers
{
    public class SchoolSupervisorController : Controller
    {
        ISchoolSupervisorRepository schoolSupervisorRepo;
        ISchoolRepository schoolRepo;
        ITrainingRepository trainingRepo;
        IAssignmentRepository assignmentRepo;
        IReportRepository reportRepo;
        IReportsLogRepository reportsLogRepo;
        IDocumentRepository documentRepo;
        IReportStatusRepository reportStatusRepo;
        IStudentRepository studentRepo;
        public SchoolSupervisorController(IStudentRepository studentRepo, ISchoolSupervisorRepository schoolSupervisorRepo,
            ISchoolRepository schoolRepo, ITrainingRepository trainingRepo, IAssignmentRepository assignmentRepo, IReportStatusRepository reportStatusRepo, IDocumentRepository documentRepo,
            IReportRepository reportRepo, IReportsLogRepository reportsLogRepo)
        {
            this.studentRepo = studentRepo;
            this.schoolSupervisorRepo = schoolSupervisorRepo;
            this.schoolRepo = schoolRepo;
            this.trainingRepo = trainingRepo;
            this.assignmentRepo = assignmentRepo;
            this.reportRepo = reportRepo;
            this.reportsLogRepo = reportsLogRepo;
            this.documentRepo = documentRepo;
            this.reportStatusRepo = reportStatusRepo;
        }
        //Read From Database
        [Authorize(Roles = "ADMIN")]
        public IActionResult Index()
        {
            ViewBag.schoolSupervisors = schoolSupervisorRepo.GetAllSchoolSupervisors();
            return View();
        }
        [Authorize(Roles = "ADMIN")]
        public IActionResult Add()
        {
            ViewBag.schools = schoolRepo.GetAllSchools();
            return View();
        }
        //Create SchoolSupervisor Row in Database
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Insert(SchoolSupervisorDTO schoolSupervisor)
        {
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                var schoolSupervisor_ = new SchoolSupervisor();
                schoolSupervisor_.fristName = schoolSupervisor.fristName;
                schoolSupervisor_.secondName = schoolSupervisor.secondName;
                schoolSupervisor_.thirdName = schoolSupervisor.thirdName;
                schoolSupervisor_.lastName = schoolSupervisor.lastName;
                schoolSupervisor_.PhoneNumber = schoolSupervisor.PhoneNumber;
                schoolSupervisor_.Email = schoolSupervisor.Email;
                schoolSupervisor_.NormalizedEmail = schoolSupervisor.Email.ToUpper();
                schoolSupervisor_.UserName = schoolSupervisor.Email;
                schoolSupervisor_.NormalizedUserName = schoolSupervisor.Email.ToUpper();
                schoolSupervisor_.schoolId = schoolSupervisor.schoolId;
                await schoolSupervisorRepo.AddSchoolSupervisor(schoolSupervisor_, schoolSupervisor.Password);
                return RedirectToAction("Index");
            }
            ViewBag.schools = schoolRepo.GetAllSchools();
            return View("Add",schoolSupervisor);
        }
        [Authorize(Roles = "ADMIN")]
        public IActionResult Edit(string Id)
        {
            ViewBag.schools = schoolRepo.GetAllSchools();
            var schoolSupervisor = schoolSupervisorRepo.GetSchoolSupervisor(Id);
            var schoolSupervisor_ = new SchoolSupervisorDTO();
            schoolSupervisor_.fristName = schoolSupervisor.fristName;
            schoolSupervisor_.secondName = schoolSupervisor.secondName;
            schoolSupervisor_.thirdName = schoolSupervisor.thirdName;
            schoolSupervisor_.lastName = schoolSupervisor.lastName;
            schoolSupervisor_.PhoneNumber = schoolSupervisor.PhoneNumber;
            schoolSupervisor_.Email = schoolSupervisor.Email;
            schoolSupervisor_.schoolId = schoolSupervisor.schoolId;
            schoolSupervisor_.Id = schoolSupervisor.Id;
            return View(schoolSupervisor_);
        }
        //Edit SchoolSupervisor Row in Database
        [Authorize(Roles = "ADMIN")]
        public IActionResult Edited(SchoolSupervisorDTO schoolSupervisor)
        {
            ModelState.Remove("Id");
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                var schoolSupervisor_ = new SchoolSupervisor();
                schoolSupervisor_.Id = schoolSupervisor.Id;
                schoolSupervisor_.fristName = schoolSupervisor.fristName;
                schoolSupervisor_.secondName = schoolSupervisor.secondName;
                schoolSupervisor_.thirdName = schoolSupervisor.thirdName;
                schoolSupervisor_.lastName = schoolSupervisor.lastName;
                schoolSupervisor_.Email = schoolSupervisor.Email;
                schoolSupervisor_.NormalizedEmail = schoolSupervisor.Email.ToUpper();
                schoolSupervisor_.UserName = schoolSupervisor.Email;
                schoolSupervisor_.NormalizedUserName = schoolSupervisor.Email.ToUpper();
                schoolSupervisor_.PhoneNumber = schoolSupervisor.PhoneNumber;
                schoolSupervisor_.schoolId = schoolSupervisor.schoolId;
                schoolSupervisorRepo.EditSchoolSupervisor(schoolSupervisor_);
                return RedirectToAction("Index");
            }
            ViewBag.schools = schoolRepo.GetAllSchools();
            return View("Edit",  schoolSupervisor );
        }
        //Delete SchoolSupervisor Row in Database
        [Authorize(Roles = "ADMIN")]
        public IActionResult Delete(string Id)
        {
            schoolSupervisorRepo.DeleteSchoolSupervisor(Id);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "SCHOOLSUPERVISOR")]
        public IActionResult Trainings()
        {
            var schoolSupervisorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.trainings = trainingRepo.GetAllTrainings().Where(t => t.schoolSupervisorId == schoolSupervisorId).ToList();
            var schoolSupervisor = schoolSupervisorRepo.GetSchoolSupervisor(schoolSupervisorId);
            return View(schoolSupervisor);
        }
        [Authorize(Roles = "SCHOOLSUPERVISOR")]
        public IActionResult Assignments(int trainingId)
        {
            ViewBag.assignments = assignmentRepo.GetAllAssignments().Where(a => a.trainingId == trainingId).ToList();
            ViewBag.documents = documentRepo.GetAllDocuments();
            return View(trainingId);
        }
        [Authorize(Roles = "SCHOOLSUPERVISOR")]
        public IActionResult TimeLine(int assignmentId, int trainingId)
        {
            ViewBag.reportsLog = reportsLogRepo.GetAllReportsLogs().Where(rl => rl.report.assignmentId==assignmentId).OrderByDescending(r => r.logDate).ToList();
            Assignment assignment = assignmentRepo.GetAssignment(assignmentId, trainingId);
            return View(assignment);
        }
        [Authorize(Roles = "SCHOOLSUPERVISOR")]
        public IActionResult Dashboard()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.trainings = trainingRepo.GetAllTrainings().Where(t => t.schoolSupervisorId == Id).ToList();
            var schoolSupervisor = schoolSupervisorRepo.GetSchoolSupervisor(Id);
            ViewBag.students = studentRepo.GetAllStudents().Where(s => s.schoolId == schoolSupervisor.schoolId).ToList();
            ViewBag.schoolSupervisors = schoolSupervisorRepo.GetAllSchoolSupervisors().Where(s => s.schoolId == schoolSupervisor.schoolId).ToList();
            ViewBag.teamLeaders = trainingRepo.GetAllTrainings().Where(t => t.schoolSupervisorId == Id).ToList();
            ViewBag.reports = reportRepo.GetAllReports().Where(r => r.assignment.training.schoolSupervisorId == Id);
            ViewBag.assignments = assignmentRepo.GetAllAssignments().Where(a => a.training.schoolSupervisorId == Id).ToList();
            ViewBag.reportslog = reportsLogRepo.GetAllReportsLogs().Where(rl => rl.report.assignment.training.schoolSupervisorId == Id).ToList();
            ViewBag.ApprovedCount = reportRepo.GetAllReports().Count(r => r.assignment.training.schoolSupervisorId == Id && r.reportStatusId == 1);
            ViewBag.RejectedCount = reportRepo.GetAllReports().Count(r => r.assignment.training.schoolSupervisorId == Id && r.reportStatusId == 2);
            ViewBag.PendingCount = reportRepo.GetAllReports().Count(r => r.assignment.training.schoolSupervisorId == Id && r.reportStatusId == 3);
			var schools = schoolRepo.GetAllSchools();
			var studentsInSchools = schools.Select(school =>
				new
				{
					SchoolName = school.shortName,
					StudentCount = studentRepo.GetAllStudents().Count(student => student.schoolId == school.schoolId)
				}).ToList();
			ViewBag.SchoolsAndStudentCounts = studentsInSchools;
            ViewBag.lastThreeReports = reportRepo.GetAllReports()
                 .Where(r => r.assignment.training.schoolSupervisorId == Id)
                 .OrderByDescending(r => r.reportId)
                 .Take(3)
                 .ToList();
            return View(schoolSupervisor);
        }
        [Authorize(Roles = "SCHOOLSUPERVISOR")]
        public IActionResult Students()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var schoolSupervisor = schoolSupervisorRepo.GetSchoolSupervisor(Id);
            ViewBag.students = studentRepo.GetAllStudents().Where(s => s.school.schoolId == schoolSupervisor.schoolId).ToList();
            return View();
        }
        [Authorize(Roles = "SCHOOLSUPERVISOR")]
        public IActionResult SchoolSupervisors()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var schoolSupervisor = schoolSupervisorRepo.GetSchoolSupervisor(Id);
            ViewBag.schoolSupervisors = schoolSupervisorRepo.GetAllSchoolSupervisors().Where(s => s.schoolId == schoolSupervisor.schoolId).ToList();
            return View();
        }
    }
}