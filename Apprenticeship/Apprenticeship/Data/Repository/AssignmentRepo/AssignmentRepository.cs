using Apprenticeship.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Apprenticeship.Data.Repository.AssignmentObjectivesRepo;
using Apprenticeship.Data.Repository.DocumentRepo;
using Apprenticeship.Data.Repository.ReportRepo;
using Apprenticeship.Data.Repository.ReportsLogRepo;
namespace Apprenticeship.Data.Repository.AssignmentRepo
{
    public class AssignmentRepository : IAssignmentRepository
    {
        ApplicationDbContext context;
        IAssignmentObjectivesRepository assignmentObjectivesRepo;
        IDocumentRepository documentRepo;
        IReportRepository reportRepo;
		IReportsLogRepository reportsLogRepo;
		public AssignmentRepository(ApplicationDbContext context, IAssignmentObjectivesRepository assignmentObjectivesRepo, IDocumentRepository documentRepo, IReportRepository reportRepo,
        IReportsLogRepository reportsLogRepo)
        {
            this.context = context;
            this.assignmentObjectivesRepo = assignmentObjectivesRepo;
            this.documentRepo = documentRepo;
            this.reportRepo = reportRepo;
            this.reportsLogRepo = reportsLogRepo;
        }
        public void AddAssignment(Assignment assignment,List<int> objectiveIds)
        {
            context.assignments.Add(assignment);
            context.SaveChanges();
            var lastAssignment = context.assignments.OrderByDescending(a => a.assignmentId).FirstOrDefault();

            foreach (var objectiveId in objectiveIds)
            {
                var ao = new AssignmentObjectives();
                ao.objectiveId = objectiveId;
                ao.assignmentId = lastAssignment.assignmentId;
                assignmentObjectivesRepo.AddAssignmentObjective(ao);
            }
        }

        public void DeleteAssignment(int assignmentId, int trainingId)
        {
            var assignment = GetAssignment(assignmentId, trainingId);
			var assignmentObjectives = assignmentObjectivesRepo
		         .GetAllAssignmentObjectives()
		         .Where(t => t.assignmentId == assignmentId)
		         .ToList();
			foreach (var ao in assignmentObjectives)
            {
                assignmentObjectivesRepo.DeleteAssignmentObjective(ao.objectiveId,ao.assignmentId);
			}
			var documents = documentRepo.GetAllDocuments().Where(d => d.assignmentId == assignmentId).ToList();
			foreach (var d in documents)
			{
				documentRepo.DeleteDocument(d.documentId);
			}
            var reports = reportRepo.GetAllReports().Where(r => r.assignmentId == assignmentId).ToList();
            foreach (var r in reports)
            {
				var logs = reportsLogRepo.GetAllReportsLogs().Where(l => l.reportId == r.reportId).ToList();
				foreach (var l in logs)
				{
					reportsLogRepo.DeleteReportLog(l.reportLogId);
				}
				var reportsDocuments = documentRepo.GetAllDocuments().Where(d => d.reportId == r.reportId).ToList();
				foreach (var d in reportsDocuments)
				{
					documentRepo.DeleteDocument(d.documentId);
				}
			}
			context.assignments.Remove(assignment);
            context.SaveChanges();
        }

        public void EditAssignment(Assignment assignment)
        {
            var assignmentInfo = GetAssignment(assignment.assignmentId,assignment.trainingId);
            assignmentInfo.assignmentTitle = assignment.assignmentTitle;
            assignmentInfo.assignmentDescription = assignment.assignmentDescription;
            assignmentInfo.startDate = assignment.startDate;
            assignmentInfo.endDate = assignment.endDate;
            context.SaveChanges();
        }

        public List<Assignment> GetAllAssignments()
        {
            return context.assignments.Include(a=>a.assignmentObjectives).ThenInclude(o=>o.objective).Include(a=>a.training).ThenInclude(t=>t.student).ToList();
        }

        public Assignment GetAssignment(int assignmentId, int trainingId)
        {
            return context.assignments.Include(r => r.document).Where(a => a.assignmentId == assignmentId && a.trainingId==trainingId).SingleOrDefault();
        }
    }
}
