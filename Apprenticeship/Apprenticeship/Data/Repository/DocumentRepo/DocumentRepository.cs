using Apprenticeship.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace Apprenticeship.Data.Repository.DocumentRepo
{
	public class DocumentRepository : IDocumentRepository
	{
		ApplicationDbContext context;
		public DocumentRepository(ApplicationDbContext context)
		{
			this.context = context;
		}
        public void AddDocument(Document document)
        {
			var lastRow = 
			document.reportId = document.reportId != 0 ? document.reportId : null;
			document.reportsLogId = document.reportId != null ? context.reportsLogs.Max(e => e.reportLogId) : null;
			document.assignmentId = document.assignmentId != 0 ? document.assignmentId : null;
            context.documents.Add(document);
            context.SaveChanges();
        }


        public void DeleteDocument(long documentId)
		{
			var document = GetDocument(documentId);
			context.documents.Remove(document);
			context.SaveChanges();
		}

		public void EditDocument(Document document)
		{
			var documentInfo = GetDocument(document.documentId);
			documentInfo.name = document.name;
			documentInfo.contentType = document.contentType;
			documentInfo.file = document.file;
			context.SaveChanges();
		}

		public List<Document> GetAllDocuments()
		{
			return context.documents.ToList();
		}

		public Document GetDocument(long documentId)
		{
			var documentInfo = context.documents.Where(d => d.documentId == documentId).SingleOrDefault();
			return documentInfo;
		}
	}
}
