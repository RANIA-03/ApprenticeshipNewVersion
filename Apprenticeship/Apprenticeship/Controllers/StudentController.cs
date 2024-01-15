using Apprenticeship.Data.Entities;
using Apprenticeship.Data.Repository.AssignmentRepo;
using Apprenticeship.Data.Repository.DocumentRepo;
using Apprenticeship.Data.Repository.ReportRepo;
using Apprenticeship.Data.Repository.ReportsLogRepo;
using Apprenticeship.Data.Repository.SchoolRepo;
using Apprenticeship.Data.Repository.SchoolSupervisorRepo;
using Apprenticeship.Data.Repository.StudentRepo;
using Apprenticeship.Data.Repository.TeamLeaderRepo;
using Apprenticeship.Data.Repository.TrainingRepo;
using Apprenticeship.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Apprenticeship.Controllers
{
    public class StudentController : Controller
    {
		IDocumentRepository documentRepo;
		IStudentRepository studentRepo;
		ISchoolRepository schoolRepo;
		IAssignmentRepository assignmentsRepo;
		ITrainingRepository trainingRepo;
		IReportRepository reportRepo;
        ISchoolSupervisorRepository schoolSupervisorRepo;
        ITeamLeaderRepository teamLeaderRepo;
        IAssignmentRepository assignmentRepo;
        IReportsLogRepository reportsLogRepo;
        public StudentController(ISchoolSupervisorRepository schoolSupervisorRepo,
        ITeamLeaderRepository teamLeaderRepo,
        IAssignmentRepository assignmentRepo,
        IReportsLogRepository reportsLogRepo,IStudentRepository studentRepo, 
        IDocumentRepository documentRepo, ISchoolRepository schoolRepo,
        ITrainingRepository trainingRepo, IAssignmentRepository assignmentsRepo,
        IReportRepository reportRepo)
        {
            this.documentRepo = documentRepo;
            this.studentRepo = studentRepo;
            this.schoolRepo = schoolRepo;
			this.trainingRepo = trainingRepo;
            this.assignmentsRepo = assignmentsRepo;
			this.reportRepo = reportRepo;
            this.assignmentRepo = assignmentRepo;
            this.schoolSupervisorRepo = schoolSupervisorRepo;
            this.teamLeaderRepo = teamLeaderRepo;
            this.reportsLogRepo = reportsLogRepo;
        }
        //Read From Database
        [Authorize(Roles = "ADMIN")]
        public IActionResult Index()
        {
            ViewBag.Students = studentRepo.GetAllStudents();
            return View();
        }
        [Authorize(Roles = "ADMIN")]
        public IActionResult Add() {
            ViewBag.schools = schoolRepo.GetAllSchools();
            return View();
        }
        //Create Student Row in Database

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Insert(StudentDTO student)
		{
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                var student_ = new Student();
                student_.fristName = student.fristName;
                student_.secondName = student.secondName;
                student_.thirdName = student.thirdName;
                student_.lastName = student.lastName;
                student_.schoolId = student.schoolId;
                student_.PhoneNumber = student.PhoneNumber;
                student_.Email = student.Email;
                student_.NormalizedEmail = student.Email.ToUpper();
                student_.UserName = student.Email;
                student_.NormalizedUserName = student.Email.ToUpper();
                student_.studentMajor = student.studentMajor;
                student_.schoolId = student.schoolId;
                await studentRepo.AddStudent(student_, student.Password);
                return RedirectToAction("Index");
            }
            ViewBag.schools = schoolRepo.GetAllSchools();
            return View("Add", student);
        }
        [Authorize(Roles = "ADMIN")]
        public IActionResult Edit(string Id)
		{
            ViewBag.schools = schoolRepo.GetAllSchools();
            var student = studentRepo.GetStudent(Id);
            var student_ = new StudentDTO();
            student_.Id = student.Id;
            student_.fristName = student.fristName;
            student_.secondName = student.secondName;
            student_.thirdName = student.thirdName;
            student_.lastName = student.lastName;
            student_.schoolId = student.schoolId;
            student_.PhoneNumber = student.PhoneNumber;
            student_.Email = student.Email;
            student_.studentMajor = student.studentMajor;
            student_.schoolId = student.schoolId;
            return View(student_);
		}
        //Edit Student Row in Database
        [Authorize(Roles = "ADMIN")]
        public IActionResult Edited(StudentDTO student)
		{
            ModelState.Remove("Id");
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                var student_ = new Student();
                student_.Id = student.Id;
                student_.fristName = student.fristName;
                student_.secondName = student.secondName;
                student_.thirdName = student.thirdName;
                student_.lastName = student.lastName;
                student_.schoolId = student.schoolId;
                student_.Email = student.Email;
                student_.NormalizedEmail = student.Email.ToUpper();
                student_.UserName = student.Email;
                student_.NormalizedUserName = student.Email.ToUpper();
                student_.PhoneNumber = student.PhoneNumber;
                student_.studentMajor = student.studentMajor;
                student_.schoolId = student.schoolId;
                studentRepo.EditStudent(student_);
                return RedirectToAction("Index");
            }
            ViewBag.schools = schoolRepo.GetAllSchools();
            return View("Edit", student);
        }
        //Delete Student Row in Database
        [Authorize(Roles = "ADMIN")]

        public IActionResult Delete(string Id)
		{
			studentRepo.DeleteStudent(Id);
			return RedirectToAction("Index");
		}
        [Authorize(Roles = "STUDENT")]
        public IActionResult Trainings()
		{
			var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			ViewBag.trainings = trainingRepo.GetAllTrainings().Where(t => t.studentId == studentId).ToList();
			var student = studentRepo.GetStudent(studentId);
			return View(student);
		}
        [Authorize(Roles = "STUDENT")]
        public IActionResult Assignments(int trainingId)
		{
			ViewBag.assignments = assignmentsRepo.GetAllAssignments().Where(t => t.trainingId == trainingId).ToList();
			ViewBag.documents = documentRepo.GetAllDocuments();
			return View();
		}
        [Authorize(Roles = "STUDENT")]
        public IActionResult Reports(int assignmentId,int trainingId)
        {
            var reports = reportRepo.GetAllReports().Where(r => r.assignmentId == assignmentId).ToList();
			ViewBag.reports = reports;
            var assignment = assignmentRepo.GetAssignment(assignmentId, trainingId);
            ViewBag.cadAddReport = DateTime.Now < assignment.endDate;
			ViewBag.documents = documentRepo.GetAllDocuments();
            ViewBag.trainingId=trainingId;
			return View(assignmentId);
		}
        [Authorize(Roles = "STUDENT")]
        public IActionResult Dashboard()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.trainings = trainingRepo.GetAllTrainings().Where(s => s.studentId == Id).ToList();
            var student = studentRepo.GetStudent(Id);
            ViewBag.students = studentRepo.GetAllStudents().Where(s => s.schoolId == student.schoolId).ToList();
            ViewBag.schoolSupervisors = schoolSupervisorRepo.GetAllSchoolSupervisors().Where(s => s.schoolId == student.schoolId).ToList();
            ViewBag.teamLeaders = teamLeaderRepo.GetAllTeamLeaders();
            ViewBag.reports = reportRepo.GetAllReports().Where(r => r.assignment.training.studentId == Id).ToList();
            ViewBag.assignments = assignmentRepo.GetAllAssignments().Where(a => a.training.studentId == Id).ToList();
            ViewBag.reportslog = reportsLogRepo.GetAllReportsLogs().Where(rl => rl.report.assignment.training.studentId == Id).ToList();
            ViewBag.ApprovedCount = reportRepo.GetAllReports().Count(r => r.assignment.training.studentId == Id && r.reportStatusId == 1);
            ViewBag.RejectedCount = reportRepo.GetAllReports().Count(r => r.assignment.training.studentId == Id && r.reportStatusId == 2);
            ViewBag.PendingCount = reportRepo.GetAllReports().Count(r => r.assignment.training.studentId == Id && r.reportStatusId == 3);
            ViewBag.lastThreeReports = reportRepo.GetAllReports()
                .Where(r => r.assignment.training.studentId == Id)
                .OrderByDescending(r => r.reportId)
                .Take(3)
                .ToList();
            var schools = schoolRepo.GetAllSchools();
            var studentsInSchools = schools.Select(school =>
                new
                {
                    SchoolName = school.shortName,
                    StudentCount = studentRepo.GetAllStudents().Count(student => student.schoolId == school.schoolId)
                }).ToList();
            ViewBag.SchoolsAndStudentCounts = studentsInSchools;

            return View(student);
        }

        [Authorize(Roles = "STUDENT")]
        public IActionResult AllAssignments()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.assignments = assignmentRepo.GetAllAssignments().Where(r => r.training.studentId == Id).ToList();
			ViewBag.documents = documentRepo.GetAllDocuments();
			return View();
        }
        [Authorize(Roles = "STUDENT")]
        public IActionResult AllReports()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.reports = reportRepo.GetAllReports().Where(r => r.assignment.training.studentId == Id).ToList();
			ViewBag.documents = documentRepo.GetAllDocuments();
			return View();
        }
    }
}
