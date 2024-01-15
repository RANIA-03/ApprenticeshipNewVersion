using Apprenticeship.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Apprenticeship.DTO
{
    public class ReportDTO
    {
        public int reportId { get; set; }
        [Required]
        public string reportName { get; set; }
        [Required]
        public string reportDescription { get; set; }
        public string reportNotes { get; set; }
        public int assignmentId { get; set; }
        public int reportStatusId { get; set; }
    }
}
