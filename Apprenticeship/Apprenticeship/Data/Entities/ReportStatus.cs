namespace Apprenticeship.Data.Entities
{
    public class ReportStatus // Lookup table
    {
        public int reportStatusId { get; set; }
        public string reportStatusName { get; set; }

        public List<Report> reports { get; set; }
    }
}
