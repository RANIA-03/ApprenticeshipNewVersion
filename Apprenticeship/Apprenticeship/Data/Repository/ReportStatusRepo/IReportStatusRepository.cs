using Apprenticeship.Data.Entities;

namespace Apprenticeship.Data.Repository.ReportStatusRepo
{
	public interface IReportStatusRepository
	{
		public List<ReportStatus> GetAllReportStatuses();
		public void DeleteReportStatus(int reportStatusId);
		public ReportStatus GetReportStatus(int reportStatusId);
		public void EditReportStatus(ReportStatus reportStatus);
		public void AddReportStatus(ReportStatus reportStatus);
	}
}
