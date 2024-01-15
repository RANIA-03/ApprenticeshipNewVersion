using Apprenticeship.Data.Entities;
using Microsoft.EntityFrameworkCore;
namespace Apprenticeship.Data.Repository.ReportsLogRepo
{
	public class ReportsLogRepository : IReportsLogRepository
	{
		public ApplicationDbContext context;
		public ReportsLogRepository(ApplicationDbContext context)
		{
			this.context = context;
		}
		public void AddReportLog(ReportsLog reportLog)
		{
			context.reportsLogs.Add(reportLog);
			context.SaveChanges();
		}

		public void DeleteReportLog(int reportLogId)
		{
			var reportlog = GetReportLog(reportLogId);
			context.reportsLogs.Remove(reportlog);
			context.SaveChanges();
		}

		public void EditReportLog(ReportsLog reportLog)
		{
			var reportLogInfo = GetReportLog(reportLog.reportLogId);
			reportLogInfo.reportName = reportLog.reportName;
			reportLogInfo.reportDescription = reportLog.reportDescription;
			reportLogInfo.reportNotes = reportLog.reportNotes;
			reportLogInfo.reportId = reportLog.reportId;
			reportLogInfo.logDate = reportLog.logDate;
			reportLogInfo.reportStatusId = reportLog.reportStatusId;
			context.SaveChanges();
		}

		public List<ReportsLog> GetAllReportsLogs()
		{
			return context.reportsLogs
				.Include(r => r.report)
				.Include(r => r.reportStatus)
				.Include(r => r.document)
				.ToList();
		}

		public ReportsLog GetReportLog(int reportLogId)
		{
			return context.reportsLogs
				.Include(r => r.report)
				.Include(r => r.reportStatus)
				.Include(r => r.document)
				.Where(r => r.reportLogId == reportLogId)
				.SingleOrDefault();
		}
	}
}
