using Apprenticeship.Data;
using Apprenticeship.Data.Entities;
using Apprenticeship.Data.Repository.AssignmentRepo;
using Apprenticeship.Data.Repository.CompanyRepo;
using Apprenticeship.Data.Repository.ContactMessageRepo;
using Apprenticeship.Data.Repository.ReportRepo;
using Apprenticeship.Data.Repository.ReportsLogRepo;
using Apprenticeship.Data.Repository.SchoolRepo;
using Apprenticeship.Data.Repository.SchoolSupervisorRepo;
using Apprenticeship.Data.Repository.StudentRepo;
using Apprenticeship.Data.Repository.TeamLeaderRepo;
using Apprenticeship.Data.Repository.TrainingRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Security.Claims;

namespace Apprenticeship.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class AdminController : Controller
    {
        private ApplicationDbContext context;
        ITrainingRepository trainingRepo;
        IStudentRepository studentRepo;
        ISchoolSupervisorRepository schoolSupervisorRepo;
        ITeamLeaderRepository teamLeaderRepo;
        IReportRepository reportRepo;
        IAssignmentRepository assignmentRepo;
        IReportsLogRepository reportsLogRepo;
        ICompanyRepository companyRepo;
        ISchoolRepository schoolRepo;
        IContactMessageRepository contactMessageRepo;

        public AdminController(ApplicationDbContext context, ITrainingRepository trainingRepo,
            IStudentRepository studentRepo,
            ISchoolSupervisorRepository schoolSupervisorRepo,
            ITeamLeaderRepository teamLeaderRepo,
            IReportRepository reportRepo,
            IAssignmentRepository assignmentRepo,
            IReportsLogRepository reportsLogRepo,
            ICompanyRepository companyRepo, ISchoolRepository schoolRepo,
            IContactMessageRepository contactMessageRepo)
        {
            this.context = context;
            this.trainingRepo = trainingRepo;
            this.studentRepo = studentRepo;
            this.schoolSupervisorRepo = schoolSupervisorRepo;
            this.teamLeaderRepo = teamLeaderRepo;
            this.reportRepo = reportRepo;
            this.assignmentRepo = assignmentRepo;
            this.reportsLogRepo = reportsLogRepo;
            this.companyRepo = companyRepo;
            this.schoolRepo = schoolRepo;
            this.contactMessageRepo = contactMessageRepo;
        }

        public IActionResult Dashboard()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user_ = context.Users.Where(user => user.Id == Id).SingleOrDefault();
            var schools = schoolRepo.GetAllSchools();

            var studentsInSchools = schools.Select(school =>
                new
                {
                    SchoolName = school.shortName,
                    StudentCount = studentRepo.GetAllStudents().Count(student => student.schoolId == school.schoolId)
                }).ToList();

            ViewBag.trainings = trainingRepo.GetAllTrainings();
            ViewBag.students = studentRepo.GetAllStudents();
            ViewBag.schoolSupervisors = schoolSupervisorRepo.GetAllSchoolSupervisors();
            ViewBag.teamLeaders = teamLeaderRepo.GetAllTeamLeaders();
            ViewBag.assignments = assignmentRepo.GetAllAssignments();
            ViewBag.ApprovedCount = reportRepo.GetAllReports().Count(r => r.reportStatusId == 1);
            ViewBag.RejectedCount = reportRepo.GetAllReports().Count(r => r.reportStatusId == 2);
            ViewBag.PendingCount = reportRepo.GetAllReports().Count(r => r.reportStatusId == 3);
            ViewBag.SchoolsAndStudentCounts = studentsInSchools;
            ViewBag.lastThreeReports = reportRepo.GetAllReports()
             .OrderByDescending(r => r.reportId)
             .Take(3)
             .ToList();
            return View(user_);
        }
		public IActionResult Messages()
		{
			if (TempData.ContainsKey("SuccessMessage"))
			{
				ViewBag.SweetAlert = TempData["SuccessMessage"] as string;
			}
			ViewBag.Messages = contactMessageRepo.GetAllContactMessages();
			return View();
		}
		public IActionResult MessageReply(string subject, string message, int id)
		{
			ReplyToContactEmail(subject,message,id);
			TempData["SuccessMessage"] = "Message Sent Successfully";
			return RedirectToAction("Messages");
		}
		public async Task ReplyToContactEmail(string subject, string message, int id)
		{
            ContactMessage contactMessage = contactMessageRepo.GetContactMessage(id);
			var email = new MimeMessage();
			email.From.Add(new MailboxAddress(contactMessage.FullName, "apprenticeship.company@gmail.com"));
			email.To.Add(new MailboxAddress(contactMessage.FullName, contactMessage.EmailAddress));
			email.Subject = subject;

			var bodyBuilder = new MimeKit.BodyBuilder();
			bodyBuilder.TextBody = $"Dear {contactMessage.FullName},\n\n"
									   + $"{message}\n\n"
									   + "Best regards,\n"
			                           + "Apprenticeship Team";

			email.Body = bodyBuilder.ToMessageBody();
			contactMessageRepo.EditContactMessage(contactMessage);
			using (var smtp = new MailKit.Net.Smtp.SmtpClient())
			{
				smtp.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
				smtp.Connect("smtp.gmail.com", 587, false);
				smtp.Authenticate("apprenticeship.company@gmail.com", "scfaxbsmuxephejg");
				await smtp.SendAsync(email);
				smtp.Disconnect(true);
			}
		}
	}
}
