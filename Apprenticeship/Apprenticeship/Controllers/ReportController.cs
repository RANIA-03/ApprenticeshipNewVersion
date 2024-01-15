using Apprenticeship.Data.Entities;
using Apprenticeship.Data.Repository.DocumentRepo;
using Apprenticeship.Data.Repository.ReportRepo;
using Apprenticeship.Data.Repository.ReportsLogRepo;
using Apprenticeship.Data.Repository.ReportStatusRepo;
using Apprenticeship.Data.Repository.StudentRepo;
using Apprenticeship.Data.Repository.TeamLeaderRepo;
using Apprenticeship.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Apprenticeship.Controllers
{
    public class ReportController : Controller
    {
        IReportsLogRepository reportsLogRepo;
        IReportRepository reportRepo;
        IReportStatusRepository reportStatusRepo;
        IStudentRepository studentRepo;
		IDocumentRepository documentRepo;
        ITeamLeaderRepository teamLeaderRepo;
        public ReportController(IReportsLogRepository reportsLogRepo,IReportRepository reportRepo, IDocumentRepository documentRepo, IReportStatusRepository reportStatusRepo, IStudentRepository studentRepo,ITeamLeaderRepository teamLeaderRepo) {
            this.reportsLogRepo = reportsLogRepo;
            this.reportRepo = reportRepo;
			this.reportStatusRepo = reportStatusRepo;
			this.documentRepo = documentRepo;
            this.studentRepo = studentRepo;
            this.teamLeaderRepo = teamLeaderRepo;
        }
        [Authorize(Roles = "ADMIN")]
        public IActionResult Index()
        {
            var reports = reportRepo.GetAllReports();
            ViewBag.reports = reports;
            return View();
        }
        [Authorize(Roles = "STUDENT")]
        public IActionResult Add(int assignmentId,int trainingId)
        {
            ReportDTO report = new ReportDTO();
			report.assignmentId = assignmentId;
			ViewBag.trainingId = trainingId;
			return View(report);
        }
        //Create Report Row in Database
        [Authorize(Roles = "STUDENT")]
        [HttpPost]
        public IActionResult Insert(ReportDTO report, List<IFormFile> formFile,int trainingId)
        {
            if (ModelState.IsValid)
            {
                var report_ = new Report();
                report_.reportName = report.reportName;
                report_.reportDescription = report.reportDescription;
                report_.reportNotes = report.reportNotes;
                report_.assignmentId = report.assignmentId;
                report_.reportStatusId = 3;
                reportRepo.AddReport(report_);
                //Adding document to DB
                foreach (var form in formFile)
                {
                    Document document = new Document();
                    if (form.Length > 0)
                    {
                        Stream st = form.OpenReadStream();
                        using (BinaryReader br = new BinaryReader(st))//start
                        {
                            var byteFile = br.ReadBytes((int)st.Length);
                            document.file = byteFile;
                        }//end
                        document.name = form.FileName;
                        document.contentType = form.ContentType;
                        document.reportId = report_.reportId;
                        documentRepo.AddDocument(document);
                    }
                }

                return RedirectToAction("Reports", "Student", new { assignmentId = report.assignmentId,trainingId=trainingId });
            }
			ViewBag.trainingId = trainingId;
			return View("Add", report);
		}
		[Authorize(Roles = "ADMIN, TEAMLEADER, STUDENT, SCHOOLSUPERVISOR")]
		public FileStreamResult GetFile(long documentId)
		{
			var file = documentRepo.GetDocument(documentId);
			Stream stream = new MemoryStream(file.file);
			return new FileStreamResult(stream, file.contentType);
		}
		[Authorize(Roles = "STUDENT")]
        public IActionResult Edit(int reportId,int trainingId)
        {
            var report = reportRepo.GetReport(reportId);
			var report_ = new ReportDTO();
			report_.reportName = report.reportName;
			report_.reportDescription = report.reportDescription;
			report_.reportNotes = report.reportNotes;
			report_.assignmentId = report.assignmentId;
			report_.reportId = report.reportId;
			report_.reportStatusId = report.reportStatusId;
            ViewBag.documents = documentRepo.GetAllDocuments().Where(r => r.reportId == reportId).ToList();
            ViewBag.trainingId = trainingId;
            return View(report_);
        }
        //Edit Assignment Row in Database
        [Authorize(Roles = "STUDENT")]
        [HttpPost]
        public IActionResult Edited(ReportDTO report,List<IFormFile> formFile, int trainingId)
        {
            if (ModelState.IsValid)
            {
                var report_ = new Report();
                report_.reportName = report.reportName;
                report_.reportDescription = report.reportDescription;
                report_.reportNotes = report.reportNotes;
                report_.reportId = report.reportId;
                report_.reportStatusId = report.reportStatusId;
                reportRepo.EditReport(report_);

                //Adding document to DB
                foreach (var form in formFile)
                {
                    var reportLog = new ReportsLog();
                    reportLog.reportName = report.reportName;
                    reportLog.reportDescription = report.reportDescription;
                    reportLog.reportNotes = report.reportNotes;
                    reportLog.reportId = report.reportId;
                    reportLog.reportStatusId = report.reportStatusId;
                    reportLog.logDate = DateTime.Now;
                    reportLog.reportId = report.reportId;
                    reportsLogRepo.AddReportLog(reportLog);
                    Document document = new Document();
                    if (form.Length > 0)
                    {
                        Stream st = form.OpenReadStream();
                        using (BinaryReader br = new BinaryReader(st))//start
                        {
                            var byteFile = br.ReadBytes((int)st.Length);
                            document.file = byteFile;
                        }//end
                        document.name = form.FileName;
                        document.contentType = form.ContentType;
                        document.reportId = report_.reportId;
                        document.reportsLogId = reportLog.reportId;
                        documentRepo.AddDocument(document);
                    }
                }
                return RedirectToAction("Reports", "Student", new { assignmentId = report.assignmentId, trainingId = trainingId });
            }
			ViewBag.documents = documentRepo.GetAllDocuments().Where(r => r.reportId == report.reportId).ToList();
			return View("Edit", report);
		}
        [Authorize(Roles = "TEAMLEADER")]
        public IActionResult EditeReportStatus(ReportDTO report)
		{
			var report_ = new Report();
			report_.reportId= report.reportId;
			report_.reportStatusId= report.reportStatusId;
			reportRepo.EditReportStatusByTeamLeader(report_);

			return RedirectToAction("Reports", "TeamLeader", new { assignmentId = report.assignmentId});
		}
        //Delete Assignment Row in Database
        [Authorize(Roles = "STUDENT")]
        public IActionResult Delete(ReportDTO report, int trainingId)
        {
            reportRepo.DeleteReport(report.reportId);
			return RedirectToAction("Reports", "Student", new { assignmentId = report.assignmentId, trainingId = trainingId });
		}
        [Authorize(Roles = "STUDENT")]
        public IActionResult DeleteDocument(int reportId, int documentId)
        {
            documentRepo.DeleteDocument(documentId);
            return RedirectToAction("Edit", "Report", new { reportId });
        }
    }
}
