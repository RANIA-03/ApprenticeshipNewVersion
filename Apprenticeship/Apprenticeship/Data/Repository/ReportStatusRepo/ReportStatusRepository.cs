using Apprenticeship.Data.Entities;
using System.Security.AccessControl;

namespace Apprenticeship.Data.Repository.ReportStatusRepo
{
	public class ReportStatusRepository : IReportStatusRepository
	{
		ApplicationDbContext context;
		public ReportStatusRepository(ApplicationDbContext context)
		{
			this.context = context;
		}
		public void AddReportStatus(ReportStatus reportStatus)
		{
			context.reportStatuses.Add(reportStatus);
			context.SaveChanges();
		}

		public void DeleteReportStatus(int reportStatusId)
		{
			var reportStatus = GetReportStatus(reportStatusId);
			context.reportStatuses.Remove(reportStatus);
			context.SaveChanges();
		}

		public void EditReportStatus(ReportStatus reportStatus)
		{
			var reportStatuseInfo = GetReportStatus(reportStatus.reportStatusId);
			reportStatuseInfo.reportStatusName = reportStatus.reportStatusName;
			context.SaveChanges();
		}

		public List<ReportStatus> GetAllReportStatuses()
		{
			return context.reportStatuses.ToList();
		}

		public ReportStatus GetReportStatus(int reportStatusId)
		{
			var reportStatuseInfo = context.reportStatuses.Where(o => o.reportStatusId == reportStatusId).SingleOrDefault();
			return reportStatuseInfo;
		}
	}
}
