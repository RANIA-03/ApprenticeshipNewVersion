using Apprenticeship.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Apprenticeship.DTO
{
    public class DocumentDTO
    {
        public int documentId { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string contentType { get; set; }
        public byte[] file { get; set; }
        public int reportId { get; set; }
        public int assignmentId { get; set; }
        public int reportsLogId { get; set; }
    }
}
