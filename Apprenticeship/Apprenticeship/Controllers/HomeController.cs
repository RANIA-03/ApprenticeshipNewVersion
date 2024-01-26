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
using NuGet.Protocol.Plugins;
using Apprenticeship.Data.Entities;
using Apprenticeship.Data.Repository.ContactMessageRepo;
using Microsoft.EntityFrameworkCore;
using MimeKit;

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
        IContactMessageRepository contactMessageRepo;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context,
            ITrainingRepository trainingRepo,
            IStudentRepository studentRepo,
            ISchoolSupervisorRepository schoolSupervisorRepo,
            ITeamLeaderRepository teamLeaderRepo,
            IReportRepository reportRepo,
            IAssignmentRepository assignmentRepo,
            IReportsLogRepository reportsLogRepo,
            ICompanyRepository companyRepo,
            IContactMessageRepository contactMessageRepo)
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
            this.contactMessageRepo = contactMessageRepo;
        }
        public IActionResult Index()
        {
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SweetAlert = TempData["SuccessMessage"] as string;
            }
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

        [HttpPost]
        public IActionResult Contact(ContactMessage contactMessage)
        {
            contactMessageRepo.AddContactMessage(contactMessage);
            SendContactMessageConfirmationEmail(contactMessage);
            TempData["SuccessMessage"] = "Message Sent Successfully";
            return RedirectToAction("Index");
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
        public async Task SendContactMessageConfirmationEmail(ContactMessage contactMessage)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(contactMessage.FullName, "apprenticeship.company@gmail.com"));
            email.To.Add(new MailboxAddress(contactMessage.FullName, contactMessage.EmailAddress));
            email.Subject = contactMessage.EmailSubject;

            var bodyBuilder = new MimeKit.BodyBuilder();
            bodyBuilder.TextBody = $"Dear {contactMessage.FullName},\n\n"
                             + "Thank you for reaching out!\n"
                             + $"Your message has been received with the following details:\n"
                             + $"Email Address: {contactMessage.EmailAddress}\n"
                             + $"Mobile Number: {contactMessage.MobileNumber}\n"
                             + $"Email Subject: {contactMessage.EmailSubject}\n"
                             + $"Message: {contactMessage.Msg}\n\n"
                             + "We appreciate your interest and will get back to you as soon as possible!\n\n"
                             + "Best regards,\n"
                             + "Apprenticeship Team";

            email.Body = bodyBuilder.ToMessageBody();
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
