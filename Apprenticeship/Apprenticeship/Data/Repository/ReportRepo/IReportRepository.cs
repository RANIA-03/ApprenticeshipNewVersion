using Apprenticeship.Data.Entities;

namespace Apprenticeship.Data.Repository.ReportRepo
{
    public interface IReportRepository
    { 
            public List<Report> GetAllReports();
            public void DeleteReport(int reportId);
            public Report GetReport(int reportId);
            public void EditReport(Report report);
            public void AddReport(Report report);
            public void EditReportStatusByTeamLeader(Report report);

	}
}
