using Apprenticeship.Data.Entities;
using Apprenticeship.Data.Repository.DocumentRepo;
using Apprenticeship.Data.Repository.ReportsLogRepo;
using Microsoft.EntityFrameworkCore;

namespace Apprenticeship.Data.Repository.ReportRepo
{
    public class ReportRepository : IReportRepository
    {
        public ApplicationDbContext context;
        IReportsLogRepository reportsLogRepo;
		IDocumentRepository documentRepo;

		public ReportRepository(ApplicationDbContext context, IReportsLogRepository reportsLogRepo, IDocumentRepository documentRepo)
        {
            this.context = context;
            this.reportsLogRepo = reportsLogRepo;
			this.documentRepo = documentRepo;
        }
        public void AddReport(Report report)
        {
			context.reports.Add(report);
			context.SaveChanges();
			var reportIdLastOne = context.reports.OrderByDescending(a => a.reportId).FirstOrDefault().reportId;
			var reportLog = new ReportsLog();
			reportLog.reportName = report.reportName;
			reportLog.reportDescription = report.reportDescription;
			reportLog.reportNotes= report.reportNotes;
			reportLog.reportId= reportIdLastOne;
			reportLog.reportStatusId= report.reportStatusId;
			reportLog.logDate= DateTime.Now;
			reportsLogRepo.AddReportLog(reportLog);
		}

        public void DeleteReport(int reportId)
        {
			var logs = reportsLogRepo.GetAllReportsLogs().Where(r => r.reportId == reportId).ToList();
			foreach (var l in logs)
			{
				reportsLogRepo.DeleteReportLog(l.reportLogId);
			}
			var documents = documentRepo.GetAllDocuments().Where(d => d.reportId == reportId).ToList();
			foreach (var d in documents)
			{
				documentRepo.DeleteDocument(d.documentId);
			}
			var report = GetReport(reportId);
            context.reports.Remove(report);
            context.SaveChanges();
        }

		public void EditReport(Report report)
		{
			var reportInfo = GetReport(report.reportId);
			reportInfo.reportName = report.reportName;
			reportInfo.reportDescription = report.reportDescription;
			reportInfo.reportNotes = report.reportNotes;
			reportInfo.reportStatusId = report.reportStatusId;
			context.SaveChanges();
			var reportLog = new ReportsLog();
			reportLog.reportName = report.reportName;
			reportLog.reportDescription = report.reportDescription;
			reportLog.reportNotes = report.reportNotes;
			reportLog.reportId = report.reportId;
			reportLog.reportStatusId = report.reportStatusId;
			reportLog.logDate = DateTime.Now;
			reportsLogRepo.AddReportLog(reportLog);
		}
		public void EditReportStatusByTeamLeader(Report report)
		{
			var reportInfo = GetReport(report.reportId);
			reportInfo.reportStatusId = report.reportStatusId;
			context.SaveChanges();
            var reportLog = new ReportsLog();
			reportLog.reportName = reportInfo.reportName;
			reportLog.reportDescription = reportInfo.reportDescription;
			reportLog.reportNotes = reportInfo.reportNotes;
			reportLog.reportId = reportInfo.reportId;
			reportLog.reportStatusId = reportInfo.reportStatusId;
			reportLog.logDate = DateTime.Now;
			var document = documentRepo.GetAllDocuments().Where(d => d.reportId == report.reportId).SingleOrDefault();
			reportsLogRepo.AddReportLog(reportLog);
			//Document newDocument = new Document();
   //         newDocument.reportsLogId = reportLog.reportLogId;
   //         newDocument.reportId = report.reportId;
   //         newDocument.name = document.name == null ? "":document.name;
   //         newDocument.contentType= document.contentType;
   //         newDocument.file = document.file;
   //         documentRepo.AddDocument(newDocument);
        }

        public List<Report> GetAllReports()
        {
            return context.reports.Include(r => r.assignment).ThenInclude(r => r.training).Include(r => r.reportStatus).Include(r => r.document).ToList();
        }

        public Report GetReport(int reportId)
        {
            return context.reports.Include(r => r.assignment).Include(r => r.document).Include(r => r.reportStatus).Where(r => r.reportId == reportId).SingleOrDefault();
        }

    }
}
